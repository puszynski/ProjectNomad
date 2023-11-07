using System.Runtime.CompilerServices;

namespace ProjectNomad.Shared.Enums
{
    public enum ETaskType
    {
        GatheringFood = 100,

        GatheringWood = 200,

        LightAFire = 220,
        KeepFire = 201,

        TribeRelocation = 300,

        // auto-tasks
        ConsumeFood = 1001,
        HeatUpHumanByFireEnded = 1002,
    }

    public static class HumanUnitTaskTypeExtensions
    {
        public static string GetDescription(this ETaskType type)
        {
            switch (type)
            {
                case ETaskType.GatheringFood:
                    return "gromadź jedzenie";
                case ETaskType.GatheringWood:
                    return "gromadź drewno";
                case ETaskType.LightAFire:
                    return "rozpal ogień";
                case ETaskType.KeepFire:
                    return "utrzymuj ogień";
                case ETaskType.TribeRelocation:
                    return "zmień lokalizację";
                case ETaskType.ConsumeFood:
                    return "konsumuj jedzenie";
                default:
                    throw new NotImplementedException();
            }
        }

        public static List<ETaskType> GetAutoTasks()
        {
            return new List<ETaskType>() { ETaskType.ConsumeFood };
        }
    }
}
