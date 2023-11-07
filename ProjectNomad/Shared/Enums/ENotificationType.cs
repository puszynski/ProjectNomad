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

        WoodGatheringStarted, 
        WoodGatheringEnded,
        NoWoodToLightFire,
        AttemptToStartFireStarted,
        AttemptToStartFireFailed,
        FireStarted,
        KeepFireProceeded,

        FoodConsumptionStarted,
        FoodConsumptionEnded,

        HeatUpHumanByFireEnded,

        Newborn,
    }
}
