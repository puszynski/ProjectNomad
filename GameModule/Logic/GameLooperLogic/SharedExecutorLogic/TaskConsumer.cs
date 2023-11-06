using GameModule.Entities;
using GameModule.Logic.TasksLogic;
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
        void ITaskConsumer.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop,
            List<INotification> notifications)
        {
            var humanUnitIds = tribe.Humans
                .Select(x => x.Id)
                .ToList();

            var tasksToConsume = tribe.HumanTasks
                .Where(x => humanUnitIds.Contains(x.HumanId))
                .Where(x => x.To <= currentTimeInLoop && !x.IsCompleted)
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

        INotification? ConsumeTask(HumanTask humanUnitTask,
            Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop)
        {
            ITask consumer;

            switch (humanUnitTask.Type)
            {
                case ETaskType.GatheringFood:
                    consumer = new GatheringFood();
                    break;
                case ETaskType.GatheringWood:
                    consumer = new GatheringWood();
                    break;
                case ETaskType.LightAFire:
                    consumer = new LightFire();
                    break;
                case ETaskType.KeepFire:
                    consumer = new KeepFire();
                    break;
                case ETaskType.ConsumeFood:
                    consumer = new ConsumeFood();
                    break;

                default: throw new NotImplementedException();
            }

            return consumer.End(humanUnitTask,
                tribe,
                currentTimeInLoop,
                mapTiles);
        }
    }
}
