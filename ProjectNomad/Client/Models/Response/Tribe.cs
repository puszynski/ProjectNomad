using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public class Tribe : ITribe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Wood { get; set; }
        public int FreshFood { get; set; }
        public ETribeRelocationStatus RelocationStatus { get; set; }
    }
}
