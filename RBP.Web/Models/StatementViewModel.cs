using RBP.Services.Models;
using RBP.Services.Utils;
using RBP.Services.Dto;

namespace RBP.Web.Models
{
    public class StatementViewModel : ClientBasedViewModel
    {
        private EmployeeRoleData _responsible;
        private string _defectsJson;

        public StatementReturnDto Statement { get; set; }
        public IList<ProductReturnDto> AllProducts { get; set; }
        public IList<AccountReturnDto> AllEmployees { get; set; }
        public IList<HandbookEntityReturnDto> AllDefects { get; set; }
        public IList<HandbookEntityReturnDto> AllSegments { get; set; }

        public IDictionary<StatementType, string> StatementTypesNames => StatementTypesConfig.Names;
        public EmployeeRoleData Responsible => _responsible ??= Statement.Responsible.RoleDataJson.FromJson<EmployeeRoleData>();
        public Guid? ProductId => Statement.Product?.Id;
        public Guid? ResponsibleId => Statement.Responsible?.Id;
        public string DefectsJson => _defectsJson ??= Statement.Defects.ToJson();
        public int? SegmentId => Statement.Segment?.Id;

        public StatementViewModel(string pageTitle, AccountReturnDto client, StatementReturnDto statement, IList<ProductReturnDto> allProducts, IList<AccountReturnDto> allEmployees, IList<HandbookEntityReturnDto> allDefects, IList<HandbookEntityReturnDto> allSegments) : base(pageTitle, client)
        {
            Statement = statement;
            AllProducts = allProducts;
            AllEmployees = allEmployees;
            AllDefects = allDefects;
            AllSegments = allSegments;
        }
    }
}