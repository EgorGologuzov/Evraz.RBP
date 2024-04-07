using RBP.Web.Dto;

namespace RBP.Web.Models
{
    public class EmployeeListViewModel : ClientBasedViewModel
    {
        public string? SearchRequest { get; set; }
        public IList<EmployeeViewModel> Employees { get; set; }

        public EmployeeListViewModel(string pageTitle, AccountData client) : base(pageTitle, client)
        {
        }
    }
}