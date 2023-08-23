using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using GameModule.Repositories;
using NSubstitute;
using ProjectNomad.Shared;
using Xunit;

namespace UnitTests.GameModuleTests.Logic.GameLooperLogic
{
    public class HumanUnitAutoTaskSchedulerTests
    {
        [Fact]
        public async Task should_create_autoTask_when_tribe_has_food_and_humanUnit_needed()
        {
            //Arrange
            var TRIBE_FOOD = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit;
            var HUMAN_FOOD_LEVEL = 80;

            var accountId = Guid.NewGuid();
            var tribe = new Tribe 
            { 
                Id = 1, 
                AccountId = accountId, 
                Resources = new Resources() 
                { 
                    FreshFood = TRIBE_FOOD
                }  
            };

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<HumanUnitTask>();
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<HumanUnitTask>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(humanUnitTaskRepository);
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, new DateTime(2020, 01, 01));

            //Assert
            await humanUnitTaskRepository.Received(1).AddAsync(Arg.Any<HumanUnitTask>());
            await humanUnitTaskRepository.Received(1).SaveChangesAsync();
            Assert.Equal(0, tribe.Resources.FreshFood);
            Assert.Single(humanUnitTasks);
        }

        [Fact]
        public async Task should_not_create_autoTask_when_tribe_has_food_and_humanUnit_needed_but_humanUnit_has_tasks_in_progress()
        {
            //Arrange
            var TRIBE_FOOD = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit;
            var HUMAN_FOOD_LEVEL = 80;

            var accountId = Guid.NewGuid();
            var tribe = new Tribe
            {
                Id = 1,
                AccountId = accountId,
                Resources = new Resources()
                {
                    FreshFood = TRIBE_FOOD
                }
            };

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<HumanUnitTask>() { new HumanUnitTask { HumanUnitId = 1, TribeId = tribe.Id } };
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<HumanUnitTask>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(humanUnitTaskRepository);
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, Arg.Any<DateTime>());

            //Assert
            await humanUnitTaskRepository.Received(0).AddAsync(Arg.Any<HumanUnitTask>());
            await humanUnitTaskRepository.Received(0).SaveChangesAsync();
        }

        [Fact]
        public async Task should_not_create_autoTask_when_tribe_has_no_food_and_humanUnit_needed()
        {
            //Arrange
            var TRIBE_FOOD = 0;
            var HUMAN_FOOD_LEVEL = 80;

            var accountId = Guid.NewGuid();
            var tribe = new Tribe
            {
                Id = 1,
                AccountId = accountId,
                Resources = new Resources()
                {
                    FreshFood = TRIBE_FOOD
                }
            };

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<HumanUnitTask>();
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<HumanUnitTask>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(humanUnitTaskRepository);
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, Arg.Any<DateTime>());

            //Assert
            await humanUnitTaskRepository.Received(0).AddAsync(Arg.Any<HumanUnitTask>());
            await humanUnitTaskRepository.Received(0).SaveChangesAsync();
            Assert.Empty(humanUnitTasks);
        }

        [Fact]
        public async Task should_not_create_autoTask_when_tribe_has_food_but_humanUnit_do_not_needed()
        {
            //Arrange
            var TRIBE_FOOD = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit;
            var HUMAN_FOOD_LEVEL = 81;

            var accountId = Guid.NewGuid();
            var tribe = new Tribe
            {
                Id = 1,
                AccountId = accountId,
                Resources = new Resources()
                {
                    FreshFood = TRIBE_FOOD
                }
            };

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<HumanUnitTask>();
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<HumanUnitTask>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(humanUnitTaskRepository);
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, Arg.Any<DateTime>());

            //Assert
            await humanUnitTaskRepository.Received(0).AddAsync(Arg.Any<HumanUnitTask>());
            await humanUnitTaskRepository.Received(0).SaveChangesAsync();
            Assert.Empty(humanUnitTasks);
        }
    }
}
