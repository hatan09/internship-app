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
            CreateMap<UserDTO, User>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());

            CreateMap<Student, StudentDTO>().ForMember(dto => dto.Birthdate, opt => opt.MapFrom(ent => ent.Birthdate.Date));
            CreateMap<Student, GetStudentDTO>()
                .ForMember(dto => dto.SkillIds, opt => opt.MapFrom(stu => stu.StudentSkills!.Select(ss => ss.SkillId)))
                .ForMember(dto => dto.IsAccepted, opt => opt.MapFrom(stu => (stu.StudentJobs!).Any(sj => sj.IsAccepted)));
            CreateMap<StudentDTO, Student>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.Birthdate, o => o.MapFrom(dto => DateTime.Parse(dto.Birthdate, null, DateTimeStyles.AssumeUniversal)));

            CreateMap<Instructor, InstructorDTO>();
            CreateMap<InstructorDTO, Instructor>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.Birthdate, o => o.MapFrom(dto => DateTime.Parse(dto.Birthdate, null, DateTimeStyles.AssumeUniversal)));

            CreateMap<Recruiter, RecruiterDTO>();
            CreateMap<RecruiterDTO, Recruiter>()
                .ForMember(ent => ent.Id, opt => opt.Ignore())
                .ForMember(ent => ent.Birthdate, o => o.MapFrom(dto => DateTime.Parse(dto.Birthdate, null, DateTimeStyles.AssumeUniversal)));

            CreateMap<Job, JobDTO>()
                .ForMember(dto => dto.SkillIds, opt => opt.MapFrom(job => job.JobSkills!.Select(js => js.SkillId)));
            CreateMap<JobDTO, Job>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());
            CreateMap<CreateJobDTO, Job>();

            CreateMap<Skill, SkillDTO>();
            CreateMap<SkillDTO, Skill>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());

            CreateMap<InternGroup, InternGroupDTO>()
                .ForMember(dto => dto.DepartmentTitle, opt => opt.MapFrom(ent => ent.Department.Title))
                .ForMember(dto => dto.InstructorName, opt => opt.MapFrom(ent => ent.Instructor.FullName));
            CreateMap<InternGroupDTO, InternGroup>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());

            CreateMap<Evaluation, EvaluationDTO>();
            CreateMap<EvaluationDTO, Evaluation>()
                .ForMember(ent => ent.Id, opt => opt.Ignore());
        }
    }
}
