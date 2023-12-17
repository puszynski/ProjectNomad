using GameModule.Entities;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.TasksLogic
{
    //todo: ITaskFromOrder
    internal interface ITaskFromJobStart
    {
        internal INotification? Start(
            Human human,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles);
    }

    internal interface IAutoTaskStart
    {
        internal INotification? Start(
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles);
    }

    internal interface ITaskEnd
    {
        internal INotification? End(
            HumanTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles);
    }
}