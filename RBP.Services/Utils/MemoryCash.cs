using Newtonsoft.Json.Linq;
using RBP.Services.Interfaces;

namespace RBP.Services.Utils
{
    public class MemoryCash<TValue> where TValue : class
    {
        private readonly Dictionary<string, TValue> _items = new();
        private readonly SortedDictionary<DateTime, string> _times = new();
        private readonly int _itemsLimit = 100;
        private readonly int _clearCount = 10;

        public TValue? Get(string key)
        {
            return _items.ContainsKey(key) ? _items[key] : null;
        }

        public void Set(string key, TValue? value)
        {
            if (_items.Count <= _itemsLimit || _items.ContainsKey(key))
            {
                _items.Add(key, value);
                _times.Add(DateTime.Now, key);
            }

            int i = 0;
            foreach (var item in _items)
            {
                if (i >= _clearCount)
                {
                    break;
                }

                _items.Remove(item.Key);
                i++;
            }

            _items.Add(key, value);
            _times.Add(DateTime.Now, key);
        }

        public void Remove(string key)
        {
            if (_items.ContainsKey(key))
            {
                _items.Remove(key);
            }

            DateTime timeKey = _times.FirstOrDefault((p) => p.Value == key).Key;
            if (timeKey != default)
            {
                _times.Remove(timeKey);
            }
        }
    }
}