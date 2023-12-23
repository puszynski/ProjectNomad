using GameModule.Entities;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic.TasksLogic.Helpers
{
    internal static class BasicDataSelector
    {
        /// <param name="notInCriticalCondition"> Set false for eating and heating up in camp fire (when in critical condition, human must take care of them self)</param>
        internal static IEnumerable<Human> SelectHumansWithCondition(
            Tribe tribe,
            bool notInCriticalCondition)
        {
            if (tribe.Humans == null || !tribe.Humans.Any())
                return default;

            var selectedHumans = new List<Human>();

            var humanIDsWithTaskAssigned = tribe
                .Humans
                .Where(x => x.HumanTask != null)
                .Select(x => x.Id);
            
            selectedHumans = tribe.Humans
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id))
                .ToList();

            return notInCriticalCondition ? HumansNotInCriticalCondition() : selectedHumans;

            List<Human> HumansNotInCriticalCondition()
                => selectedHumans
                    .Where(x => (x.FoodLevelPercentage > GameSETTINGS.HumanConditions.CriticalFoodLevel) 
                        || (x.ThermalLevelPercentage > GameSETTINGS.HumanConditions.CriticalLowThermalLevel
                        && x.ThermalLevelPercentage < GameSETTINGS.HumanConditions.CriticalHighThermalLevel))
                    .ToList();
        }
    }
}
