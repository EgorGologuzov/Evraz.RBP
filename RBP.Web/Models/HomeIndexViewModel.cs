using RBP.Services.Dto;

namespace RBP.Web.Models
{
    public class HomeIndexViewModel : ClientBasedViewModel
    {
        public HomeIndexViewModel(string pageTitle, AccountReturnDto client) : base(pageTitle, client)
        {
        }

        public string Text { get; set; }
    }
}