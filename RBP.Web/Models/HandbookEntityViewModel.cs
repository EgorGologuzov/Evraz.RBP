using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class HandbookEntityViewModel : ClientBasedViewModel
    {
        public Handbook Handbook { get; set; }
        public HandbookEntityReturnDto Entity { get; set; }
        public string HandbookName => Handbook.Name;

        public HandbookEntityViewModel(string pageTitle, AccountReturnDto client, Handbook handbook, HandbookEntityReturnDto entity) : base(pageTitle, client)
        {
            Entity = entity;
            Handbook = handbook;
        }
    }
}