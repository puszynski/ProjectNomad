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

        IHumanUnitTaskConsumer _humanUnitTaskConsumer;
        IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        IDateTimeProvider _dateTimeProvider;

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

            var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = tribe.Id, FoodLevelPercentage = 100 } };
            _humanUnitRepository = Substitute.For<IHumanUnitRepository>();
            _humanUnitRepository.GetHumanUnitsByTribeId(tribe.Id).Returns(Task.FromResult(humanUnits));

            var humanUnitTasks = new List<HumanUnitTask>();
            _humanUnitTaskRepository = Substitute.For<IHumanUnitTaskRepository>();
            _humanUnitTaskRepository.GetHumanUnitTasksByTribeId(1).Returns(humanUnitTasks);

            var mapTiles = new List<MapTile>();
            _mapTileRepository = Substitute.For<IMapTileRepository>();
            _mapTileRepository.GetByIds(new List<int?> { }).Returns(Task.FromResult(mapTiles));

            _humanUnitTaskConsumer = Substitute.For<IHumanUnitTaskConsumer>();
            _humanUnitAutoTaskScheduler = Substitute.For<IHumanUnitAutoTaskScheduler>();

            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider.UtcNow().Returns(_endLoopAt);
        }

        [Fact]
        public async Task Should_loop_properly_during_one_hour()
        {
            //Assign
            var start = new DateTime(2020, 01, 01, 00, 00, 00);
            var end = start.AddSeconds(1);
            MockData(start, end);

            var gameLooper = new GameLOOPER(
                _tribeRepository,
                _dateTimeProvider, 
                _mapTileRepository,
                _humanUnitRepository,
                _humanUnitTaskConsumer, 
                _humanUnitTaskRepository,
                _humanUnitAutoTaskScheduler);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
            //mockContext.Verify(x => x.SaveChanges(), Times.Once()); //todo check that hit once => DeathApplicator.StarvationDeath

            await _humanUnitAutoTaskScheduler
                .Received(2)
                .Execute(Arg.Any<List<HumanUnit>>(), Arg.Any<Tribe>(), Arg.Any<List<HumanUnitTask>>());

            _humanUnitTaskConsumer
                .Received(2)
                .Execute(Arg.Any<List<HumanUnit>>(), Arg.Any<Tribe>(), Arg.Any<List<HumanUnitTask>>(), Arg.Any<List<MapTile>>());

            //todo others methods / hour -> ZERO

        }

        [Fact]
        public async Task Should_loop_properly_thru_hours_and_days()//todo for sec + min     and for hours and days...
        {
            //Assign
            var gameLooper = new GameLOOPER(
                _tribeRepository,
                _dateTimeProvider,
                _mapTileRepository,
                _humanUnitRepository,
                _humanUnitTaskConsumer,
                _humanUnitTaskRepository,
                _humanUnitAutoTaskScheduler);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
        }
    }
}
