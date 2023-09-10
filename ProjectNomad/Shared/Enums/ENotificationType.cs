namespace ProjectNomad.Shared.Enums
{
    public enum ENotificationType
    {
        GameOver,

        Starvation,
        DeathFromStarvation,
        DeathFromAgeOrIllness, 

        FoodGatheringStarted,
        FoodGatheringEnded,

        FoodConsumptionStarted,
        FoodConsumptionEnded,

        Newborn,

    }

    public static class NotificationTypeValidator //TODO OBSOLETE DELETE
    {
        public static bool IsWASMNotification(this ENotificationType type) => WASMNotifications.Contains(type);
        public static bool IsServerNotification(this ENotificationType type) => !WASMNotifications.Contains(type);

        private static List<ENotificationType> WASMNotifications = new List<ENotificationType>
        {
            ENotificationType.FoodGatheringStarted,
            ENotificationType.FoodGatheringEnded
        };
    }
}
