using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Interfaces.Properties;

namespace GameModule.Entities
{
    // PROBLEM WITH LAST MIGRATION (NOT APPLAYED)
    // ClientConnectionId:5128da67-00f0-4e19-a9ca-0e5367d5ce57
    //    Error Number:1785,State:0,Class:16
    //Introducing FOREIGN KEY constraint 'FK_HumanUnitTasks_Tribes_TribeId' on table 'HumanUnitTasks' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
    //Could not create constraint or index.See previous errors.
    // => https://stackoverflow.com/questions/17127351/introducing-foreign-key-constraint-may-cause-cycles-or-multiple-cascade-paths

    /// <summary>
    /// flow:
    /// 1. Order to relocate
    /// 2. w8 for preparate needed resources
    /// 3. Run task - one task but displayed in 3 phase: a) Packing stuff b) Relocating c) Unpacking stuff
    /// </summary>
    internal class TribeRelocation : IId
    {
        public int Id { get; set; }
        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Localization Start { get; set; }
        public Localization Destiny { get; set; }
    }
}