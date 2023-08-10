using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Tasks
{
    //todo in next stages - ViewModel and separated blazor component
    public class AddHumanUnitTaskViewModel : IAddHumanUnitTaskDto
    {
        public AddHumanUnitTaskViewModel(int tribeId, 
            int humanUnitId, 
            EHumanUnitTaskType type, 
            int localizationStart_X, 
            int localizationStart_Y, 
            int localizationEnd_X, 
            int localizationEnd_Y)
        {
            TribeId = tribeId;
            HumanUnitId = humanUnitId;
            Type = type;
            LocalizationStart_X = localizationStart_X;
            LocalizationStart_Y = localizationStart_Y;
            LocalizationEnd_X = localizationEnd_X;
            LocalizationEnd_Y = localizationEnd_Y;
        }

        public int TribeId { get; set; }
        public int HumanUnitId { get; set; }
        public EHumanUnitTaskType Type { get; set; }
        public int LocalizationStart_X { get; set; }
        public int LocalizationStart_Y { get; set; }
        public int LocalizationEnd_X { get; set; }
        public int LocalizationEnd_Y { get; set; }
    }
}
