using GameModule.Entities;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.TasksLogic
{
    //todo: ITaskFromOrder
    internal interface ITaskFromJobStart
    {
        internal (HumanTask? HumanTask, INotification Notification) Start(
            Human human,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles);
    }

    internal interface IAutoTaskStart
    {
        internal (HumanTask? HumanTask, INotification Notification) Start(
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