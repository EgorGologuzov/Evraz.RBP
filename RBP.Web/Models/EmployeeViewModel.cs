using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class EmployeeViewModel : ClientBasedViewModel
    {
        private EmployeeRoleData _employeeData;

        public AccountReturnDto EmployeeAccount { get; set; }
        public IList<HandbookEntityReturnDto> AllSegments { get; set; }

        public EmployeeRoleData EmployeeData => _employeeData ??= EmployeeAccount.RoleDataJson.FromJson<EmployeeRoleData>();
        public HandbookEntityReturnDto Segment => AllSegments.First(s => s.Id == EmployeeData.SegmentId);
        public IList<string> AllGenders => EmployeeRoleData.Genders;

        public EmployeeViewModel(string pageTitle, AccountReturnDto client, AccountReturnDto employee, IList<HandbookEntityReturnDto> allSegments) : base(pageTitle, client)
        {
            EmployeeAccount = employee;
            AllSegments = allSegments;
        }
    }
}