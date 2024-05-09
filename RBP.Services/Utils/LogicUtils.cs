namespace RBP.Services.Utils
{
    public static class LogicUtils
    {
        public static T Min<T>(T value1, T value2) where T : IComparable<T>
        {
            return value1.CompareTo(value2) < 0 ? value1 : value2;
        }

        public static DateTime StripSeconds(this DateTime value)
        {
            return new(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
        }
    }
}