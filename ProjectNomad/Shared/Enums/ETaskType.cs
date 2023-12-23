namespace ProjectNomad.Shared.Enums
{
    public enum ETaskType //order makes priority of assigning
    {
        //jobs
        CampfireUp = 1,
        GatheringFood = 100,
        GatheringWood = 200,

        //special
        TribeRelocation = 300,

        // auto-tasks
        Sleep = 1001,
        ConsumeFood = 1002,
        HeatUpHumanByFireEnded = 1003,
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
                case ETaskType.Sleep:
                    return "sen";
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
            return new List<ETaskType>() 
            { 
                ETaskType.CampfireUp 
            };
        }

        public static List<ETaskType> GetJobs()
        {
            return new List<ETaskType>()
            {   
                ETaskType.GatheringWood,
                ETaskType.GatheringFood,
                ETaskType.CampfireUp
            };
        }

        public static List<ETaskType> GetAutoTasks()
        {
            return new List<ETaskType>() 
            { 
                ETaskType.ConsumeFood, 
                ETaskType.HeatUpHumanByFireEnded,
                ETaskType.Sleep
            };
        }
    }
}
