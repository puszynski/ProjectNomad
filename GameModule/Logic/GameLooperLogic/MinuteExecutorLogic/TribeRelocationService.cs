using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Logic;

namespace GameModule.Logic.GameLooperLogic.MinuteExecutorLogic
{
    internal interface ITribeRelocationService
    {
        internal ETribeRelocationStatus GetTribeRelocationStatus(Tribe tribe);
        internal bool IsValidToStartRelocationProcess(Tribe tribe);
        internal void StartRelocationProcess(HumanTaskOrder taskOrder, Tribe tribe);
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

            if (tribe.HumanTaskOrders.Any(x => x.Type == ETaskType.TribeRelocation))
                return ETribeRelocationStatus.Scheduled;

            return ETribeRelocationStatus.None;
        }

        bool ITribeRelocationService.IsValidToStartRelocationProcess(Tribe tribe)
        {
            var relocationTaskOrder = tribe.HumanTaskOrders
                .Where(x => x.Type == ETaskType.TribeRelocation)
                .SingleOrDefault();

            if (relocationTaskOrder == null)
                return false;

            return true;
            //todo later - client must display info about requirments
            //ALBO - NIE WYMAGAĆ TEGO - ALE - WTEDY FOOD WCHODZI W MINUS I TRZEBA GO NAJPIERW ODBÓDOWAĆ (JUŻ TAK SIĘ ZROBIŁO)
            //return HasTribeEnoughResources();

            //bool HasTribeEnoughResources()
            //{
            //    return ResourcesNeededToRelocate(tribe) < tribe.Resources.FreshFood;
            //    int ResourcesNeededToRelocate(Tribe tribe)
            //    {
            //        var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
            //                    tribe.Localization.Y,
            //                    relocationTaskOrder.Localization.X,
            //                    relocationTaskOrder.Localization.Y);
            //        return distance * tribe.HumanUnits.Count * GameSETTINGS.TribeRelocation.FoodPointsNeededToTravelOneTileForOneTribeMember;
            //    }
            //}
        }

        void ITribeRelocationService.StartRelocationProcess(HumanTaskOrder taskOrder, Tribe tribe)
        {
            var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                        tribe.Localization.Y,
                        taskOrder.Localization.X,
                        taskOrder.Localization.Y);

            var taskDuration = TaskDurationCalculator.TribeRelocation(distance);

            var relocation = new TribeRelocation
            {
                From = _dateTimeProvider.UtcNow(),
                To = _dateTimeProvider.UtcNow().Add(taskDuration),
                Start = new Localization
                {
                    X = tribe.Localization.X,
                    Y = tribe.Localization.Y
                },
                Destiny = new Localization
                {
                    X = taskOrder.Localization.X,
                    Y = taskOrder.Localization.Y
                }
            };

            tribe.TribeRelocation = relocation;
            tribe.HumanTaskOrders?.Remove(taskOrder);
        }

        void ITribeRelocationService.EndRelocationProcess(Tribe tribe)
        {
            if (tribe.Humans == null || !tribe.Humans.Any())
                return;

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

                return distance * tribe.Humans.Count * GameSETTINGS.TribeRelocation.FoodPointsNeededToTravelOneTileForOneTribeMember;
            }
        }
    }
}
