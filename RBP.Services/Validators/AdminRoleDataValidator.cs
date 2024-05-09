using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.Services.Validators
{
    public class AdminRoleDataValidator : GeneralValidator<AdminRoleData>
    {
        public override void Validate(AdminRoleData entity)
        {
            entity.JobTitle.CheckMatch(ValidationSettings.JobTitlePattern, nameof(entity.JobTitle));
        }
    }
}