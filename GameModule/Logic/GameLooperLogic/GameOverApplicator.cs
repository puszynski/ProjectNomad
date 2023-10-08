using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IGameOverApplicator
    {
        static bool IsGameOver(ICollection<HumanUnit> humans) => !humans.Any();
        INotification Execute(Tribe tribe);
    }

    internal class GameOverApplicator : IGameOverApplicator
    {
        readonly IHumanUnitTaskOrderRepository _humanUnitTaskOrderRepository;
        readonly ITribeStructureRepository _tribeStructureRepository;
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        public GameOverApplicator(IHumanUnitTaskOrderRepository humanUnitTaskOrderRepository,
            ITribeStructureRepository tribeStructureRepository,
            IHumanUnitTaskRepository humanUnitTaskRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _humanUnitTaskOrderRepository = humanUnitTaskOrderRepository;
            _tribeStructureRepository = tribeStructureRepository;
            _humanUnitTaskRepository = humanUnitTaskRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        INotification IGameOverApplicator.Execute(Tribe tribe)
        {
            _humanUnitTaskOrderRepository.RemoveAll(tribe.Id);
            _humanUnitTaskRepository.RemoveAll(tribe.Id);
            _tribeStructureRepository.RemoveAll(tribe.Id);
            tribe.Resources.FreshFood = 0;
            tribe.Resources.Wood = 0;
            return new NotificationDto(0, "none", _dateTimeProvider.UtcNow(), ProjectNomad.Shared.Enums.ENotificationType.GameOver, null);
        }
    }
}
