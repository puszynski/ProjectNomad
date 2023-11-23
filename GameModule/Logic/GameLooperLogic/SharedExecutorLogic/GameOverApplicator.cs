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
                await _humanUnitTaskOrderRepository.SaveChangesAsync(); //czasami jak jest tu problem - to zadziała bez jak sie to zakomentuje, ale to skolei generuje ptoblem wcześniejszy z DbUpdateConcurrencyException w LOOP`erze -.-
             }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
                //SqlException: The UPDATE statement conflicted with the FOREIGN KEY constraint "FK_HumanTaskOrders_HumanTasks_HumanTaskId". The conflict occurred in database "ProjectNomadV3", table "GameModule.HumanTasks", column 'Id'.

                //note - ten błąd powstaje po tym jak wszyscy umierają i przed wykonaniem poniższych kodów chcemy zapisać w lini 32 - gdy usuwamy humanUnits.. nie mają na sobie podpiętych tasków..

            }

            await _humanUnitTaskOrderRepository.RemoveAll(tribe.Id);
            await _humanUnitTaskRepository.RemoveAll(tribe.Id);
            await _tribeStructureRepository.RemoveAll(tribe.Id);
            tribe.Resources.FreshFood = 0;
            tribe.Resources.Wood = 0;
            
        }
    }
}
