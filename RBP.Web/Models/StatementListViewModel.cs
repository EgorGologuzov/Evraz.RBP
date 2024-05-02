using RBP.Services.Dto;

namespace RBP.Web.Models
{
    public class StatementListViewModel : ClientBasedViewModel
    {
        public IList<StatementViewModel> Statements { get; set; }
        public HandbookEntityReturnDto Segment { get; set; }
        public AccountReturnDto Employee { get; set; }
        public DateTime Date { get; set; }

        public int SegmentId => Segment.Id;
        public Guid EmployeeId => Employee.Id;

        public StatementListViewModel(string pageTitle, AccountReturnDto client, HandbookEntityReturnDto segment, DateTime date, IList<StatementViewModel> statements, AccountReturnDto employee) : base(pageTitle, client)
        {
            Statements = statements;
            Segment = segment;
            Date = date;
            Employee = employee;
        }
    }
}