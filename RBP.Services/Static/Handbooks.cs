using RBP.Services.Models;

namespace RBP.Web.Properties
{
    public class Handbooks
    {
        public static readonly List<Handbook> Config = new()
        {
            new()
            {
                Name = nameof(RailProfile),
                Title = "Профили",
                Comment = "Виды профилей рельсобалочной продукции"
            },
            new()
            {
                Name = nameof(SteelGrade),
                Title = "Марки стали",
                Comment = "Марки стали, используемые в производстве"
            },
            new()
            {
                Name = nameof(Defect),
                Title = "Дефекты",
                Comment = "Виды дефектов, присущие продукции"
            },
            new()
            {
                Name = nameof(WorkshopSegment),
                Title = "Сегменты цеха",
                Comment = "Части цеха, в которых может находиться продукция"
            },
        };
    }
}