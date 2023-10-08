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
        public static List<EHumanUnitTaskType> GetAutoTasks()
        {
            return new List<EHumanUnitTaskType>() { EHumanUnitTaskType.ConsumeFood };
        }
    }
}
