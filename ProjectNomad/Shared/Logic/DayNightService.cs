namespace ProjectNomad.Shared.Logic
{
    /// <summary>
    /// Goals of day-night:
    ///  - block making task during night + sleeping task assigned for night time duration
    ///  - on multiday trips - extend time with nights (for sleep out of camp)
    ///     !? should mark that on human?    ///     
    ///  - affecting temperature and environment
    /// </summary>
    public static class DayNightService
    {
        const int DayDurationInMinutes = 10;
        const int NightDurationInMinutes = 4; //note - in future will depend on season and localization

        public static bool IsNight(DateTime now) 
            => now.Minute >= DayDurationInMinutes - NightDurationInMinutes;

        public static EDayNightType GetDayNightType(DateTime now)
            => (now.Minute % 10 > DayDurationInMinutes - NightDurationInMinutes) ? EDayNightType.Night
            : (now.Minute % 10 == 1) ? EDayNightType.Morning
            : (now.Minute % 10 == DayDurationInMinutes - NightDurationInMinutes) ? EDayNightType.Evening : EDayNightType.Day;

        public static bool IsAbleToEndTaskInOneDayButNotToday(DateTime now, TimeSpan taskDuration)
        {
            var actualMinute = now.Minute % 10;
            var amountOfMinutes = actualMinute + taskDuration.Minutes;

            return amountOfMinutes <= DayDurationInMinutes - NightDurationInMinutes;
        }

        //co jeśli task trwa np 7,8,9.. ? => wydłużamy o noc?
        public static bool IsMultidayTask(TimeSpan taskDuration) 
            => taskDuration.Minutes > DayDurationInMinutes - NightDurationInMinutes;

        //public TimeSpan ExtendTaskDurationWithNightSleep(DateTime now, TimeSpan originalTaskDuration)
        //    => todo..
    }

    public enum EDayNightType
    {
        Morning,
        Day,
        Evening,
        Night
    }
}
