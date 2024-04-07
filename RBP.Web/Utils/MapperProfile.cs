using AutoMapper;
using RBP.Web.Dto;
using RBP.Web.Models;

namespace RBP.Web.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EmployeeCreateDto, AccountData>().ReverseMap();
            CreateMap<EmployeeCreateDto, EmployeeRoleData>().ReverseMap();
            CreateMap<EmployeeUpdateDto, AccountData>().ReverseMap();
            CreateMap<EmployeeUpdateDto, EmployeeRoleData>().ReverseMap();
            CreateMap<HandbookEntityUpdateDto, HandbookEntityData>().ReverseMap();
            CreateMap<HandbookEntityCreateDto, HandbookEntityData>().ReverseMap();
            CreateMap<ProductUpdateDto, ProductData>().ReverseMap();
            CreateMap<ProductCreateDto, ProductData>().ReverseMap();
            CreateMap<StatementCreateDto, StatementData>().ReverseMap();
            CreateMap<AdminCreateDto, AccountData>().ReverseMap();
            CreateMap<AdminCreateDto, AdminRoleData>().ReverseMap();
            CreateMap<AdminUpdateDto, AccountData>().ReverseMap();
            CreateMap<AdminUpdateDto, AdminRoleData>().ReverseMap();
        }
    }
}