﻿namespace RBP.Services.Models
{
    public class StatementDefect
    {
        public Guid StatementId { get; set; }
        public int DefectId { get; set; }
        public Defect Defect { get; set; }
        public decimal Size { get; set; }
    }
}