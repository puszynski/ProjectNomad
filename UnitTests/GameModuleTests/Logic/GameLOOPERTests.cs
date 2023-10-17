using GameModule.Entities;
using GameModule.Logic;
using GameModule.Logic.GameLooperLogic;
using GameModule.Repositories;
using NSubstitute;
using ProjectNomad.Shared;
using Xunit;

namespace UnitTests.GameModuleTests.Logic
{
    public class GameLOOPERTests
    {
        Guid _accountId;

        ITribeRepository _tribeRepository;
        IMapTileRepository _mapTileRepository;
        IGameOverApplicator _gameOverApplicator;
        IHumanUnitRepository _humanUnitRepository;
        IHumanUnitTaskRepository _humanUnitTaskRepository;
        IHumanUnitTaskOrderRepository _humanUnitTaskOrderRepository;

        IDateTimeProvider _dateTimeProvider;

        ISecundExecutor _secundExecutor;
        IMinuteExecutor _minuteExecutor;
        IHourExecutor _hourExecutor;
        IDayExecutor _dayExecutor;

        DateTime _startLoopFrom;
        DateTime _endLoopAt;

        public void MockData(DateTime startLoop, DateTime endLoop)
        {
            _gameOverApplicator = Substitute.For<IGameOverApplicator>();

            _accountId = Guid.NewGuid();

            _startLoopFrom = startLoop;
            _endLoopAt = endLoop;

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = _startLoopFrom };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = 100 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(tribe.Id).Returns(System.Threading.Tasks.Task.FromResult(humanUnits));

            var humanUnitTasks = new List<GameModule.Entities.HumanTask>();
            _humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            _humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(humanUnitTasks);

            _humanUnitTaskOrderRepository = Substitute.For<IHumanUnitTaskOrderRepository>();

            var mapTiles = new List<MapTile>();
            _mapTileRepository = Substitute.For<IMapTileRepository>();
            _mapTileRepository.GetByIds(new List<int> { }).Returns(System.Threading.Tasks.Task.FromResult(mapTiles));

            _secundExecutor = Substitute.For<ISecundExecutor>();
            _minuteExecutor = Substitute.For<IMinuteExecutor>();
            _hourExecutor = Substitute.For<IHourExecutor>();
            _dayExecutor = Substitute.For<IDayExecutor>();

            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider.UtcNow().Returns(_endLoopAt);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async System.Threading.Tasks.Task secundExecutor_should_loop_per_each_secund(int seconds)
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddSeconds(seconds);
            MockData(start, end);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor,
                _tribeRepository,
                _dateTimeProvider, 
                _mapTileRepository,
                _gameOverApplicator,
                _humanUnitRepository,
                _humanUnitTaskRepository,
                _humanUnitTaskOrderRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _secundExecutor
                .Received(seconds)
                .Execute(Arg.Any<List<Human>>(),
                Arg.Any<Tribe>(),
                Arg.Any<List<GameModule.Entities.HumanTask>>(),
                Arg.Any<List<HumanTaskOrder>>(),
                Arg.Any<List<MapTile>>(),
                Arg.Any<DateTime>());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async System.Threading.Tasks.Task minuteExecutor_Should_loop_once_per_minute(int minute)
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddMinutes(minute);
            MockData(start, end);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor,
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _gameOverApplicator,
                _humanUnitRepository,
                _humanUnitTaskRepository,
                _humanUnitTaskOrderRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            _minuteExecutor
                .Received(minute)
                .Execute(Arg.Any<List<Human>>(), Arg.Any<Tribe>(), Arg.Any<List<GameModule.Entities.HumanTask>>());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async System.Threading.Tasks.Task hourExecutor_should_loop_once_per_hour(int hour)
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddHours(hour);
            MockData(start, end);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor,
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _gameOverApplicator,
                _humanUnitRepository,
                _humanUnitTaskRepository,
                _humanUnitTaskOrderRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            _hourExecutor
                .Received(hour)
                .Execute();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async System.Threading.Tasks.Task dayExecutor_should_loop_once_per_day(int day)
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddDays(day);
            MockData(start, end);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor,
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _gameOverApplicator,
                _humanUnitRepository,
                _humanUnitTaskRepository,
                _humanUnitTaskOrderRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            _dayExecutor
                .Received(day)
                .Execute();
        }

