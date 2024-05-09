using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.Services.Validators
{
    public class AccountValidator : GeneralValidator<Account>
    {
        public readonly IValidator<AdminRoleData> AdminValidator;
        public readonly IValidator<EmployeeRoleData> EmployeeValidator;

        public AccountValidator(IValidator<AdminRoleData> adminValidator, IValidator<EmployeeRoleData> employeeValidator)
        {
            AdminValidator = adminValidator;
            EmployeeValidator = employeeValidator;
        }

        public override void Validate(Account entity)
        {
            entity.Phone.CheckMatch(ValidationSettings.PhonePattern, nameof(entity.Phone));
            entity.PasswordHash.CheckLength(ValidationSettings.PasswordHashLength, ValidationSettings.PasswordHashLength, nameof(entity.PasswordHash));
            entity.Name.CheckMatch(ValidationSettings.FioPattern, nameof(entity.Name));
            entity.Role.CheckContains(ClientRoles.AllRoles, nameof(entity.Role));
            entity.CreationTime.CheckRange(ValidationSettings.SystemStartDay, DateTime.Now, nameof(entity.CreationTime));
            entity.Comment.CheckLength(0, ValidationSettings.MaxCommentLength, nameof(entity.Comment));

            switch (entity.Role)
            {
                case ClientRoles.Admin:
                    AdminRoleData adminData = entity.RoleDataJson.CheckParseJson<AdminRoleData>(nameof(entity.RoleDataJson));
                    AdminValidator.Validate(adminData);
                    break;
                case ClientRoles.Employee:
                    EmployeeRoleData employeeData = entity.RoleDataJson.CheckParseJson<EmployeeRoleData>(nameof(entity.RoleDataJson));
                    EmployeeValidator.Validate(employeeData);
                    break;
            }
        }
    }
}