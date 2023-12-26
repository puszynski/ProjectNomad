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
        internal string? GetDescription()
        {
            switch (Type)
            {
                case ENotificationType.Starvation: return $"{HumanName} ŚMIERĆ GŁÓD";
                case ENotificationType.DeathFromStarvation: return $"{HumanName} głód";
                case ENotificationType.DeathFromAgeOrIllness: return $"{HumanName} ŚMIERĆ TAJEMNICA";
                case ENotificationType.Newborn: return $"{HumanName} NOWY MAŁY CZŁOWIEK";

                case ENotificationType.FoodGatheringEnded: return $"{HumanName} przynieść jedzenie +{CustomValue}";
                case ENotificationType.NoFoodFounded: return $"{HumanName} nie znaleźć jedzenie!";

                case ENotificationType.WoodGatheringEnded: return $"{HumanName} przynieść drewno +{CustomValue}";
                case ENotificationType.NoWoodFounded: return $"{HumanName} nie znaleźć drewno!";

                case ENotificationType.NoWoodForCampfire: return $"{HumanName} drewno brak ogień nie";
                case ENotificationType.AttemptToStartFireFailed: return $"{HumanName} ogień nie udać";
                case ENotificationType.FireStarted: return $"{HumanName} ogień palić";
                case ENotificationType.DeathFromFreezing: return $"{HumanName} ŚMIERĆ MRÓZ";
                case ENotificationType.DeathFromOverheat: return $"{HumanName} ŚMIERĆ UPAŁ";

                case ENotificationType.SleepInterruptedDueToCriticalConditions: return $"{HumanName} nie móc spać";
                case ENotificationType.AttemptToStartFireStartedDueToFreezingAndLackOfCampfire: return $"{HumanName} ogień próbować bo zamarzać";

                case ENotificationType.FoodGatheringStarted:
                case ENotificationType.WoodGatheringStarted:
                case ENotificationType.AttemptToStartFireStarted:
                case ENotificationType.KeepFireProceeded:
                case ENotificationType.FoodConsumptionStarted:
                case ENotificationType.FoodConsumptionEnded:
                case ENotificationType.HeatUpHumanByFireEnded:
                case ENotificationType.SleepStart:
                case ENotificationType.SleepEnd:
                    return default;

                default: return Type.ToString();
            }
        }
    }
}
