using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal class TribeRelocation : ITask
    {
        readonly ITribeRelocationService _relocationService;
        public TribeRelocation(ITribeRelocationService relocationService)
        {
            _relocationService = relocationService;
        }

        INotification ITask.Start(HumanUnitTaskOrder taskOrder, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            var taskOrderToAssign = tribe.HumanUnitTaskOrders
                    .Where(x => x.Type == EHumanUnitTaskType.TribeRelocation)
                    .Where(x => !x.IsInProgress)
                    .SingleOrDefault();

            if (taskOrderToAssign == null)
                return default;

            if (!_relocationService.IsValidToStartRelocationProcess(tribe))
                return default;

            _relocationService.StartRelocationProcess(tribe);
            return default;
        }

        INotification ITask.End(HumanUnitTask taskToEnd, Tribe tribeMaterializedData, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            throw new NotImplementedException();
        }

    }
}
