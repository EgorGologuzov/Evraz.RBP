using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.Services.Validators
{
    public class EmployeeRoleDataValidator : GeneralValidator<EmployeeRoleData>
    {
        public IHandbookRepository Repository { get; }

        public EmployeeRoleDataValidator(IHandbookRepository repository)
        {
            Repository = repository;
        }

        public override void Validate(EmployeeRoleData entity)
        {
            entity.Gender.CheckContains(ValidationSettings.GendersList, nameof(entity.Gender));
            entity.BirthDate.CheckRange(ValidationSettings.BirthdayMinDate, DateTime.Now.AddYears(ValidationSettings.EmployeeMinAge), nameof(entity.BirthDate));
            entity.EmploymentDate.CheckRange(ValidationSettings.CompanyFoundationDate, DateTime.Now, nameof(entity.EmploymentDate));

            Task<IList<HandbookEntityReturnDto>> task = Repository.GetAll(nameof(WorkshopSegment));
            task.Wait();
            entity.SegmentId.CheckContains(task.Result.Select(e => e.Id), nameof(entity.SegmentId), "В справочнике нет сегмента с таким кодом");
        }
    }
}