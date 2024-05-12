namespace RBP.Services.Static
{
    public static class ValidationSettings
    {
        public const int EmployeeMinAge = 18;
        public const int MaxCommentLength = 150;
        public const int PasswordHashLength = 64;
        public const int ProductPropertiesMaxCount = 30;
        public const int StatementMaxWeight = 1_000_000;
        public const int StatementMaxDefectsCount = 10;
        public const string FioPattern = "^[А-ЯЁ][а-яё]{0,30} [А-ЯЁ][а-яё]{0,30} [А-ЯЁ][а-яё]{0,30}$";
        public const string HandbookEntityNamePattern = "^.{1,30}$";
        public const string JobTitlePattern = @"^[А-ЯЁа-яё\d\s]{1,40}$";
        public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,25}$";
        public const string PasswordMessage = "Пароль должен содержать от 8 до 25 символов, одну большую и одну маленькую латинские буквы, цифру и спец. символ";
        public const string PhonePattern = @"^\d{10}$";
        public const string ProductNamePattern = "^.{1,40}$";
        public const string PropertyNamePattern = "^.{1,40}$";
        public const string PropertyValuePattern = "^.{1,40}$";
        public static readonly DateTime BirthdayMinDate = new(1900, 01, 01);
        public static readonly DateTime CompanyFoundationDate = new(1930, 01, 01);
        public static readonly DateTime SystemStartDay = new(2024, 01, 01);
        public static readonly string[] GendersList = new string[] { "М", "Ж" };
    }
}