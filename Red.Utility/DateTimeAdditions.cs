using System;
namespace Red.Utility
{
    static public class DateTimeAdditions
    {
        static public DateTime FirstSecondOnDay(this DateTime aDateTime)
        {
            return aDateTime.Date;
        }

        static public DateTime LastSecondOnDay(this DateTime aDateTime)
        {
            return aDateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        static public bool IsBetweenDates(this DateTime aDateTime, DateTime minimum, DateTime maximum)
        {
            return aDateTime >= minimum && aDateTime <= maximum;
        }

        static public bool IsSameDate(this DateTime aDateTime)
        {
            return aDateTime.IsBetweenDates(aDateTime.FirstSecondOnDay(), aDateTime.LastSecondOnDay());
        }
    }
}
