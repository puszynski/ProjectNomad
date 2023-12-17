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
        public const int DAY_DURATION_MINUTES = 10; //means 24h => day + night
        public const int NIGHT_DURATION_MINUTES = 4; //note - in future will depend on season and localization

        public static bool IsNight(DateTime now) 
            => now.Minute % 10 >= DAY_DURATION_MINUTES - NIGHT_DURATION_MINUTES;

        public static EDayNightType GetDayNightType(DateTime now)
            => (now.Minute % 10 > DAY_DURATION_MINUTES - NIGHT_DURATION_MINUTES) ? EDayNightType.Night
            : (now.Minute % 10 == 1) ? EDayNightType.Morning
            : (now.Minute % 10 == DAY_DURATION_MINUTES - NIGHT_DURATION_MINUTES) ? EDayNightType.Evening : EDayNightType.Day;

        public static bool IsAbleToEndTaskInOneDayButNotToday(DateTime now, TimeSpan taskDuration)
        {
            var actualMinute = now.Minute % 10;
            var amountOfMinutes = actualMinute + taskDuration.Minutes;

            return amountOfMinutes <= DAY_DURATION_MINUTES - NIGHT_DURATION_MINUTES;
        }

        //co jeśli task trwa np 7,8,9.. ? => wydłużamy o noc?
        public static bool IsMultidayTask(TimeSpan taskDuration) 
            => taskDuration.Minutes > DAY_DURATION_MINUTES - NIGHT_DURATION_MINUTES;

        //public TimeSpan ExtendTaskDurationWithNightSleep(DateTime now, TimeSpan originalTaskDuration)
        //    => todo..

        public static int TotalDays(DateTime from, DateTime to) 
            => (int)((to - from).TotalMinutes % 10);
    }

    public enum EDayNightType
    {
        Morning,
        Day,
        Evening,
        Night
    }
}
