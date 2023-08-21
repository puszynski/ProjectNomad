using ProjectNomad.Shared;

namespace GameModule.Logic.TasksLogic
{
    internal class HumanUnitTaskEndCalculator
    {
        //todo async
        internal DateTime Execute(int startX, 
            int startY, 
            int endX, 
            int endY, 
            int foodPoints, //todo
            IDateTimeProvider dateTimeProvider)
        {
            var distance = MapTileDistanceCalculator.Execute(startX, startY, endX, endY); //todo

            return dateTimeProvider.UtcNow();
        }
    }
}
