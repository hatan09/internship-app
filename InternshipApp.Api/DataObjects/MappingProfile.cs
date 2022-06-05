using AutoMapper;
using InternshipApp.Core.Entities;
using System;
using System.Globalization;
using System.Linq;

namespace InternshipApp.Api.DataObjects
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());

            CreateMap<User, UserDTO>()
                .ForMember(dto => dto.Roles, opt => opt.MapFrom(usr => usr.UserRoles.Select(u_r => u_r.Role!.Name)));
            CreateMap<UserDTO, User>();

            CreateMap<Student, StudentDTO>();
            CreateMap<StudentDTO, Student>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.Birthdate, o => o.MapFrom(dto => DateTime.Parse(dto.Birthdate, null, DateTimeStyles.AssumeUniversal)));

            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());
            CreateMap<CreateCustomerDTO, Customer>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.Birthdate, o => o.MapFrom(dto => DateTime.Parse(dto.Birthdate, null, DateTimeStyles.AssumeUniversal)));
            CreateMap<Customer, GetCustomerDTO>();

            CreateMap<Staff, StaffDTO>();
            CreateMap<StaffDTO, Staff>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());
            CreateMap<CreateStaffDTO, Staff>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.CompanyId, opt => opt.Ignore())
                .ForMember(ent => ent.Birthdate, o => o.MapFrom(dto => DateTime.Parse(dto.Birthdate, null, DateTimeStyles.AssumeUniversal)));
            CreateMap<Staff, GetStaffDTO>();

            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.Guid, opt => opt.Ignore());

            CreateMap<App, AppDTO>();
            CreateMap<AppDTO, App>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.LeaderId, opt => opt.Ignore());
            CreateMap<CreateAppDTO, App>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());

            CreateMap<Report, ReportDTO>();
            CreateMap<ReportDTO, Report>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());

            CreateMap<Bug, BugDTO>();
            CreateMap<BugDTO, Bug>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());
            CreateMap<CreateBugDTO, Bug>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());
        }
    }
}
