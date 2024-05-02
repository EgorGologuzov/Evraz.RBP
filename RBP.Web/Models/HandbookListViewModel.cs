using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class HandbookListViewModel : ClientBasedViewModel
    {
        public IList<Handbook> Handbooks { get; set; }

        public HandbookListViewModel(string pageTitle, AccountReturnDto client, IList<Handbook> handbooks) : base(pageTitle, client)
        {
            Handbooks = handbooks;
        }
    }
}