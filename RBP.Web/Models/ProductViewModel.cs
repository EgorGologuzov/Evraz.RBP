namespace RBP.Web.Models
{
    public class ProductViewModel : ClientBasedViewModel
    {
        public const string PropertyViewKey = "Key";
        public const string PropertyViewValue = "Value";

        public ProductData Product { get; set; }
        public IList<HandbookEntityData> AllProfiles { get; set; }
        public IList<HandbookEntityData> AllSteels { get; set; }

        public HandbookEntityData Profile => AllProfiles.First(p => p.Id == Product.ProfileId);
        public HandbookEntityData Steel => AllSteels.First(s => s.Id == Product.SteelId);

        public ProductViewModel(
            string pageTitle,
            AccountData client,
            ProductData product,
            IList<HandbookEntityData> allProfiles,
            IList<HandbookEntityData> allSteels) : base(pageTitle, client)
        {
            Product = product;
            AllProfiles = allProfiles;
            AllSteels = allSteels;
        }
    }
}