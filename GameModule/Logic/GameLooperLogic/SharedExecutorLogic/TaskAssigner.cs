using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
using GameModule.Logic.TasksLogic;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.SharedExecutorLogic
{
    internal interface ITaskAssigner
    {
        Task Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications,
            DateTime currentTimeInLoop);
    }

    internal class TaskAssigner : ITaskAssigner
    {
        readonly ITribeRelocationService _relocationService;
        public TaskAssigner(ITribeRelocationService relocationService)
        {
            _relocationService = relocationService;
        }

        async Task ITaskAssigner.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications,
            DateTime currentTimeInLoop)
        {
            AutoTaskAssign();
            OrderTaskAssign();

            void AutoTaskAssign()
            {
                foreach (var autoTasks in HumanUnitTaskTypeExtensions.GetAutoTasks())
                {
                    ITask? assigner = null;

                    switch (autoTasks)
                    {
                        case ETaskType.ConsumeFood:
                            assigner = new ConsumeFood();
                            break;
                        case ETaskType.HeatUpHumanByFireEnded:
                            assigner = new HeatUpByFire();
                            break;
                    }

                    var notification = assigner?.Start(null, tribe, currentTimeInLoop, mapTiles);

                    if (notification != null)
                        notifications.Add(notification);
                }
            }

            void OrderTaskAssign()
            {
                var taskOrdersToAssign = tribe.HumanTaskOrders
                    .Where(x => !x.HumanTaskId.HasValue)
                    .ToList();

                foreach (var taskOrder in taskOrdersToAssign)
                {
                    ITask? assigner = null;

                    switch (taskOrder.Type)
                    {
                        case ETaskType.GatheringFood:
                            assigner = new GatheringFood();
                            break;
                        case ETaskType.GatheringWood:
                            assigner = new GatheringWood();
                            break;
                        case ETaskType.LightAFire:
                            assigner = new LightFire();
                            break;
                        case ETaskType.KeepFire:
                            assigner = new KeepFire();
                            break;
                        case ETaskType.TribeRelocation:
                            TribeRelocationTasksAssign(taskOrder, tribe);
                            break;
                    }

                    var notification = assigner?.Start(taskOrder, tribe, currentTimeInLoop, mapTiles);

                    if (notification != null)
                        notifications.Add(notification);
                }
            }
        }



        void TribeRelocationTasksAssign(HumanTaskOrder taskOrder, Tribe tribe)
        {
            if (!_relocationService.IsValidToStartRelocationProcess(tribe))
                return;

            _relocationService.StartRelocationProcess(taskOrder, tribe);
        }
    }
}
