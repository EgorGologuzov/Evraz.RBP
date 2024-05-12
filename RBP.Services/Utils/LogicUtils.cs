using RBP.Services.Models;

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

        public static List<DateIntPare> FillFullListPreviosValues(IList<DateIntPare> list, DateTime start, DateTime end)
        {
            List<DateIntPare> updatedList = new();

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                DateIntPare existing = list.FirstOrDefault(p => p.Key.Date == date.Date);

                if (existing.Key != default)
                {
                    updatedList.Add(existing);
                }
                else
                {
                    DateIntPare previousPair = updatedList.LastOrDefault(pair => pair.Key < date);
                    int value = previousPair.Key != default ? previousPair.Value : 0;
                    updatedList.Add(new DateIntPare(date, value));
                }
            }

            return updatedList;
        }

        public static List<DateIntPare> FillFullListZeroValues(IList<DateIntPare> list, DateTime start, DateTime end)
        {
            List<DateIntPare> updatedList = new();

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                DateIntPare existing = list.FirstOrDefault(p => p.Key.Date == date.Date);

                if (existing.Key != default)
                {
                    updatedList.Add(existing);
                }
                else
                {
                    updatedList.Add(new DateIntPare(date, 0));
                }
            }

            return updatedList;
        }
    }
}