using RBP.Services.Dto;

namespace RBP.Web.Models
{
    public class ResetPasswordViewModel : ClientBasedViewModel
    {
        public Guid Id { get; set; }
        public string NewPassword { get; set; }
        public string PasswordRepeat { get; set; }

        public ResetPasswordViewModel(string pageTitle, AccountReturnDto client) : base(pageTitle, client)
        {
        }
    }
}