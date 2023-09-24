using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.MinuteExecutorLogic
{
    internal interface ITribeRelocationService
    {
        internal ETribeRelocationStatus GetTribeRelocationStatus(Tribe tribe);
        internal bool IsValidToStartRelocationProcess(Tribe tribe);
        internal void StartRelocationProcess(Tribe tribe);
        internal void EndRelocationProcess(Tribe tribe);
    }

    internal class TribeRelocationService : ITribeRelocationService
    {
        readonly ITribeRelocationRepository _tribeRelocationRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        public TribeRelocationService(ITribeRelocationRepository tribeRelocationRepository, 
            IDateTimeProvider dateTimeProvider)
        {
            _tribeRelocationRepository = tribeRelocationRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        ETribeRelocationStatus ITribeRelocationService.GetTribeRelocationStatus(Tribe tribe)
        {
            if (tribe.TribeRelocation != null)
                return ETribeRelocationStatus.InProgress;

            if (tribe.HumanUnitTaskOrders.Any(x => x.Type == EHumanUnitTaskType.TribeRelocation))
                return ETribeRelocationStatus.Scheduled;

            return ETribeRelocationStatus.None;
        }

        bool ITribeRelocationService.IsValidToStartRelocationProcess(Tribe tribe)
        {
            var relocationTaskOrder = tribe.HumanUnitTaskOrders
                .Where(x => x.Type == EHumanUnitTaskType.TribeRelocation)
                .SingleOrDefault();

            if (relocationTaskOrder == null)
                return false;

            return HasTribeEnoughResources();

            bool HasTribeEnoughResources()
            {
                return ResourcesNeededToRelocate(tribe) < tribe.Resources.FreshFood;

                int ResourcesNeededToRelocate(Tribe tribe)
                {
                    var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                                tribe.Localization.Y,
                                relocationTaskOrder.Localization.X,
                                relocationTaskOrder.Localization.Y);

                    return distance * tribe.HumanUnits.Count * GameSETTINGS.TribeRelocation.FoodPointsNeededToTravelOneTileForOneTribeMember;
                }
            }
        }

        void ITribeRelocationService.StartRelocationProcess(Tribe tribe)
        {
            //todo test...
            var relocationTaskOrder = tribe.HumanUnitTaskOrders
                .Where(x => x.Type == EHumanUnitTaskType.TribeRelocation)
                .SingleOrDefault();

            if (relocationTaskOrder == null)
                return;

            var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                        tribe.Localization.Y,
                        relocationTaskOrder.Localization.X,
                        relocationTaskOrder.Localization.Y);

            var relocation = new TribeRelocation
            {
                From = _dateTimeProvider.UtcNow(),
                To = _dateTimeProvider.UtcNow().AddMinutes(distance * GameSETTINGS.Moving.MinutesToTravelOneTileWhileTribeIsRelocating),
                Start = new Localization
                {
                    X = tribe.Localization.X,
                    Y = tribe.Localization.Y
                },
                Destiny = new Localization
                {
                    X = relocationTaskOrder.Localization.X,
                    Y = relocationTaskOrder.Localization.Y
                }
            };

            tribe.TribeRelocation = relocation;
            tribe.HumanUnitTaskOrders.Remove(relocationTaskOrder);
        }

        void ITribeRelocationService.EndRelocationProcess(Tribe tribe)
        {
            if (tribe.TribeRelocation == null)
                return;

            if (tribe.TribeRelocation.To > _dateTimeProvider.UtcNow())
                return;
            
            tribe.Resources.FreshFood -= ResourcesNeededToRelocate(tribe);

            tribe.Localization = new Localization 
            { 
                X = tribe.TribeRelocation.Destiny.X, 
                Y = tribe.TribeRelocation.Destiny.Y
            };

            tribe.TribeRelocation = null;

            int ResourcesNeededToRelocate(Tribe tribe)
            {
                var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                            tribe.Localization.Y,
                            tribe.TribeRelocation.Destiny.X,
                            tribe.TribeRelocation.Destiny.Y);

                return distance * tribe.HumanUnits.Count * GameSETTINGS.TribeRelocation.FoodPointsNeededToTravelOneTileForOneTribeMember;
            }
        }
    }
}
