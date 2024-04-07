namespace RBP.Services.Models
{
    public enum StatementType
    {
        Accept,
        Storage,
        Transfer,
        Shipment
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