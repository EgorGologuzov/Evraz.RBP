namespace RBP.Web.Models
{
    public class AdminListViewModel : ClientBasedViewModel
    {
        public string SearchRequest { get; set; }
        public IList<AdminViewModel> Admins { get; set; }

        public AdminListViewModel(string pageTitle, AccountData client, string searchRequest, IList<AdminViewModel> admins) : base(pageTitle, client)
        {
            SearchRequest = searchRequest;
            Admins = admins;
        }
    }
}