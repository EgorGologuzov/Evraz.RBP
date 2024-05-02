using RBP.Services.Dto;

namespace RBP.Web.Models
{
    public class ProductViewModel : ClientBasedViewModel
    {
        public const string PropertyViewKey = "Key";
        public const string PropertyViewValue = "Value";

        public ProductReturnDto Product { get; set; }
        public IList<HandbookEntityReturnDto> AllProfiles { get; set; }
        public IList<HandbookEntityReturnDto> AllSteels { get; set; }

        public HandbookEntityReturnDto Profile => AllProfiles.First(p => p.Id == Product.ProfileId);
        public HandbookEntityReturnDto Steel => AllSteels.First(s => s.Id == Product.SteelId);

        public ProductViewModel(
            string pageTitle,
            AccountReturnDto client,
            ProductReturnDto product,
            IList<HandbookEntityReturnDto> allProfiles,
            IList<HandbookEntityReturnDto> allSteels) : base(pageTitle, client)
        {
            Product = product;
            AllProfiles = allProfiles;
            AllSteels = allSteels;
        }
    }
}