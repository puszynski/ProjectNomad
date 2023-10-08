using GameModule.Entities;
using GameModule.Logic.TasksLogic;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
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
            var humanUnitIds = tribe.HumanUnits
                .Select(x => x.Id)
                .ToList();

            var tasksToConsume = tribe.HumanUnitTasks
                .Where(x => humanUnitIds.Contains(x.HumanUnitId))
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

                tribe.HumanUnitTasks.Remove(task);

                var finishedHumanTaskOrder = tribe.HumanUnitTaskOrders
                    .Where(x => x.IsInProgress && x.Type == task.Type)
                    .OrderBy(x => x.Added).FirstOrDefault();
                if (finishedHumanTaskOrder != null)
                    tribe.HumanUnitTaskOrders.Remove(finishedHumanTaskOrder);
                else { /*todo log - finishedHumanTaskOrder should always exists, if its null its due to problem - happens 2 times..*/  }
            }
        }

        INotification? ConsumeTask(HumanUnitTask humanUnitTask,
            Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop)
        {
            ITask consumer;

            switch (humanUnitTask.Type)
            {
                case EHumanUnitTaskType.GatheringFood:
                    consumer = new GatheringFood();
                    break;
                case EHumanUnitTaskType.GatheringWood:
                    consumer = new GatheringWood();
                    break;
                case EHumanUnitTaskType.LightAFire:
                    consumer = new LightFire();//GDY TASK TRWA TO MUSI TU WPADAC, NIE USTAWIA NA IsInProg = false and removing task order... KURDE NIE POWINNO TU WEJSC POKI TASK SIE NIE KONCZY, TO CZEMU WSKOCZYŁO W TRAKCIE??
                    break;
                case EHumanUnitTaskType.KeepFire:
                    consumer = new KeepFire();
                    break;
                case EHumanUnitTaskType.ConsumeFood:
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
