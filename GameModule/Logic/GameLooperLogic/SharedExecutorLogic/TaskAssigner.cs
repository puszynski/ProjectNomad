using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
using GameModule.Logic.GameLooperLogic.TasksLogic;
using GameModule.Logic.GameLooperLogic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Logic;

namespace GameModule.Logic.GameLooperLogic.SharedExecutorLogic
{
    internal interface ITaskAssigner
    {
        Task Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications,
            DateTime currentTimeInLoop);
    }

    internal class TaskAssigner : ITaskAssigner
    {
        readonly ITribeRelocationService _relocationService;
        readonly GatheringFood _gatheringFood;
        readonly GatheringWood _gatheringWood;
        public TaskAssigner(
            ITribeRelocationService relocationService,
            GatheringFood gatheringFood,
            GatheringWood gatheringWood)
        {
            _relocationService = relocationService;
            _gatheringFood = gatheringFood;
            _gatheringWood = gatheringWood;
        }

        async Task ITaskAssigner.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications,
            DateTime currentTimeInLoop)
        {
            if (tribe.Humans == null || !tribe.Humans.Any())
                return;


            AutoTaskAssign();
            SkipRegularTasksWhenInCriticalCondition();//todo
            CriticalTasksAutoAssign();
            JobTaskAssign();
            //OrderTaskAssign();

            void AutoTaskAssign()
            {
                foreach (var autoTasks in TaskTypeExtensions.GetAutoTasks())
                {
                    var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);
                    if (!shouldAssign)
                        continue;

                    IAutoTaskStart? assigner = null;

                    if (DayNightService.IsNight(currentTimeInLoop))
                    {
                        switch (autoTasks)
                        {
                            case ETaskType.Sleep:
                                assigner = new Sleep();
                                break;
                            case ETaskType.ConsumeFood:
                                assigner = new ConsumeFood();
                                break;
                            case ETaskType.HeatUpHumanByFireEnded:
                                assigner = new HeatUpByFire();
                                break;
                        }
                    }
                    else
                    {
                        switch (autoTasks)
                        {
                            case ETaskType.ConsumeFood:
                                assigner = new ConsumeFood();
                                break;
                            case ETaskType.HeatUpHumanByFireEnded:
                                assigner = new HeatUpByFire();
                                break;
                        }
                    }

                    var notification = assigner?.Start(tribe, currentTimeInLoop, mapTiles);
                    if (notification != null)
                        notifications.Add(notification);
                }
            }

            void SkipRegularTasksWhenInCriticalCondition()
            {
                if (tribe.Humans == null || !tribe.Humans.Any())
                    return;

                var humansInCriticalConditionsWithTasksAssigned = tribe.Humans
                    .Where(x => x.HumanTask != null)
                    .Where(x => x.HumanTask.Type == ETaskType.Sleep)
                    .AsQueryable()
                    .Where(BasicDataSelector.HumansInCriticalCondition());

                foreach (var human in humansInCriticalConditionsWithTasksAssigned)
                {
                    notifications.Add(new NotificationDto(human.Id,
                        human.Name,
                        currentTimeInLoop,
                        ENotificationType.SleepInterruptedDueToCriticalConditions,
                        null));

                    var humanTaskToRemove = human.HumanTask;

                    tribe.HumanTasks.Remove(humanTaskToRemove);
                    human.HumanTask = null;
                }
            }

            void CriticalTasksAutoAssign()
            {
                if (tribe.Resources.FreshFood == 0)
                {
                    var starvingHumans = BasicDataSelector
                        .SelectHumansWithNoTasksAssigned(tribe, notInCriticalCondition: true)
                        .Where(x => x.FoodLevelPercentage <= GameSETTINGS.HumanConditions.CriticalFoodLevel);

                    foreach (var starvingHuman in starvingHumans)
                    {
                        notifications.Add(new NotificationDto(starvingHuman.Id,
                            starvingHuman.Name,
                            currentTimeInLoop,
                            ENotificationType.FoodGatheringAutoStartedDueToStarvationAndLackOfFood,
                            null));

                        ITaskFromJobStart assigner = _gatheringFood;
                        assigner.Start(starvingHuman, tribe, currentTimeInLoop, mapTiles);
                    }
                }

                if (!tribe.TribeStructures.Any(x => x.Type == ETribeStructureType.Firecamp && x.PowerAndDurability > 0))
                {
                    var freezingHumans = BasicDataSelector
                        .SelectHumansWithNoTasksAssigned(tribe, notInCriticalCondition: true)
                        .Where(x => x.ThermalLevelPercentage <= GameSETTINGS.HumanConditions.CriticalLowThermalLevel);

                    foreach (var freezingHuman in freezingHumans)
                    {
                        notifications.Add(new NotificationDto(freezingHuman.Id,
                            freezingHuman.Name,
                            currentTimeInLoop,
                            ENotificationType.AttemptToStartFireStartedDueToFreezingAndLackOfCampfire,
                            null));

                        ITaskFromJobStart assigner = new Campfire();
                        assigner.Start(freezingHuman, tribe, currentTimeInLoop, mapTiles);
                    }
                }
            }

            void JobTaskAssign()
            {
                foreach (var human in BasicDataSelector.SelectHumansWithNoTasksAssigned(tribe, notInCriticalCondition: true))
                {
                    //coefficient simulate probability once per minute
                    if (RandomCalculator.GetBoolWithGivenProbability(0.02))
                        continue;

                    var jobs = human.Jobs //joby nie są zaciagnięte.. 
                        .OrderBy(x => x.PriorityPercentage)
                        .ToList();

                    // coefficient of tasks to randomly get one from it
                    var randomInt = RandomCalculator.GetRandomInt();
                    var actualPercentage = 0;
                    foreach (var job in jobs)
                    {
                        actualPercentage += job.PriorityPercentage;

                        if (randomInt <= actualPercentage)
                            SelectAndRunTaskAssigner(human, job);
                    }
                }

                void SelectAndRunTaskAssigner(Human human, Job job)
                {
                    ITaskFromJobStart? assigner = null;

                    if (DayNightService.IsNight(currentTimeInLoop))
                    {
                        switch (job.Type)
                        {
                            case ETaskType.CampfireUp:
                                assigner = new Campfire();
                                break;
                        }
                    }
                    else
                    {
                        switch (job.Type)
                        {
                            case ETaskType.GatheringFood:
                                assigner = _gatheringFood;
                                break;
                            case ETaskType.GatheringWood:
                                assigner = _gatheringWood;
                                break;
                            case ETaskType.CampfireUp:
                                assigner = new Campfire();
                                break;
                            default:
                                break;
                        }
                    }

                    var notification = assigner?.Start(human, tribe, currentTimeInLoop, mapTiles);
                    if (notification != null)
                        notifications.Add(notification);
                }

                //    [Obsolete("use JobTaskAssign")]
                //    void OrderTaskAssign()
                //    {
                //        var taskOrdersToAssign = tribe.HumanTaskOrders
                //            .Where(x => !x.HumanTaskId.HasValue)
                //            .OrderBy(x => x.Type)
                //            .ToList();

                //        foreach (var taskOrder in taskOrdersToAssign)
                //        {
                //            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);
                //            if (!shouldAssign)
                //                continue;

                //            ITask? assigner = null;

                //            if (DayNightService.IsNight(currentTimeInLoop))
                //            {
                //                switch (taskOrder.Type)
                //                {
                //                    case ETaskType.CampfireUp:
                //                        assigner = new Campfire();
                //                        break;
                //                    case ETaskType.Sleep:
                //                        assigner = new Sleep();
                //                        break;
                //                }
                //            }
                //            else
                //            {
                //                switch (taskOrder.Type)
                //                {
                //                    case ETaskType.GatheringFood:
                //                        assigner = new GatheringFood();
                //                        break;
                //                    case ETaskType.GatheringWood:
                //                        assigner = new GatheringWood();
                //                        break;
                //                    case ETaskType.CampfireUp:
                //                        assigner = new Campfire();
                //                        break;
                //                    case ETaskType.TribeRelocation:
                //                        TribeRelocationTasksAssign(taskOrder, tribe);
                //                        break;
                //                }
                //            }

                //            var notification = assigner?.Start(taskOrder, tribe, currentTimeInLoop, mapTiles);
                //            if (notification != null)
                //                notifications.Add(notification);
                //        }
                //    }
                //}

                void TribeRelocationTasksAssign(HumanTaskOrder taskOrder, Tribe tribe)
                {
                    if (!_relocationService.IsValidToStartRelocationProcess(tribe))
                        return;

                    _relocationService.StartRelocationProcess(taskOrder, tribe);
                }
            }
        }
    }
}
