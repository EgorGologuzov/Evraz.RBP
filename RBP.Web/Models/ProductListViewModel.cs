using RBP.Services.Dto;

namespace RBP.Web.Models
{
    public class ProductListViewModel : ClientBasedViewModel
    {
        public IList<ProductViewModel> Products { get; set; }
        public string SearchRequest { get; set; }

        public ProductListViewModel(string pageTitle, AccountReturnDto client, IList<ProductViewModel> products, string searchRequest) : base(pageTitle, client)
        {
            Products = products;
            SearchRequest = searchRequest;
        }
    }
}