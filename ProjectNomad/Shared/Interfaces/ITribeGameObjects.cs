namespace ProjectNomad.Shared.Interfaces
{
    public interface ITribeGameObjects
    {
        ITribe Tribe { get; }
        IEnumerable<IHumanUnit> HumanUnits { get; }
    }
}