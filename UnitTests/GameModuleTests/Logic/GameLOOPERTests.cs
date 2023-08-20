using GameModule.Configurations;
using GameModule.Entities;
using GameModule.Logic;
using GameModule.Logic.GameLooperLogic;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ProjectNomad.Shared;
using Xunit;

namespace UnitTests.GameModuleTests.Logic
{
    public class GameLOOPERTests
    {
        Guid _accountId;
        GameModuleDbContext _dbContext;
        IHumanUnitTaskConsumer _humanUnitTaskConsumer;
        IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        IDateTimeProvider _dateTimeProvider;

        DateTime _startLoopFrom = new DateTime(2020, 01, 01, 00, 00, 00);
        DateTime _endLoopAt = new DateTime(2020, 02, 01, 23, 59, 59);

        //https://stackoverflow.com/questions/21069986/nsubstitute-dbset-iqueryablet/21075569#21075569
        public static DbSet<T> FakeDbSet<T>(List<T> dataList) where T : class
        {
            var data = dataList.AsQueryable();
            var fakeDbSet = Substitute.For<DbSet<T>, IQueryable<T>>();
            ((IQueryable<T>) fakeDbSet).Provider.Returns(data.Provider);
            ((IQueryable<T>) fakeDbSet).Expression.Returns(data.Expression);
            ((IQueryable<T>) fakeDbSet).ElementType.Returns(data.ElementType);
            ((IQueryable<T>) fakeDbSet).GetEnumerator().Returns(data.GetEnumerator());

            //fakeDbSet.Returns(fakeDbSet);
            fakeDbSet.AsNoTracking().Returns(fakeDbSet);

            return fakeDbSet;
        }

        public GameLOOPERTests()
        {
            _accountId = Guid.NewGuid();

            _startLoopFrom = new DateTime(2020, 01, 01, 00, 00, 00);
            _endLoopAt = new DateTime(2020, 02, 01, 23, 59, 59);

            var tribes = new List<Tribe>() { new Tribe { Id = 1, AccountId = _accountId, Updated = _startLoopFrom } }.AsQueryable();
            //TODO IMPLEMENT REPO TO MOCK IT... FUCK DBCONTEXTS..

            //todo
            //var humanUnits = new List<HumanUnit>() { new HumanUnit { Id = 1, FoodLevelPercentage = 100 } }.AsQueryable();
            //_dbContext.HumanUnits.Returns(humanUnits);
            //var humanUnitTasks = new List<HumanUnitTask>().AsQueryable();
            //_dbContext.HumanUnitTasks.Returns(humanUnitTasks);

            _humanUnitTaskConsumer = Substitute.For<IHumanUnitTaskConsumer>();

            _humanUnitAutoTaskScheduler = Substitute.For<IHumanUnitAutoTaskScheduler>();

            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider.UtcNow().Returns(_endLoopAt);
        }

        [Fact]
        public async Task Should_loop_thru_seconds_minutes_hours_days()//todo for sec + min     and for hours and days...
        {
            //Assign
            var gameLooper = new GameLOOPER(_dbContext, _dateTimeProvider, _humanUnitTaskConsumer, _humanUnitAutoTaskScheduler);

            //Act
            await gameLooper.LoopTribe(_accountId);

            //Assert
        }
    }
}
