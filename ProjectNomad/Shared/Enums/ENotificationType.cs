namespace ProjectNomad.Shared.Enums
{
    public enum ENotificationType
    {
        GameOver,

        Starvation,
        DeathFromStarvation,
        DeathFromAgeOrIllness, 
        DeathFromFreezing,
        DeathFromOverheat,

        FoodGatheringStarted,
        FoodGatheringEnded,
        NoFoodFounded,
        FoodGatheringAutoStartedDueToStarvationAndLackOfFood,

        WoodGatheringStarted, 
        WoodGatheringEnded,
        NoWoodFounded,
        NoWoodInCampArea,

        NoWoodForCampfire,
        AttemptToStartFireStarted,
        AttemptToStartFireFailed,
        FireStarted,
        KeepFireProceeded,
        AttemptToStartFireStartedDueToFreezingAndLackOfCampfire,

        FoodConsumptionStarted,
        FoodConsumptionEnded,

        HeatUpHumanByFireEnded,

        SleepStart,
        SleepEnd,
        SleepInterruptedDueToCriticalConditions,

        Newborn,
    }
}
