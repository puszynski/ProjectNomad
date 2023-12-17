using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.TasksLogic;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.SharedExecutorLogic
{
    internal interface ITaskConsumer
    {
        void Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop,
            List<INotification> notifications);
    }

    internal class TaskConsumer : ITaskConsumer
    {
        readonly GatheringFood _gatheringFood;
        readonly GatheringWood _gatheringWood;
        public TaskConsumer(GatheringFood gatheringFood, GatheringWood gatheringWood)
        {
            _gatheringFood = gatheringFood;
            _gatheringWood = gatheringWood;
        }

        void ITaskConsumer.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop,
            List<INotification> notifications)
        {
            if (tribe.Humans == null || !tribe.Humans.Any() || tribe.HumanTasks == null || !tribe.HumanTasks.Any())
                return;

            var tasksToConsume = tribe.Humans
                .Where(x => x.HumanUnitTask != null)
                .Select(x => x.HumanUnitTask)
                .Where(x => x.To <= currentTimeInLoop)
                .ToList();

            foreach (var task in tasksToConsume)
            {
                var notification = ConsumeTask(task,
                    tribe,
                    mapTiles,
                    currentTimeInLoop);

                if (notification != null)
                    notifications.Add(notification);
            }
        }

        INotification? ConsumeTask(HumanTask taskToEnd,
            Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop)
        {
            ITaskEnd taskEnd;

            switch (taskToEnd.Type)
            {
                case ETaskType.GatheringFood:
                    taskEnd = _gatheringFood;
                    break;
                case ETaskType.GatheringWood:
                    taskEnd = _gatheringWood;
                    break;
                case ETaskType.CampfireUp:
                    taskEnd = new Campfire();
                    break;
                case ETaskType.ConsumeFood:
                    taskEnd = new ConsumeFood();
                    break;
                case ETaskType.HeatUpHumanByFireEnded:
                    taskEnd = new HeatUpByFire();
                    break;
                case ETaskType.Sleep:
                    taskEnd = new Sleep();
                    break;

                default: throw new NotImplementedException();
            }
            
            var notification = taskEnd.End(taskToEnd,
                    tribe,
                    currentTimeInLoop,
                    mapTiles);

            tribe.HumanTasks?.Remove(taskToEnd); //może to wystarczy żeby usunać i encje i referencje z human??
            //humanUnitTask.Human.HumanUnitTask = null;
            //await _humanTasks.Remove(humanUnitTask); //todo

            return notification;
        }
    }
}
