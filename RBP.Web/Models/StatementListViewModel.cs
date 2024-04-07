namespace RBP.Web.Models
{
    public class StatementListViewModel : ClientBasedViewModel
    {
        public IList<StatementViewModel> Statements { get; set; }
        public HandbookEntityData Segment { get; set; }
        public AccountData Employee { get; set; }
        public DateTime Date { get; set; }

        public int SegmentId => Segment.Id;
        public Guid EmployeeId => Employee.Id;

        public StatementListViewModel(string pageTitle, AccountData client, HandbookEntityData segment, DateTime date, IList<StatementViewModel> statements, AccountData employee) : base(pageTitle, client)
        {
            Statements = statements;
            Segment = segment;
            Date = date;
            Employee = employee;
        }
    }
}