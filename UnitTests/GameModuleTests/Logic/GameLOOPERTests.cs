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
        IHumanUnitRepository _humanUnitRepository;
        IHumanUnitTaskRepository _humanUnitTaskRepository;
        IMapTileRepository _mapTileRepository;

        IDateTimeProvider _dateTimeProvider;

        ISecundExecutor _secundExecutor;
        IMinuteExecutor _minuteExecutor;
        IHourExecutor _hourExecutor;
        IDayExecutor _dayExecutor;

        DateTime _startLoopFrom;
        DateTime _endLoopAt;

        public void MockData(DateTime startLoop, DateTime endLoop)
        {
            _accountId = Guid.NewGuid();

            _startLoopFrom = startLoop;
            _endLoopAt = endLoop;

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = _startLoopFrom };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(Task.FromResult(tribe));

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = tribe.Id, FoodLevelPercentage = 100 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(tribe.Id).Returns(Task.FromResult(humanUnits));

            var humanUnitTasks = new List<HumanUnitTask>();
            _humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            _humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(humanUnitTasks);

            var mapTiles = new List<MapTile>();
            _mapTileRepository = Substitute.For<IMapTileRepository>();
            _mapTileRepository.GetByIds(new List<int?> { }).Returns(Task.FromResult(mapTiles));

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
        public async Task secundExecutor_should_loop_per_each_secund(int seconds)
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
                _humanUnitRepository,
                _humanUnitTaskRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _secundExecutor
                .Received(seconds)
                .Execute(Arg.Any<List<HumanUnit>>(), 
                Arg.Any<Tribe>(), 
                Arg.Any<List<HumanUnitTask>>(), 
                Arg.Any<List<MapTile>>(), 
                Arg.Any<DateTime>());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async Task minuteExecutor_Should_loop_once_per_minute(int minute)
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
                _humanUnitRepository,
                _humanUnitTaskRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            _minuteExecutor
                .Received(minute)
                .Execute(Arg.Any<List<HumanUnit>>(), Arg.Any<Tribe>());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async Task hourExecutor_should_loop_once_per_hour(int hour)
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
                _humanUnitRepository,
                _humanUnitTaskRepository);

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
        public async Task dayExecutor_should_loop_once_per_day(int day)
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
                _humanUnitRepository,
                _humanUnitTaskRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            _dayExecutor
                .Received(day)
                .Execute();
        }

        [Fact]
        public async Task should_create_foodAutoTask_and_consumeIt()
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood);
            MockData(start, end);

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = start, Resources = new Resources { FreshFood = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit } };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(Task.FromResult(tribe));

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = 1, FoodLevelPercentage = 80 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(1).Returns(Task.FromResult(humanUnits));

            var humanUnitTaskConsumer = new HumanUnitTaskConsumer(_humanUnitTaskRepository);
            var hHumanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(_humanUnitTaskRepository);
            _secundExecutor = new SecundExecutor(humanUnitTaskConsumer, hHumanUnitAutoTaskScheduler);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor, //NOTE - minute is only mock, so humanUnit is not getting hungry in game loop
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _humanUnitRepository,
                _humanUnitTaskRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _humanUnitTaskRepository.Received(1).AddAsync(Arg.Any<HumanUnitTask>());
            _humanUnitTaskRepository.Received(1).Remove(Arg.Any<HumanUnitTask>());
            Assert.Equal(100, humanUnits.Single().FoodLevelPercentage);
        }

        [Fact]
        public async Task should_create_foodAutoTask_BUT_not_consume_it_one_more_second_is_needed()
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood).AddSeconds(-1); 
            MockData(start, end);

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = start, Resources = new Resources { FreshFood = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit } };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(Task.FromResult(tribe));

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = 1, FoodLevelPercentage = 80 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(1).Returns(Task.FromResult(humanUnits));

            var humanUnitTaskConsumer = new HumanUnitTaskConsumer(_humanUnitTaskRepository);
            var hHumanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(_humanUnitTaskRepository);
            _secundExecutor = new SecundExecutor(humanUnitTaskConsumer, hHumanUnitAutoTaskScheduler);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor, //NOTE - minute is only mock, so humanUnit is not getting hungry in game loop
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _humanUnitRepository,
                _humanUnitTaskRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _humanUnitTaskRepository.Received(1).AddAsync(Arg.Any<HumanUnitTask>());
            _humanUnitTaskRepository.DidNotReceive().Remove(Arg.Any<HumanUnitTask>());
            Assert.Equal(80, humanUnits.Single().FoodLevelPercentage);
        }

        [Fact]
        public async Task should_not_create_foodAutoTask_and_not_consumeIt_humanUnit_have_more_then_81_food_level()
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood).AddSeconds(1); // NOTE - in first loop(1sec) there is no task - it`s created, so second loop(2sec) is starting counting time for task
            MockData(start, end);

            var tribe = new Tribe { Id = 1, AccountId = _accountId, Updated = start, Resources = new Resources { FreshFood = GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit } };
            _tribeRepository = Substitute.For<ITribeRepository>();
            _tribeRepository.GetByAccountId(_accountId).Returns(Task.FromResult(tribe));

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, TribeId = 1, FoodLevelPercentage = 81 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(1).Returns(Task.FromResult(humanUnits));

            var humanUnitTaskConsumer = new HumanUnitTaskConsumer(_humanUnitTaskRepository);
            var hHumanUnitAutoTaskScheduler = new HumanUnitAutoTaskScheduler(_humanUnitTaskRepository);
            _secundExecutor = new SecundExecutor(humanUnitTaskConsumer, hHumanUnitAutoTaskScheduler);

            var gameLooper = new GameLOOPER(
                _dayExecutor,
                _hourExecutor,
                _secundExecutor,
                _minuteExecutor, //NOTE - minute is only mock, so humanUnit is not getting hungry in game loop
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _humanUnitRepository,
                _humanUnitTaskRepository);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            await _humanUnitTaskRepository.DidNotReceive().AddAsync(Arg.Any<HumanUnitTask>());
            _humanUnitTaskRepository.DidNotReceive().Remove(Arg.Any<HumanUnitTask>());
            Assert.Equal(81, humanUnits.Single().FoodLevelPercentage);
        }
    }
}
