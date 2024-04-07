namespace RBP.Web.Models
{
    public class HandbookEntityListViewModel : ClientBasedViewModel
    {
        public HandbookData Handbook { get; set; }
        public IList<HandbookEntityData> Entities { get; set; }

        public HandbookEntityListViewModel(string pageTitle, AccountData client, HandbookData handbook, IList<HandbookEntityData> entities) : base(pageTitle, client)
        {
            Entities = entities;
            Handbook = handbook;
        }
    }
}