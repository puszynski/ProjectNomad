using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IGameOverApplicator
    {
        static bool IsGameOver(List<HumanUnit> humans) => !humans.Any();
        INotification Execute(int tribeId);
    }

    internal class GameOverApplicator : IGameOverApplicator
    {
        readonly IHumanUnitTaskOrderRepository _humanUnitTaskOrderRepository;
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        public GameOverApplicator(IHumanUnitTaskOrderRepository humanUnitTaskOrderRepository, 
            IHumanUnitTaskRepository humanUnitTaskRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _humanUnitTaskOrderRepository = humanUnitTaskOrderRepository;
            _humanUnitTaskRepository = humanUnitTaskRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        INotification IGameOverApplicator.Execute(int tribeId)
        {
            _humanUnitTaskOrderRepository.RemoveAll(tribeId);
            _humanUnitTaskRepository.RemoveAll(tribeId);
            return new NotificationDto(0, "none", _dateTimeProvider.UtcNow(), ProjectNomad.Shared.Enums.ENotificationType.GameOver, null);
        }
    }
}
