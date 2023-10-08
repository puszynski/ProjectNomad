using GameModule.Entities;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal interface ITask
    {
        internal INotification Start(HumanUnitTaskOrder taskOrder, 
            Tribe tribeMaterializedData,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles); 

        internal INotification End(HumanUnitTask taskToEnd, 
            Tribe tribeMaterializedData,
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles);
    }
}