        [Fact]
        public async System.Threading.Tasks.Task should_create_foodAutoTask_and_consumeIt()
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood);
            MockData(start, end);

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = start, Resources = new Resources { FreshFood = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit } };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = 1, FoodLevelPercentage = 80 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(1).Returns(System.Threading.Tasks.Task.FromResult(humanUnits));

            var humanUnitTaskConsumer = new HumanUnitTaskConsumer(_humanUnitTaskRepository);
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(_humanUnitTaskRepository);
            var taskAssigner = Substitute.For<ITaskAssigner>();

            _secundExecutor = new SecundExecutor(taskAssigner, humanUnitTaskConsumer, humanUnitAutoTaskScheduler);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor, //NOTE - minute is only mock, so humanUnit is not getting hungry in game loop
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _gameOverApplicator,
                _humanUnitRepository,
                _humanUnitTaskRepository,
                _humanUnitTaskOrderRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _humanUnitTaskRepository.Received(1).AddAsync(Arg.Any<GameModule.Entities.HumanTask>());
            _humanUnitTaskRepository.Received(1).Remove(Arg.Any<GameModule.Entities.HumanTask>());
            Assert.Equal(100, humanUnits.Single().FoodLevelPercentage);
        }

        [Fact]
        public async System.Threading.Tasks.Task should_create_foodAutoTask_BUT_not_consume_it_one_more_second_is_needed()
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood).AddSeconds(-1); 
            MockData(start, end);

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = start, Resources = new Resources { FreshFood = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit } };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = 1, FoodLevelPercentage = 80 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(1).Returns(System.Threading.Tasks.Task.FromResult(humanUnits));

            var humanUnitTaskConsumer = new HumanUnitTaskConsumer(_humanUnitTaskRepository);
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(_humanUnitTaskRepository);
            var taskAssigner = Substitute.For<ITaskAssigner>();
            _secundExecutor = new SecundExecutor(taskAssigner, humanUnitTaskConsumer, humanUnitAutoTaskScheduler);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor, //NOTE - minute is only mock, so humanUnit is not getting hungry in game loop
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _gameOverApplicator,
                _humanUnitRepository,
                _humanUnitTaskRepository,
                _humanUnitTaskOrderRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _humanUnitTaskRepository.Received(1).AddAsync(Arg.Any<GameModule.Entities.HumanTask>());
            _humanUnitTaskRepository.DidNotReceive().Remove(Arg.Any<GameModule.Entities.HumanTask>());
            Assert.Equal(80, humanUnits.Single().FoodLevelPercentage);
        }

        [Fact]
        public async System.Threading.Tasks.Task should_not_create_foodAutoTask_and_not_consumeIt_humanUnit_have_more_then_81_food_level()
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood).AddSeconds(1); // NOTE - in first loop(1sec) there is no task - it`s created, so second loop(2sec) is starting counting time for task
            MockData(start, end);

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = start, Resources = new Resources { FreshFood = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit } };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(System.Threading.Tasks.Task.FromResult(tribe));

            var humanUnits = new List<Human>() { new Human { Id = 1, TribeId = 1, FoodLevelPercentage = 81 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(1).Returns(System.Threading.Tasks.Task.FromResult(humanUnits));

            var humanUnitTaskConsumer = new HumanUnitTaskConsumer(_humanUnitTaskRepository);
            var humanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(_humanUnitTaskRepository);
            var taskAssigner = Substitute.For<ITaskAssigner>();
            _secundExecutor = new SecundExecutor(taskAssigner, humanUnitTaskConsumer, humanUnitAutoTaskScheduler);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor, //NOTE - minute is only mock, so humanUnit is not getting hungry in game loop
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _gameOverApplicator,                
                _humanUnitRepository,
                _humanUnitTaskRepository,
                _humanUnitTaskOrderRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _humanUnitTaskRepository.DidNotReceive().AddAsync(Arg.Any<GameModule.Entities.HumanTask>());
            _humanUnitTaskRepository.DidNotReceive().Remove(Arg.Any<GameModule.Entities.HumanTask>());
            Assert.Equal(81, humanUnits.Single().FoodLevelPercentage);
        }
    }
}
