using RBP.Services.Static;

namespace RBP.Web.Properties
{
    public class RoleInfo
    {
        public string Title { get; set; }
        public Dictionary<string, string> Functions { get; set; }
    }

    public class Roles
    {
        public static readonly Dictionary<string, RoleInfo> Config = new()
        {
            {
                ClientRoles.Admin,
                new()
                {
                    Title = "Администратор",
                    Functions = new Dictionary<string, string>
                    {
                        { "Главная", "/Home/Index" },
                        { "Ведомости", "/Statement/Index" },
                        { "Администраторы", "/Admin/Index" },
                        { "Сотрудники", "/Employee/Index" },
                        { "Справочники", "/Handbook/Index" },
                        { "Продукция", "/Product/Index" },
                        { "Обновить пароль", "/Account/UpdatePassword"},
                    }
                }
            },
            {
                ClientRoles.Employee,
                new()
                {
                    Title = "Сотрудник",
                    Functions = new Dictionary<string, string>
                    {
                        { "Главная", "/Home/Index" },
                        { "Ведомости", "/Statement/Index" },
                        { "Справочники", "/Handbook/Index" },
                        { "Продукция", "/Product/Index" },
                        { "Обновить пароль", "/Account/UpdatePassword"},
                    }
                }
            }
        };
    }
}