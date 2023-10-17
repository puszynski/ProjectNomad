using GameModule.DtoModels;
using GameModule.Entities;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal class ConsumeFood : ITask
    {
        private const int HUMAN_FOOD_PERCENTAGE_TO_START_CONSUME = 80;

        INotification ITask.Start(HumanTaskOrder taskOrder, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            taskOrder = null;//not used in auto task

            var humanUnitIdsWithTaskInProgress = tribe.HumanTasks
                .Select(x => x.HumanId)
                .ToList();
            var human = tribe.Humans
                .Where(x => !humanUnitIdsWithTaskInProgress.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage <= HUMAN_FOOD_PERCENTAGE_TO_START_CONSUME)
                .FirstOrDefault();
            if (human == null)
                return default;

            if (tribe.Resources.FreshFood < GameSETTINGS.Food.TribeFoodNeededToFill20PercentageOfHumanUnit)
                return default;

            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);
            if (!shouldAssign)
                return default;

            var entity = new Entities.HumanTask
            {
                From = currentTimeInLoop, //.AddSeconds(-1),//todo remove -1 due to TaskRequire mechanism and change UT
                HumanId = human.Id,
                TribeId = tribe.Id,
                Type = ETaskType.ConsumeFood,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Food.MinutesToConsumeFoodToFill20PercentageOfFood), //.AddSeconds(-1),//todo remove -1 TaskRequire mechanism and change UT
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

        INotification ITask.End(Entities.HumanTask taskToEnd, Tribe tribeMaterializedData, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            return new NotificationDto(taskToEnd.HumanId,
                tribeMaterializedData.Humans.Single(x => x.Id == taskToEnd.HumanId).Name, 
                currentTimeInLoop, 
                ENotificationType.FoodConsumptionEnded, 
                null);
        }

    }
}
