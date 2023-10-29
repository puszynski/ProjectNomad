using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.HourExecutorLogic;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHourExecutor
    {
        void Execute(Tribe tribe, ICollection<MapTile> mapTiles, List<INotification> notifications);
    }
    internal class HourExecutor : IHourExecutor
    {
        readonly MapTileRegenerator _mapTileRegenerator;
        readonly BreedingApplicator _breedingApplicator;
        readonly IHumansDeathApplicator _humansDeathApplicator;
        public HourExecutor(MapTileRegenerator mapTileRegenerator,
            BreedingApplicator breedingApplicator,
            IHumansDeathApplicator humansDeathApplicator)
        {
            _mapTileRegenerator = mapTileRegenerator;
            _breedingApplicator = breedingApplicator;
            _humansDeathApplicator = humansDeathApplicator;
        }

        void IHourExecutor.Execute(Tribe tribe, ICollection<MapTile> mapTiles, List<INotification> notifications)
        {
            var inProgressHumanTasks = tribe.HumanTasks.Where(x => !x.IsCompleted).ToList();
            tribe.HumanTasks = inProgressHumanTasks;

            _mapTileRegenerator.Execute(mapTiles);
            _breedingApplicator.Execute(tribe, notifications);
            _humansDeathApplicator.DeathFromAgeOrIllness(tribe.Humans, notifications);
        }
    }
}
