using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record Notification(int HumanUnitId,
        string HumanName,
        DateTime Added,
        ENotificationType Type,
        string? CustomValue) : INotification
    {
        internal string GetDescription()
        {
            switch (Type)
            {
                case ENotificationType.Starvation: return $"{HumanName} ŚMIERĆ GŁÓD";
                case ENotificationType.DeathFromStarvation: return $"{HumanName} głód";
                case ENotificationType.DeathFromAgeOrIllness: return $"{HumanName} ŚMIERĆ TAJEMNICA";
                case ENotificationType.Newborn: return $"{HumanName} NOWY CZŁOWIEK";
                case ENotificationType.FoodGatheringStarted: return $"{HumanName} szukać jedzenie";
                case ENotificationType.FoodGatheringEnded: return $"{HumanName} przynieść jedzenie +{CustomValue}";
                case ENotificationType.WoodGatheringStarted: return $"{HumanName} szukać drewno";
                case ENotificationType.WoodGatheringEnded: return $"{HumanName} przynieść drewno +{CustomValue}";
                case ENotificationType.NoWoodToLightFire: return $"{HumanName} drewno brak ogień nie";
                case ENotificationType.AttemptToStartFireStarted: return $"{HumanName} ogień próbować";
                case ENotificationType.AttemptToStartFireFailed: return $"{HumanName} ogień nie";
                case ENotificationType.FireStarted: return $"{HumanName} ogień duży ciepło";
                case ENotificationType.KeepFireProceeded: return $"{HumanName} ogień duży ciepło";
                case ENotificationType.FoodConsumptionStarted: return $"{HumanName} jeść";
                default: return string.Empty;
            }
        }
    }
}
