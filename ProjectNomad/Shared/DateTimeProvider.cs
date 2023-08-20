namespace ProjectNomad.Shared
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
