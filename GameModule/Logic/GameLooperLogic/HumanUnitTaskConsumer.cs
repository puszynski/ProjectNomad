using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Enums;

namespace GameModule.Logic.GameLooperLogic
{
    internal class HumanUnitTaskConsumer
    {
        readonly GameModuleDbContext _dbContext;
        public HumanUnitTaskConsumer(GameModuleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal async Task Execute(IEnumerable<HumanUnit> humanUnits, 
            Tribe tribe)
        {
            var humanUnitIds = humanUnits
                .Select(x => x.Id)
                .ToList();

            var tasksToConsume = await _dbContext.HumanUnitTasks
                .Where(x => humanUnitIds.Contains(x.HumanUnitId))
                .Where(x => x.To >= DateTime.UtcNow)
                .ToListAsync();


            foreach (var task in tasksToConsume) 
            {
                var mapTile = await _dbContext.MapTiles.SingleAsync(x => x.Id == task.MapTileId);
                
                await ConsumeTask(task.Type, 
                    humanUnits.Single(x => x.Id == task.HumanUnitId), 
                    tribe,
                    mapTile.Food.ActualPoints);

                _dbContext.HumanUnitTasks.Remove(task);
            }
        }

        async Task ConsumeTask(EHumanUnitTaskType eHumanUnitTaskType,
            HumanUnit humanUnit,
            Tribe tribe,
            int mapTileFoodPoints) //todo UT
        {
            switch (eHumanUnitTaskType)
            {
                //todo think..
                case EHumanUnitTaskType.GatheringFood:
                    const int MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK = 5;

                    var foodGatheringCoefficient = (double)humanUnit.FoodLevelPercentage / 100 * 2;
                    var foodPoints = foodGatheringCoefficient >= 1 
                        ? MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK
                        : foodGatheringCoefficient * MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK;

                    var gatheredFoodPoints = mapTileFoodPoints > foodPoints 
                        ? foodPoints 
                        : mapTileFoodPoints;

                    tribe.Resources.FreshFood += (int)gatheredFoodPoints; //todo tests
                    break;
            }
        }
    }
}
