using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
using GameModule.Logic.TasksLogic;
using ProjectNomad.Shared.Interfaces;

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
        readonly ITribeRelocationService _tribeRelocationService;
        readonly IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        public SecundExecutor(
            ITaskAssigner taskAssigner,
            ITaskConsumer humanUnitTaskConsumer,
            ITribeRelocationService tribeRelocationService,
            IHumanUnitAutoTaskScheduler humanUnitAutoTaskScheduler)
        {
            _taskAssigner = taskAssigner;
            _humanUnitTaskConsumer = humanUnitTaskConsumer;
            _tribeRelocationService = tribeRelocationService;
            _humanUnitAutoTaskScheduler = humanUnitAutoTaskScheduler;
        }

        async Task ISecundExecutor.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications,
            DateTime currentTimeInLoop)
        {
            await _taskAssigner.Execute(tribe, mapTiles, notifications, currentTimeInLoop);
            //await _humanUnitAutoTaskScheduler.Execute(tribe, notifications, currentTimeInLoop); //todo remove codes
            _humanUnitTaskConsumer.Execute(tribe, mapTiles, currentTimeInLoop, notifications);
            _tribeRelocationService.EndRelocationProcess(tribe);


            var campfire = tribe
                .TribeStructures
                .SingleOrDefault(x => x.Type == ProjectNomad.Shared.Enums.ETribeStructureType.Firecamp);
            LightFire.CampfireBurning(campfire);
        }
    }
}
