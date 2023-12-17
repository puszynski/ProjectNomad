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

        WoodGatheringStarted, 
        WoodGatheringEnded,
        NoWoodFounded,
        NoWoodInCampArea,

        NoWoodForCampfire,
        AttemptToStartFireStarted,
        AttemptToStartFireFailed,
        FireStarted,
        KeepFireProceeded,

        FoodConsumptionStarted,
        FoodConsumptionEnded,

        HeatUpHumanByFireEnded,

        SleepStart,
        SleepEnd,

        Newborn,
    }
}
