using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Validators;

namespace RBP.Services.Static
{
    public static class ClientRoles
    {
        public const string Admin = "Admin";
        public const string Employee = "Employee";

        public static readonly string[] AllRoles = new string[] { Admin, Employee }; // добавлять все роли в список!
    }
}