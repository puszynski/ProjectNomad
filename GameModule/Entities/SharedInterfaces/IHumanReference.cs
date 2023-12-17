namespace GameModule.Entities.SharedInterfaces
{
    internal interface IHumanReference
    {
        public int HumanId { get; set; }
        public Human Human { get; set; }
    }
}
