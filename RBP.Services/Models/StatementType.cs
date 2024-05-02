namespace RBP.Services.Models
{
    public enum StatementType
    {
        Accept = 0,
        Storage = 1,
        Transfer = 2,
        Shipment = 3
    }

    public static class StatementTypesConfig
    {
        public static readonly Dictionary<StatementType, string> Names = new()
        {
            { StatementType.Accept, "Приемка" },
            { StatementType.Storage, "Хранение" },
            { StatementType.Transfer, "Передача" },
            { StatementType.Shipment, "Отгрузка" },
        };
    }
}