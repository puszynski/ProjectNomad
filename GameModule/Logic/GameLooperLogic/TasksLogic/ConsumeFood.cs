using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.TasksLogic
{
    internal class ConsumeFood : IAutoTaskStart, ITaskEnd
    {
        private const int HUMAN_FOOD_PERCENTAGE_TO_START_CONSUME = 80;

        INotification IAutoTaskStart.Start(
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var humansWithConditions = BasicDataSelector
                .SelectHumansWithCondition(tribe, notInCriticalCondition: false)
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
                Human = human,
                TribeId = tribe.Id,
                Type = ETaskType.ConsumeFood,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Food.MinutesToConsumeFoodToFill20PercentageOfFood),
                Localization = null
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

        INotification ITaskEnd.End(HumanTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            return new NotificationDto(taskToEnd.HumanId,
                tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                currentTimeInLoop,
                ENotificationType.FoodConsumptionEnded,
                null);
        }

    }
}
