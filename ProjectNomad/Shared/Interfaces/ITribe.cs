namespace ProjectNomad.Shared.Interfaces
{
    public interface ITribe
    {
        public int Id { get; }
        public string Name { get; }
        public int X { get; }
        public int Y { get; }
        public int Wood { get; }
        public int FreshFood { get; set; }
    }
}
