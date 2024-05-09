namespace RBP.Services.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public RailProfile Profile { get; set; }
        public int SteelId { get; set; }
        public SteelGrade Steel { get; set; }
        public string PropertiesJson { get; set; }
        public string? Comment { get; set; }
    }
}