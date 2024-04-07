using RBP.Services.Models;
using RBP.Services.Utils;

namespace RBP.Web.Models
{
    public class StatementViewModel : ClientBasedViewModel
    {
        private EmployeeRoleData _responsible;
        private string _defectsJson;

        public StatementData Statement { get; set; }
        public IList<ProductData> AllProducts { get; set; }
        public IList<AccountData> AllEmployees { get; set; }
        public IList<HandbookEntityData> AllDefects { get; set; }
        public IList<HandbookEntityData> AllSegments { get; set; }

        public IDictionary<StatementType, string> StatementTypesNames => StatementTypesConfig.Names;
        public EmployeeRoleData Responsible => _responsible ??= Statement.Responsible.RoleDataJson.FromJson<EmployeeRoleData>();
        public Guid? ProductId => Statement.Product?.Id;
        public Guid? ResponsibleId => Statement.Responsible?.Id;
        public string DefectsJson => _defectsJson ??= Statement.Defects.ToJson();
        public int? SegmentId => Statement.Segment?.Id;

        public StatementViewModel(string pageTitle, AccountData client, StatementData statement, IList<ProductData> allProducts, IList<AccountData> allEmployees, IList<HandbookEntityData> allDefects, IList<HandbookEntityData> allSegments) : base(pageTitle, client)
        {
            Statement = statement;
            AllProducts = allProducts;
            AllEmployees = allEmployees;
            AllDefects = allDefects;
            AllSegments = allSegments;
        }
    }
}