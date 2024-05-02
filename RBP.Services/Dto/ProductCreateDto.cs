namespace RBP.Services.Dto
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public int SteelId { get; set; }
        public string PropertiesJson { get; set; }
        public string Comment { get; set; }
    }
}