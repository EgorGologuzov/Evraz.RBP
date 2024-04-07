using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class StatementData
    {
        public Guid Id { get; set; }
        public StatementType Type { get; set; }
        public DateTime Date { get; set; }
        public ProductData Product { get; set; }
        public int Weight { get; set; }
        public AccountData Responsible { get; set; }
        public HandbookEntityData Segment { get; set; }
        public IList<StatementDefectData> Defects { get; set; }
        public string Comment { get; set; }

        public StatementData()
        {
            Defects = new List<StatementDefectData>();
        }
    }
}