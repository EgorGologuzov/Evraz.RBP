using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class AdminViewModel : ClientBasedViewModel
    {
        private AdminRoleData _adminData;

        public AccountReturnDto AdminAccount { get; set; }

        public AdminRoleData AdminData => _adminData ??= AdminAccount.RoleDataJson.FromJson<AdminRoleData>();

        public AdminViewModel(string pageTitle, AccountReturnDto client, AccountReturnDto adminAccount) : base(pageTitle, client)
        {
            AdminAccount = adminAccount;
        }
    }
}