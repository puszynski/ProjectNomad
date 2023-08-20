namespace ProjectNomad.Client.Logic.NotificationsManagerLogic
{
    internal record Notification(int HumanUnitId, 
        DateTime Added, 
        ENotificationType Type);
}
