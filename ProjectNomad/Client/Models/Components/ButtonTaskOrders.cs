using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Components
{
    internal class ButtonTaskOrders
    {
        public int InProgressCount { get; set; }
        public int AllCount { get; set; }
        public ETaskType Type { get; set; }
        public List<IHumanUnitTaskDto> MatchedTasks = new List<IHumanUnitTaskDto>();
    };
}
