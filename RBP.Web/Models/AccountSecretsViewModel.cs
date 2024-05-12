using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class AccountSecretsViewModel : ClientBasedViewModel
    {
        public AccountSecrets Secrets { get; set; }

        public AccountSecretsViewModel(string pageTitle, AccountReturnDto client, AccountSecrets secrets) : base(pageTitle, client)
        {
            Secrets = secrets;
        }
    }
}