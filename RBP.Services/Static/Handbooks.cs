using RBP.Services.Models;

namespace RBP.Web.Properties
{
    public class Handbooks
    {
        public static readonly List<Handbook> Config = new()
        {
            new()
            {
                Name = "RailProfile",
                Title = "Профили",
                Comment = "Виды профилей рельсобалочной продукции"
            },
            new()
            {
                Name = "SteelGrade",
                Title = "Марки стали",
                Comment = "Марки стали, используемые в производстве"
            },
            new()
            {
                Name = "Defect",
                Title = "Дефекты",
                Comment = "Виды дефектов, присущие продукции"
            },
            new()
            {
                Name = "WorkshopSegment",
                Title = "Сегменты цеха",
                Comment = "Части цеха, в которых может находиться продукция"
            },
        };
    }
}