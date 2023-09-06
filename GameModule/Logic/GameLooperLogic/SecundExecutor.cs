using GameModule.Entities;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ISecundExecutor
    {
        Task<IEnumerable<INotification>> Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasks,
            List<HumanUnitTaskOrder> taskOrders,
            List<MapTile> mapTiles,
            DateTime currentTimeInLoop);
    }

    internal class SecundExecutor : ISecundExecutor
    {
        readonly ITaskAssigner _taskAssigner;
        readonly IHumanUnitTaskConsumer _humanUnitTaskConsumer;
        readonly IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        public SecundExecutor(ITaskAssigner taskAssigner,
            IHumanUnitTaskConsumer humanUnitTaskConsumer,
            IHumanUnitAutoTaskScheduler humanUnitAutoTaskScheduler)
        {
            _taskAssigner = taskAssigner;
            _humanUnitTaskConsumer = humanUnitTaskConsumer;
            _humanUnitAutoTaskScheduler = humanUnitAutoTaskScheduler;
        }

        async Task<IEnumerable<INotification>> ISecundExecutor.Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasks,
            List<HumanUnitTaskOrder> taskOrders,
            List<MapTile> mapTiles,
            DateTime currentTimeInLoop)
        {
            var notificationToSendToClient = new List<INotification>();

            var taskAssignerTask = _taskAssigner.Execute(taskOrders,
                tasks, 
                humanUnits, 
                tribe, 
                mapTiles);

            var humanUnitAutoTaskSchedulerTask = _humanUnitAutoTaskScheduler.Execute(humanUnits,
                tribe,
                tasks,
                currentTimeInLoop);

            var humanUnitTaskConsumerNotifications = _humanUnitTaskConsumer.Execute(humanUnits,
                tribe,
                tasks,
                mapTiles,
                currentTimeInLoop);

            notificationToSendToClient.AddRange(await taskAssignerTask);
            notificationToSendToClient.AddRange(await humanUnitAutoTaskSchedulerTask);
            notificationToSendToClient.AddRange(humanUnitTaskConsumerNotifications);

            return notificationToSendToClient;
        }
    }
}
