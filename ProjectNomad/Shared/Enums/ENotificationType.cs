namespace ProjectNomad.Shared.Enums
{
    public enum ENotificationType
    {
        Starvation,
        DeathFromStarvation,

        FoodGatheringStarted,
        FoodGatheringEnded,
    }

    public static class NotificationTypeValidator
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
