using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class HandbookEntityListViewModel : ClientBasedViewModel
    {
        public Handbook Handbook { get; set; }
        public IList<HandbookEntityReturnDto> Entities { get; set; }

        public HandbookEntityListViewModel(string pageTitle, AccountReturnDto client, Handbook handbook, IList<HandbookEntityReturnDto> entities) : base(pageTitle, client)
        {
            Entities = entities;
            Handbook = handbook;
        }
    }
}