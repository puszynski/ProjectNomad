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

        INotification ITask.Start(HumanTaskOrder taskOrder, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            var taskOrderToAssign = tribe.HumanTaskOrders
                    .Where(x => x.Type == ETaskType.TribeRelocation)
                    .SingleOrDefault();

            if (taskOrderToAssign == null)
                return default;

            if (!_relocationService.IsValidToStartRelocationProcess(tribe))
                return default;

            _relocationService.StartRelocationProcess(tribe);
            return default;
        }

        INotification ITask.End(HumanTask taskToEnd, Tribe tribeMaterializedData, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            throw new NotImplementedException();
        }

    }
}
