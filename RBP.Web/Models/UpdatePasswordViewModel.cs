using RBP.Services.Dto;

namespace RBP.Web.Models
{
    public class UpdatePasswordViewModel : ClientBasedViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string PasswordRepeat { get; set; }

        public UpdatePasswordViewModel(string pageTitle, AccountReturnDto client) : base(pageTitle, client)
        {
        }
    }
}