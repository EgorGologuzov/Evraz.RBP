namespace RBP.Web.Models
{
    public class HandbookListViewModel : ClientBasedViewModel
    {
        public IList<HandbookData> Handbooks { get; set; }

        public HandbookListViewModel(string pageTitle, AccountData client, IList<HandbookData> handbooks) : base(pageTitle, client)
        {
            Handbooks = handbooks;
        }
    }
}