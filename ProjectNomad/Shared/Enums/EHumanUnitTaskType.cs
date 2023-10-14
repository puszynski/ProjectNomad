using System.Runtime.CompilerServices;

namespace ProjectNomad.Shared.Enums
{
    public enum EHumanUnitTaskType
    {
        GatheringFood = 100,

        GatheringWood = 200,

        LightAFire = 220,
        KeepFire = 201,

        TribeRelocation = 300,

        //autotaskss
        ConsumeFood = 1001,
    }

    public static class HumanUnitTaskTypeExtensions
    {
        public static string GetDescription(this EHumanUnitTaskType type)
        {
            switch (type)
            {
                case EHumanUnitTaskType.GatheringFood:
                    return "gromadź jedzenie";
                case EHumanUnitTaskType.GatheringWood:
                    return "gromadź drewno";
                case EHumanUnitTaskType.LightAFire:
                    return "rozpal ogień";
                case EHumanUnitTaskType.KeepFire:
                    return "utrzymuj ogień";
                case EHumanUnitTaskType.TribeRelocation:
                    return "zmień lokalizację";
                case EHumanUnitTaskType.ConsumeFood:
                    return "konsumuj jedzenie";
                default:
                    throw new NotImplementedException();
            }
        }

        public static List<EHumanUnitTaskType> GetAutoTasks()
        {
            return new List<EHumanUnitTaskType>() { EHumanUnitTaskType.ConsumeFood };
        }
    }
}
