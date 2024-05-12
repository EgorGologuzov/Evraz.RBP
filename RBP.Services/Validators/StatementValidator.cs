using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.Services.Validators
{
    public class StatementValidator : IValidator<Statement>
    {
        private static readonly int _typeMin = Enum.GetValues(typeof(StatementType)).Cast<int>().Min();
        private static readonly int _typeMax = Enum.GetValues(typeof(StatementType)).Cast<int>().Max();

        public void Validate(Statement statement)
        {
            ((int)statement.Type).CheckRange(_typeMin, _typeMax, nameof(statement.Type), "Некорректно указан тип ведомости");
            statement.Date.CheckRange(ValidationSettings.CompanyFoundationDate, DateTime.Now, nameof(statement.Date));
            statement.Weight.CheckRange(1, ValidationSettings.StatementMaxWeight, nameof(statement.Weight), "Вес должен быть положительным целым числом");
            statement.Comment.CheckLength(0, ValidationSettings.MaxCommentLength, nameof(statement.Comment), "Длина комментария превышена");
            statement.Defects.CheckNotNull(nameof(statement.Defects));
            statement.Defects.CheckCount(0, ValidationSettings.StatementMaxDefectsCount, nameof(statement.Defects), "Количество деффектов превышено");
            statement.Defects.CheckEach(d => d.Size.CheckRange(0.001m, decimal.MaxValue, nameof(d.Size), "Размер деффекта должен быть положительной десятичной дробью"));
            statement.Defects.CheckUnique((d1, d2) => d1.DefectId == d2.DefectId, nameof(statement.Defects), "Деффекты повторяются");
        }
    }
}