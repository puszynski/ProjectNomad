using GameModule.Entities;
using GameModule.Repositories;

namespace GameModule.Logic.GameLooperLogic
{
    internal class TribeDeathApplicator
    {
        internal static void Execute(List<HumanUnitTask> tasksToConsume,
            IHumanUnitTaskRepository _humanUnitTaskRepository)
        {
            if (tasksToConsume.Any())
                _humanUnitTaskRepository.RemoveRange(tasksToConsume); 
        }
    }
}
