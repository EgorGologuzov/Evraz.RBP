using System.Text;

namespace RBP.API.Utils
{
    public static class ApiUtils
    {
        public static string GeneratePassword(int length = 10)
        {
            const string big = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string small = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_+";
            const string all = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";

            StringBuilder builder = new();
            Random rand = new();

            for (int i = 0; i < length; i++)
            {
                builder.Append(all[rand.Next(0, all.Length)]);
            }

            builder[rand.Next(0, builder.Length)] = big[rand.Next(0, big.Length)];
            builder[rand.Next(0, builder.Length)] = small[rand.Next(0, small.Length)];
            builder[rand.Next(0, builder.Length)] = digits[rand.Next(0, digits.Length)];
            builder[rand.Next(0, builder.Length)] = special[rand.Next(0, special.Length)];

            return builder.ToString();
        }
    }
}