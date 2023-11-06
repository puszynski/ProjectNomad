namespace ProjectNomad.Shared.Interfaces
{
    public interface ITribeGameObjects
    {
        ITribe Tribe { get; }
        IEnumerable<IHuman> HumanUnits { get; }

        //todo
        IWorldParameters WorldParameters { get; }
    }
}