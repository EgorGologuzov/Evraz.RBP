namespace RBP.Services.Models
{
    public struct StringIntPare
    {
        public string Key { get; set; }
        public int Value { get; set; }

        public StringIntPare(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}