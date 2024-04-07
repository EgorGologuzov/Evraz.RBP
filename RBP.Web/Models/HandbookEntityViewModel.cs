namespace RBP.Web.Models
{
    public class HandbookEntityViewModel : ClientBasedViewModel
    {
        public HandbookData Handbook { get; set; }
        public HandbookEntityData Entity { get; set; }
        public string HandbookName => Handbook.Name;

        public HandbookEntityViewModel(string pageTitle, AccountData client, HandbookData handbook, HandbookEntityData entity) : base(pageTitle, client)
        {
            Entity = entity;
            Handbook = handbook;
        }
    }
}