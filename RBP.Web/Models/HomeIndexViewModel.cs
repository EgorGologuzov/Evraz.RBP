namespace RBP.Web.Models
{
    public class HomeIndexViewModel : ClientBasedViewModel
    {
        public HomeIndexViewModel(string pageTitle, AccountData client) : base(pageTitle, client)
        {
        }

        public string Text { get; set; }
    }
}