using AutoMapper;
using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EmployeeCreateDto, AccountReturnDto>().ReverseMap();
            CreateMap<EmployeeCreateDto, EmployeeRoleData>().ReverseMap();
            CreateMap<EmployeeUpdateDto, AccountReturnDto>().ReverseMap();
            CreateMap<EmployeeUpdateDto, EmployeeRoleData>().ReverseMap();
            CreateMap<HandbookEntityUpdateDto, HandbookEntityReturnDto>().ReverseMap();
            CreateMap<HandbookEntityCreateDto, HandbookEntityReturnDto>().ReverseMap();
            CreateMap<ProductUpdateDto, ProductReturnDto>().ReverseMap();
            CreateMap<ProductCreateDto, ProductReturnDto>().ReverseMap();
            CreateMap<StatementCreateDto, StatementReturnDto>().ReverseMap();
            CreateMap<AdminCreateDto, AccountReturnDto>().ReverseMap();
            CreateMap<AdminCreateDto, AdminRoleData>().ReverseMap();
            CreateMap<AdminUpdateDto, AccountReturnDto>().ReverseMap();
            CreateMap<AdminUpdateDto, AdminRoleData>().ReverseMap();
        }
    }
}