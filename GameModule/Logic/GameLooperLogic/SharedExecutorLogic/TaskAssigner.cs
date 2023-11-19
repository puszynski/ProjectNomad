using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
using GameModule.Logic.TasksLogic;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Logic;

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
            if (tribe.Humans == null || !tribe.Humans.Any())
                return;

            AutoTaskAssign();
            OrderTaskAssign();

            void AutoTaskAssign()
            {
                foreach (var autoTasks in TaskTypeExtensions.GetAutoTasks())
                {
                    ITask? assigner = null;

                    if (DayNightService.IsNight(currentTimeInLoop))
                    {
                        switch (autoTasks)
                        {
                            case ETaskType.Sleep:
                                assigner = new Sleep();
                                break;
                        }
                    }
                    else
                    {
                        switch (autoTasks)
                        {
                            case ETaskType.ConsumeFood:
                                assigner = new ConsumeFood();
                                break;
                            case ETaskType.HeatUpHumanByFireEnded:
                                assigner = new HeatUpByFire();
                                break;
                        }
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
                    .OrderBy(x => x.Type)
                    .ToList();

                foreach (var taskOrder in taskOrdersToAssign)
                {
                    ITask? assigner = null;

                    if (DayNightService.IsNight(currentTimeInLoop))
                    {
                        switch (taskOrder.Type)
                        {
                            case ETaskType.CampfireUp:
                                assigner = new Campfire();
                                break;
                            case ETaskType.Sleep:
                                assigner = new Sleep();
                                break;
                        }
                    }
                    else
                    {
                        switch (taskOrder.Type)
                        {
                            case ETaskType.GatheringFood:
                                assigner = new GatheringFood();
                                break;
                            case ETaskType.GatheringWood:
                                assigner = new GatheringWood();
                                break;
                            case ETaskType.CampfireUp:
                                assigner = new Campfire();
                                break;
                            case ETaskType.TribeRelocation:
                                TribeRelocationTasksAssign(taskOrder, tribe);
                                break;
                        }
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
