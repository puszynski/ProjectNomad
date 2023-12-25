using GameModule.Entities;

namespace GameModule.Logic.Services;

internal class HumanThermalService
{
    /// <summary>
    /// NOTE: when human is freezing, he can auto-assign heatNearFire auto task
    /// </summary>
    internal void UpdateHumanThermalLevel(Tribe tribe, int temperature)
    {
        if (tribe.Humans == null)
            return;

        if (temperature < -10)
            tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage -= 3);
        else if (temperature < 0)
            tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage -= 2);
        else if (temperature < 10)
            tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage--);

        else if (temperature > 35)
            tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage++);

        else //optimal(10-35)
        {
            tribe.Humans.Where(x => x.ThermalLevelPercentage < 50).ToList().ForEach(x => x.ThermalLevelPercentage++);
            tribe.Humans.Where(x => x.ThermalLevelPercentage > 50).ToList().ForEach(x => x.ThermalLevelPercentage--);
        }
    }

}
