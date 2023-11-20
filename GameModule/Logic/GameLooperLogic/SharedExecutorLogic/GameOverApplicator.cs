using GameModule.Entities;
using GameModule.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Logic.GameLooperLogic.SharedExecutorLogic
{
    internal interface IGameOverApplicator
    {
        static bool IsGameOver(ICollection<Human> humans) 
            => !humans.Any();
        Task Execute(Tribe tribe);
    }

    internal class GameOverApplicator : IGameOverApplicator
    {
        readonly IHumanUnitTaskOrderRepository _humanUnitTaskOrderRepository;
        readonly ITribeStructureRepository _tribeStructureRepository;
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        public GameOverApplicator(IHumanUnitTaskOrderRepository humanUnitTaskOrderRepository,
            ITribeStructureRepository tribeStructureRepository,
            IHumanUnitTaskRepository humanUnitTaskRepository)
        {
            _humanUnitTaskOrderRepository = humanUnitTaskOrderRepository;
            _tribeStructureRepository = tribeStructureRepository;
            _humanUnitTaskRepository = humanUnitTaskRepository;
        }

        async Task IGameOverApplicator.Execute(Tribe tribe)
        {
            //test - save changes before deleting to prevent error
            try
            {
                await _humanUnitTaskOrderRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            await _humanUnitTaskOrderRepository.RemoveAll(tribe.Id);
            await _humanUnitTaskRepository.RemoveAll(tribe.Id);
            await _tribeStructureRepository.RemoveAll(tribe.Id);
            tribe.Resources.FreshFood = 0;
            tribe.Resources.Wood = 0;
        }
    }
}
