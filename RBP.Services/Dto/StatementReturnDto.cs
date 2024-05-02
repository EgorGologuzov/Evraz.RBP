using RBP.Services.Models;

namespace RBP.Services.Dto
{
    public class StatementReturnDto
    {
        public Guid Id { get; set; }
        public StatementType Type { get; set; }
        public DateTime Date { get; set; }
        public ProductReturnDto Product { get; set; }
        public int Weight { get; set; }
        public AccountReturnDto Responsible { get; set; }
        public HandbookEntityReturnDto Segment { get; set; }
        public IList<StatementDefectReturnDto> Defects { get; set; }
        public string Comment { get; set; }

        public StatementReturnDto()
        {
            Defects = new List<StatementDefectReturnDto>();
        }
    }
}