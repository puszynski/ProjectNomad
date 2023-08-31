using GameModule.Entities;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ISecundExecutor
    {
        Task<IEnumerable<INotification>> Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            List<MapTile> mapTilesToConsumeTasks,
            DateTime currentTimeInLoop);
    }

    internal class SecundExecutor : ISecundExecutor
    {
        readonly IHumanUnitTaskConsumer _humanUnitTaskConsumer;
        readonly IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        public SecundExecutor(IHumanUnitTaskConsumer humanUnitTaskConsumer, 
            IHumanUnitAutoTaskScheduler humanUnitAutoTaskScheduler)
        {
            _humanUnitTaskConsumer = humanUnitTaskConsumer;
            _humanUnitAutoTaskScheduler = humanUnitAutoTaskScheduler;
        }

        async Task<IEnumerable<INotification>> ISecundExecutor.Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            List<MapTile> mapTilesToConsumeTasks,
            DateTime currentTimeInLoop)
        {
            var notificationToSendToClient = new List<INotification>();

            var humanUnitAutoTaskSchedulerTask = _humanUnitAutoTaskScheduler.Execute(humanUnits,
                tribe,
                tasksToConsume,
                currentTimeInLoop);

            var humanUnitTaskConsumerNotifications = _humanUnitTaskConsumer.Execute(humanUnits,
                tribe,
                tasksToConsume,
                mapTilesToConsumeTasks,
                currentTimeInLoop);

            notificationToSendToClient.AddRange(await humanUnitAutoTaskSchedulerTask);
            notificationToSendToClient.AddRange(humanUnitTaskConsumerNotifications);

            return notificationToSendToClient;
        }
    }
}
