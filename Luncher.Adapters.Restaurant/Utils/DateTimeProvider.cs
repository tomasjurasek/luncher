namespace Luncher.Adapters.Restaurant.Utils
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public string GetToday()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            var culture = new System.Globalization.CultureInfo("cs-CZ");
            return culture.DateTimeFormat.GetDayName(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone).DayOfWeek);
        }
    }

    internal interface IDateTimeProvider
    {
        string GetToday();
    }
}
