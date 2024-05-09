using AutoMapper;
using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AccountCreateDto, Account>().ReverseMap();
            CreateMap<AccountReturnDto, Account>().ReverseMap();
            CreateMap<AccountUpdateDto, Account>().ReverseMap();
            CreateMap<AdminCreateDto, Account>().ReverseMap();
            CreateMap<AdminCreateDto, AccountReturnDto>().ReverseMap();
            CreateMap<AdminCreateDto, AdminRoleData>().ReverseMap();
            CreateMap<AdminUpdateDto, Account>().ReverseMap();
            CreateMap<AdminUpdateDto, AccountReturnDto>().ReverseMap();
            CreateMap<AdminUpdateDto, AdminRoleData>().ReverseMap();
            CreateMap<EmployeeCreateDto, Account>().ReverseMap();
            CreateMap<EmployeeCreateDto, AccountReturnDto>().ReverseMap();
            CreateMap<EmployeeCreateDto, EmployeeRoleData>().ReverseMap();
            CreateMap<EmployeeUpdateDto, Account>().ReverseMap();
            CreateMap<EmployeeUpdateDto, AccountReturnDto>().ReverseMap();
            CreateMap<EmployeeUpdateDto, EmployeeRoleData>().ReverseMap();
            CreateMap<HandbookEntityCreateDto, HandbookEntityReturnDto>().ReverseMap();
            CreateMap<HandbookEntityUpdateDto, HandbookEntityReturnDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>().ReverseMap();
            CreateMap<ProductCreateDto, ProductReturnDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>().ReverseMap();
            CreateMap<ProductUpdateDto, ProductReturnDto>().ReverseMap();
            CreateMap<ProductReturnDto, Product>().ReverseMap();
            CreateMap<StatementCreateDto, Statement>().ReverseMap();
            CreateMap<StatementDefectCreateDto, StatementDefect>().ReverseMap();
            CreateMap<StatementDefectReturnDto, StatementDefect>().ReverseMap();
            CreateMap<StatementReturnDto, Statement>().ReverseMap();
            CreateMap<WebStatementCreateDto, Statement>().ReverseMap();
            CreateMap<WebStatementCreateDto, WebStatementReturnDto>().ReverseMap();
        }
    }
}