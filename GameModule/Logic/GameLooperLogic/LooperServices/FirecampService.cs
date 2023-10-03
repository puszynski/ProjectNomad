using GameModule.DtoModels;
using GameModule.Entities;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.LooperServices
{
    internal interface IFirecampService
    {
        //hmmm...? ech sec? ech 10s or 50 s fire must be refill? not to intense?
        internal static void FireDurationDecresseEachSecond(TribeStructure firecamp) 
            => firecamp.PowerAndDurability -= 1;


        internal void LightFire_TaskStart(
            Tribe tribe, 
            List<INotification> notifications);

        internal INotification LightFire_TaskEnd(
            HumanUnit humanUnit,
            Tribe tribe,
            DateTime currentTimeInLoop,
            ICollection<HumanUnitTaskOrder> humanUnitTaskOrders);


        internal void KeepFire_TaskStart(
            bool isBoneFire, 
            Tribe tribe);

        internal INotification KeepFire_TaskEnd(bool isBoneFire,
            HumanUnit humanUnit,
            Tribe tribe,
            DateTime currentTimeInLoop,
            ICollection<HumanUnitTaskOrder> humanUnitTaskOrders);

    }

    internal class FirecampService : IFirecampService
    {
        readonly IDateTimeProvider _dateTimeProvider;
        public FirecampService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        void IFirecampService.LightFire_TaskStart(Tribe tribe,
            List<INotification> notifications)
        {
            var taskOrderToAssign = tribe.HumanUnitTaskOrders
                    .Where(x => x.Type == EHumanUnitTaskType.TribeRelocation)
                    .Where(x => !x.IsInProgress)
                    .SingleOrDefault();

            if (taskOrderToAssign == null)
                return;

            var humanIDsWithTaskAssigned = tribe.HumanUnitTasks.Select(x => x.HumanUnitId);
            var humanWithConditionToStartNewTask = tribe
                .HumanUnits
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage > 10)
                .FirstOrDefault();

            if (humanWithConditionToStartNewTask == null)
                return;

            if (tribe.Resources.Wood < GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning)
            {
                var notification = new NotificationDto(humanWithConditionToStartNewTask.Id,
                    humanWithConditionToStartNewTask.Name,
                    _dateTimeProvider.UtcNow(),
                    ENotificationType.NoWoodToLightFire,
                    CustomValue: null);

                notifications.Add(notification);
                return;
            }

            if (!RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.Fire.ChanceToStartFire))
                return;

            var taskToAdd = new HumanUnitTask
            {
                From = _dateTimeProvider.UtcNow(),
                HumanUnitId = humanWithConditionToStartNewTask.Id,
                Localization = taskOrderToAssign.Localization,
                To = _dateTimeProvider.UtcNow().AddMinutes(GameSETTINGS.Fire.TimeToCompleteAttemptToStartFire),
                TribeId = tribe.Id,
                Type = EHumanUnitTaskType.LightAFire,
            };

            tribe.HumanUnitTasks.Add(taskToAdd);
            taskOrderToAssign.IsInProgress = true;
            tribe.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning;
        }

         INotification IFirecampService.LightFire_TaskEnd(
            HumanUnit humanUnit,
            Tribe tribe,
            DateTime currentTimeInLoop,
            ICollection<HumanUnitTaskOrder> humanUnitTaskOrders)
        {
            var fire = new TribeStructure()
            {
                TribeId = tribe.Id,
                Type = ETribeStructureType.Firecamp,
                PowerAndDurability = GameSETTINGS.Fire.PowerAndDurabilityForCampfire
            };
            tribe.TribeStructures.Add(fire);

            var finishedHumanTaskOrder = GetFinishedHumanTaskOrder(humanUnitTaskOrders, EHumanUnitTaskType.LightAFire);

            if (finishedHumanTaskOrder != null)
                humanUnitTaskOrders.Remove(finishedHumanTaskOrder);
            else
            {
                //todo add logs - finishedHumanTaskOrder should always exists, if its null its due to problem - happens 2 times.. 
            }

            return new NotificationDto(humanUnit.Id,
                humanUnit.Name,
                currentTimeInLoop,
                ENotificationType.FirecampStarted,
                CustomValue: null);
        }

        void IFirecampService.KeepFire_TaskStart(bool isBoneFire, Tribe tribe)
        {
            var fireType = isBoneFire ? EHumanUnitTaskType.KeepLowFire : EHumanUnitTaskType.KeepFireBig;

            var taskOrderToAssign = tribe.HumanUnitTaskOrders
                    .Where(x => x.Type == fireType)
                    .Where(x => !x.IsInProgress)
                    .SingleOrDefault(); //NOTE - ONLY ONE PER TRIBE (OR ONE KeepFireBig)

            if (taskOrderToAssign == null)
                return;

            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);

            if (!shouldAssign)
                return; ;

            if (!tribe.TribeStructures.Any(x => x.Type == ETribeStructureType.Firecamp))
                return;

            if (fireType == EHumanUnitTaskType.KeepLowFire)
                if (tribe.Resources.Wood < GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning)
                {
                    //todo notification - missing wood
                }
            if (fireType == EHumanUnitTaskType.KeepFireBig)
                if (tribe.Resources.Wood < GameSETTINGS.Fire.WoodUsedToKeepTheBonfireBurning)
                {
                    //todo notification - missing wood
                }

            var firecamp = tribe.TribeStructures.Single(x => x.Type == ETribeStructureType.Firecamp);

            if (fireType == EHumanUnitTaskType.KeepLowFire)
            {
                tribe.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning;
                firecamp.PowerAndDurability = GameSETTINGS.Fire.PowerAndDurabilityForCampfire;
            }
            if (fireType == EHumanUnitTaskType.KeepFireBig)
            {
                tribe.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheBonfireBurning;
                firecamp.PowerAndDurability = GameSETTINGS.Fire.PowerAndDurabilityForBonfire;
            }
            taskOrderToAssign.IsInProgress = true;
        }

        INotification IFirecampService.KeepFire_TaskEnd(bool isBoneFire,
            HumanUnit humanUnit,
            Tribe tribe,
            DateTime currentTimeInLoop,
            ICollection<HumanUnitTaskOrder> humanUnitTaskOrders)
        {
            var type = isBoneFire ? EHumanUnitTaskType.KeepLowFire : EHumanUnitTaskType.KeepFireBig;

            var finishedHumanTaskOrder = GetFinishedHumanTaskOrder(humanUnitTaskOrders, type);

            if (finishedHumanTaskOrder != null)
                humanUnitTaskOrders.Remove(finishedHumanTaskOrder);
            else
            {
                //todo add logs - finishedHumanTaskOrder should always exists, if its null its due to problem - happens 2 times.. 
            }

            //?? tribe.TribeStructures... ??

            return new NotificationDto(humanUnit.Id,
                humanUnit.Name,
                currentTimeInLoop,
                ENotificationType.FirecampKeepingLowProceeded,
                CustomValue: null);
        }

        HumanUnitTaskOrder? GetFinishedHumanTaskOrder(
            ICollection<HumanUnitTaskOrder> humanUnitTaskOrders, 
            EHumanUnitTaskType type)
            => humanUnitTaskOrders
            .Where(x => x.Type == type && x.IsInProgress)
            .OrderBy(x => x.Added)
            .FirstOrDefault();
    }
}
