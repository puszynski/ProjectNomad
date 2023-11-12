using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.HourExecutorLogic;
using GameModule.Logic.GameLooperLogic.SharedExecutorLogic;
using GameModule.Logic.Services;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHourExecutor
    {
        void Execute(Tribe tribe, 
            ICollection<MapTile> mapTiles,
            WorldZoneParameter worldZoneParameter, 
            List<INotification> notifications);
    }
    internal class HourExecutor : IHourExecutor
    {
        readonly MapTileRegenerator _mapTileRegenerator;
        readonly BreedingApplicator _breedingApplicator;
        readonly IHumansDeathApplicator _humansDeathApplicator;
        readonly WeatherAndThermalService _weatherAndThermalService;

        public HourExecutor(MapTileRegenerator mapTileRegenerator,
            BreedingApplicator breedingApplicator,
            IHumansDeathApplicator humansDeathApplicator,
            WeatherAndThermalService weatherAndThermalService)
        {
            _mapTileRegenerator = mapTileRegenerator;
            _breedingApplicator = breedingApplicator;
            _humansDeathApplicator = humansDeathApplicator;
            _weatherAndThermalService = weatherAndThermalService;
        }

        void IHourExecutor.Execute(Tribe tribe, 
            ICollection<MapTile> mapTiles, 
            WorldZoneParameter worldZoneParameter, 
            List<INotification> notifications)
        {
            if (tribe.HumanTasks == null)
                return;

            var inProgressHumanTasks = tribe.HumanTasks.Where(x => !x.IsCompleted).ToList();//removing completed tasks
            tribe.HumanTasks = inProgressHumanTasks;

            _mapTileRegenerator.Execute(mapTiles);
            _breedingApplicator.Execute(tribe, notifications); 

            if (tribe.Humans != null)
                _humansDeathApplicator.AgeOrIllnessDeath(tribe.Humans, notifications);

            //temp cancel to check if it provides errors
            //_weatherAndThermalService.UpdateWorldZoneParameters(worldZoneParameter);//UWAGA UWAGA! GAME LOOPER BĘDZIE URUCHAMIANY PER MEMBER - A TO MUSI BYĆ PER ALIKACJA!!! TODOOOO todo
        }
    }
}
