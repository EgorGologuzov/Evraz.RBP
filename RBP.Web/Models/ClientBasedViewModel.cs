using RBP.Web.Properties;

namespace RBP.Web.Models
{
    public class ClientBasedViewModel
    {
        public string PageTitle { get; set; }
        public AccountData Client { get; set; }
        public string? ErrorMessage { get; set; }
        public string? OkMessage { get; set; }

        public string ClientShortName
        {
            get
            {
                string[] list = Client.Name.Split(" ");

                return $"{list[0]} {list[1][0]}.{list[2][0]}.";
            }
        }

        public string ClientRoleTitle => Roles.Config[Client.Role].Title;
        public IDictionary<string, string> MenuOptions => Roles.Config[Client.Role].Functions;

        public ClientBasedViewModel(string pageTitle, AccountData client)
        {
            PageTitle = pageTitle;
            Client = client;
        }

        public bool ClientInRoles(params string[] roles)
        {
            return roles.Contains(Client.Role);
        }
    }
}