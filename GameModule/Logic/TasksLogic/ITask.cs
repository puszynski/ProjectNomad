using GameModule.Entities;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal interface ITask
    {
        internal INotification Start(HumanTaskOrder taskOrder, 
            Tribe tribeMaterializedData,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles); 

        internal INotification End(HumanTask taskToEnd, 
            Tribe tribeMaterializedData,
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles);
    }
}
