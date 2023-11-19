using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal class ConsumeFood : ITask
    {
        private const int HUMAN_FOOD_PERCENTAGE_TO_START_CONSUME = 80;

        INotification ITask.Start(
            HumanTaskOrder taskOrder,//not used in auto task 
            Tribe tribe, 
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles)
        {
            var humansWithConditions = BasicDataSelector
                .SelectHumansWithCondition(tribe)
                .Where(x => x.FoodLevelPercentage < HUMAN_FOOD_PERCENTAGE_TO_START_CONSUME);

            var human = RandomCalculator.GetRandomItemFromList(humansWithConditions); 

            if (human == null)
                return default;

            if (tribe.Resources.FreshFood < GameSETTINGS.Food.TribeFoodNeededToFill20PercentageOfHumanUnit)
                return default;

            var entity = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                TribeId = tribe.Id,
                Type = ETaskType.ConsumeFood,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Food.MinutesToConsumeFoodToFill20PercentageOfFood),
                Localization = null,
                IsCompleted = false,
            };

            tribe.HumanTasks.Add(entity);
            tribe.Resources.FreshFood -= GameSETTINGS.Food.TribeFoodNeededToFill20PercentageOfHumanUnit;
            human.FoodLevelPercentage += 20;

            return new NotificationDto(human.Id,
                human.Name,
                currentTimeInLoop,
                ENotificationType.FoodConsumptionStarted,
                entity.To.ToString());
        }

        INotification ITask.End(HumanTask taskToEnd, 
            Tribe tribe, 
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles)
        {
            tribe.HumanTasks.Remove(taskToEnd);

            return new NotificationDto(taskToEnd.HumanId,
                tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name, 
                currentTimeInLoop, 
                ENotificationType.FoodConsumptionEnded, 
                null);
        }

    }
}
