using GameModule.Entities;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic.HourExecutorLogic
{
    internal class MapTileRegenerator
    {
        internal void Execute(ICollection<MapTile> mapTiles)
        {
            foreach (var mapTile in mapTiles)
            {
                mapTile.Food.ActualPoints = RegenerateActualPoints(mapTile.Food.ActualPoints,
                    mapTile.Food.MaxLimitPoints,
                    GameSETTINGS.MapResources.FoodRegenerationPerHour);

                mapTile.Wood.ActualPoints = RegenerateActualPoints(mapTile.Wood.ActualPoints,
                    mapTile.Wood.MaxLimitPoints,
                    GameSETTINGS.MapResources.WoodRegenerationPerHour);
            }
        }
        int RegenerateActualPoints(int actualPoints, int maxPoints, int regenerationPoints)
        {
            if (actualPoints < maxPoints)
            {
                actualPoints = actualPoints + regenerationPoints;

                if (actualPoints > maxPoints)
                    actualPoints = maxPoints;
            }

            return actualPoints;
        }
    }
}
