namespace RBP.Services.Models
{
    public struct DateIntPare
    {
        public DateTime Key { get; set; }
        public int Value { get; set; }

        public DateIntPare(DateTime key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}