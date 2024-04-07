using RBP.Services.Utils;

namespace RBP.Web.Models
{
    public class EmployeeViewModel : ClientBasedViewModel
    {
        private EmployeeRoleData _employeeData;

        public AccountData EmployeeAccount { get; set; }
        public IList<HandbookEntityData> AllSegments { get; set; }

        public EmployeeRoleData EmployeeData => _employeeData ??= EmployeeAccount.RoleDataJson.FromJson<EmployeeRoleData>();
        public HandbookEntityData Segment => AllSegments.First(s => s.Id == EmployeeData.SegmentId);
        public IList<string> AllGenders => EmployeeRoleData.Genders;

        public EmployeeViewModel(string pageTitle, AccountData client, AccountData employee, IList<HandbookEntityData> allSegments) : base(pageTitle, client)
        {
            EmployeeAccount = employee;
            AllSegments = allSegments;
        }
    }
}