using RBP.Services.Utils;

namespace RBP.Web.Models
{
    public class AdminViewModel : ClientBasedViewModel
    {
        private AdminRoleData _adminData;

        public AccountData AdminAccount { get; set; }

        public AdminRoleData AdminData => _adminData ??= AdminAccount.RoleDataJson.FromJson<AdminRoleData>();

        public AdminViewModel(string pageTitle, AccountData client, AccountData adminAccount) : base(pageTitle, client)
        {
            AdminAccount = adminAccount;
        }
    }
}