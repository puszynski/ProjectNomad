namespace ProjectNomad.Shared.Enums
{
    public enum ETaskType
    {
        GatheringFood = 100,

        GatheringWood = 200,

        CampfireUp = 220,

        TribeRelocation = 300,

        // auto-tasks
        ConsumeFood = 1001,
        HeatUpHumanByFireEnded = 1002,
    }

    public static class TaskTypeExtensions
    {
        public static string GetDescription(this ETaskType type)
        {
            switch (type)
            {
                case ETaskType.GatheringFood:
                    return "gromadź jedzenie";
                case ETaskType.GatheringWood:
                    return "gromadź drewno";
                case ETaskType.CampfireUp:
                    return "utrzymuj ogień";
                case ETaskType.TribeRelocation:
                    return "zmień lokalizację";
                case ETaskType.ConsumeFood:
                    return "konsumuj jedzenie";
                case ETaskType.HeatUpHumanByFireEnded:
                    return "ogrzewaj przy ogniu";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetSymbol(this ETaskType type)
        {
            if (GetConstantTasks().Contains(type)) //note - that task "Type" will not depend on enum, rather on player decission
                return "∞";
            else if (GetAutoTasks().Contains(type))
                return "⚡";
            else
                return "◴"; //means scheduled tasks
        }

        public static List<ETaskType> GetConstantTasks()
        {
            return new List<ETaskType>() { ETaskType.CampfireUp };
        }

        public static List<ETaskType> GetAutoTasks()
        {
            return new List<ETaskType>() { ETaskType.ConsumeFood, ETaskType.HeatUpHumanByFireEnded };
        }
    }
}
