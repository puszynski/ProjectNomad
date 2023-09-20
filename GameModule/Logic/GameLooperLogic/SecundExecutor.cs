using GameModule.Entities;
using ProjectNomad.Shared.Interfaces;
using System.Windows.Markup;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ISecundExecutor
    {
        Task Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications,
            DateTime currentTimeInLoop);
    }

    internal class SecundExecutor : ISecundExecutor
    {
        readonly ITaskAssigner _taskAssigner;
        readonly ITaskConsumer _humanUnitTaskConsumer;
        readonly IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        public SecundExecutor(ITaskAssigner taskAssigner,
            ITaskConsumer humanUnitTaskConsumer,
            IHumanUnitAutoTaskScheduler humanUnitAutoTaskScheduler)
        {
            _taskAssigner = taskAssigner;
            _humanUnitTaskConsumer = humanUnitTaskConsumer;
            _humanUnitAutoTaskScheduler = humanUnitAutoTaskScheduler;
        }

        async Task ISecundExecutor.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications,
            DateTime currentTimeInLoop)
        {
            await _taskAssigner.Execute(tribe, mapTiles, notifications);
            await _humanUnitAutoTaskScheduler.Execute(tribe, notifications, currentTimeInLoop);
            _humanUnitTaskConsumer.Execute(tribe, mapTiles, currentTimeInLoop, notifications);

            //todo end of relocation?


        }
    }
}
