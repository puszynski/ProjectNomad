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
                case ENotificationType.Starvation: return $"{HumanName} głoduje";
                case ENotificationType.DeathFromStarvation: return $"{HumanName} odczuwa głód";
                case ENotificationType.DeathFromAgeOrIllness: return $"{HumanName} nie żyje. Nikt nie zna przyczyny";
                case ENotificationType.Newborn: return $"{HumanName} otwiera oczy po raz pierwszy";
                case ENotificationType.FoodGatheringStarted: return $"{HumanName} poszukuje jedzenia";
                case ENotificationType.FoodGatheringEnded: return $"{HumanName} zdobył {CustomValue}` jedzenia";//bezpółciowo
                case ENotificationType.WoodGatheringStarted: return $"{HumanName} poszukuje drewno";
                case ENotificationType.WoodGatheringEnded: return $"{HumanName} przyniósł {CustomValue}` drewna";//bezpółciowo
                case ENotificationType.NoWoodToLightFire: return $"{HumanName} nie może rozpalić ognia, zabrakło drewna";
                case ENotificationType.AttemptToStartFireStarted: return $"{HumanName} rozpala ogień";
                case ENotificationType.AttemptToStartFireFailed: return $"{HumanName} widzi tylko dym";
                case ENotificationType.FireStarted: return $"{HumanName} widzi buchające płomienie";
                case ENotificationType.KeepFireProceeded: return $"{HumanName} dorzuca do ognia, ogień bucha";
                case ENotificationType.FoodConsumptionStarted: return $"{HumanName} je";
                default: return string.Empty;
            }
        }
    }
}
