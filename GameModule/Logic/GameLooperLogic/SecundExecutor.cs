using GameModule.Entities;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ISecundExecutor
    {
        Task Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            List<MapTile> mapTilesToConsumeTasks);
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

        async Task ISecundExecutor.Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            List<MapTile> mapTilesToConsumeTasks)
        {
            var humanUnitAutoTaskSchedulerTask = _humanUnitAutoTaskScheduler.Execute(humanUnits,
                tribe,
                tasksToConsume);

            _humanUnitTaskConsumer.Execute(humanUnits,
                tribe,
                tasksToConsume,
                mapTilesToConsumeTasks);

            await humanUnitAutoTaskSchedulerTask;
        }
    }
}
