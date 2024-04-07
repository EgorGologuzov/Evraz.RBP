namespace RBP.Web.Models
{
    public class DeleteViewModel : ClientBasedViewModel
    {
        public string WarningMessage { get; set; }

        public DeleteViewModel(string pageTitle, AccountData client, string warningMessage) : base(pageTitle, client)
        {
            WarningMessage = warningMessage;
        }
    }
}