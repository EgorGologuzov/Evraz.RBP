namespace RBP.Web.Dto
{
    public class ProductUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public int SteelId { get; set; }
        public string PropertiesJson { get; set; }
        public string Comment { get; set; }
    }
}