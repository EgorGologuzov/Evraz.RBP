using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using RBP.Db.Repositories;
using RBP.Services.Models;
using RBP.Services.Utils;
using RBP.Services.Validators;

namespace RBP.Db
{
    static class Program
    {
        static async Task Main()
        {
            ProductRepository repos = new(new PostgresContext(),new NullLogger<ProductRepository>(), new ProductValidator());

            try
            {
                Product result = await repos.Create(new Product
                {
                    Name = "КР70, 10 м.",
                    ProfileId = 5,
                    SteelId = 1,
                    PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 4121-76\"},{\"Key\":\"Длина\",\"Value\":\"12 м.\"}]",
                    Comment = "Comment"
                });

                Console.WriteLine(result.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine((ex.InnerException as PostgresException).ConstraintName);
            }
        }
    }
}