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
        public async System.Threading.Tasks.Task should_create_autoTask_when_tribe_has_food_and_humanUnit_needed()
        {
            //Arrange
            var TRIBE_FOOD = GameSETTINGS.Food.TribeFoodNeededToFill20PercentageOfHumanUnit;
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

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<GameModule.Entities.HumanTask>();
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<Task>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler();
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, new DateTime(2020, 01, 01));

            //Assert
            await humanUnitTaskRepository.Received(1).AddAsync(Arg.Any<GameModule.Entities.HumanTask>());
            await humanUnitTaskRepository.Received(1).SaveChangesAsync();
            Assert.Equal(0, tribe.Resources.FreshFood);
            Assert.Single(humanUnitTasks);
        }

        [Fact]
        public async System.Threading.Tasks.Task should_not_create_autoTask_when_tribe_has_food_and_humanUnit_needed_but_humanUnit_has_tasks_in_progress()
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

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<GameModule.Entities.HumanTask>() { new GameModule.Entities.HumanTask { HumanId = 1, TribeId = tribe.Id } };
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<Task>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(humanUnitTaskRepository);
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, Arg.Any<DateTime>());

            //Assert
            await humanUnitTaskRepository.Received(0).AddAsync(Arg.Any<GameModule.Entities.HumanTask>());
            await humanUnitTaskRepository.Received(0).SaveChangesAsync();
        }

        [Fact]
        public async System.Threading.Tasks.Task should_not_create_autoTask_when_tribe_has_no_food_and_humanUnit_needed()
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

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<GameModule.Entities.HumanTask>();
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<Task>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(humanUnitTaskRepository);
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, Arg.Any<DateTime>());

            //Assert
            await humanUnitTaskRepository.Received(0).AddAsync(Arg.Any<GameModule.Entities.HumanTask>());
            await humanUnitTaskRepository.Received(0).SaveChangesAsync();
            Assert.Empty(humanUnitTasks);
        }

        [Fact]
        public async System.Threading.Tasks.Task should_not_create_autoTask_when_tribe_has_food_but_humanUnit_do_not_needed()
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

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = HUMAN_FOOD_LEVEL } };

            var humanUnitTasks = new List<GameModule.Entities.HumanTask>();
            var humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(new List<Task>());

            var tribeRepository = Substitute.For<ITribeRepository>();
            tribeRepository.GetByAccountId(accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            //Act
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(humanUnitTaskRepository);
            await humanUnitAutoTaskScheduler.Execute(humanUnits, tribe, humanUnitTasks, Arg.Any<DateTime>());

            //Assert
            await humanUnitTaskRepository.Received(0).AddAsync(Arg.Any<GameModule.Entities.HumanTask>());
            await humanUnitTaskRepository.Received(0).SaveChangesAsync();
            Assert.Empty(humanUnitTasks);
        }
    }
}
