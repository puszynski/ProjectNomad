using ProjectNomad.Shared;

namespace GameModule.Logic.TasksLogic
{
    internal class HumanUnitTaskEndCalculator
    {
        //todo async
        internal DateTime Execute(int startX, int startY, int endX, int endY, int foodPoints)
        {
            var distance = MapTileDistanceCalculator.Execute(startX, startY, endX, endY);

            return DateTime.UtcNow;
        }
    }
}
