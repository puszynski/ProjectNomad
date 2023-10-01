namespace GameModule.Entities.SharedInterfaces
{
    internal interface ITribeReference
    {
        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }
    }
}
