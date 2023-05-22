using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Repository;

public class InternGroupServices : IInternGroupServices
{
    #region [ Fields ]
    StudentManager _studentManager;
    InstructorManager _instructorManager;
    IInternGroupRepository _internGroupRepository;
    #endregion

    #region [ CTor ]
    public InternGroupServices(StudentManager studentManager, InstructorManager instructorManager, IInternGroupRepository internGroupRepository)
    {
        _studentManager = studentManager;
        _instructorManager = instructorManager;
        _internGroupRepository = internGroupRepository;
    }
    #endregion

    #region [ Methods - Assign ]
    public Task AutoAssign(bool isDividedEqually = true)
    {
        throw new NotImplementedException();
    }

    public async Task AutoCreateAndAssign(int maxAmount = 0)
    {
        var totalStudentAmount = await CountStudent();
        var totalInstructorAmount = await CountInstructor();
        var amountPerGroup = totalStudentAmount / totalInstructorAmount;
        var remain = totalStudentAmount % totalInstructorAmount;
        if(maxAmount > 0 && amountPerGroup + 1 > maxAmount)
        {
            amountPerGroup = maxAmount;
            remain = 0;
        }

        var students = await _studentManager.FindAll(x => x.InternGroup == null)
            .Take(amountPerGroup + ((amountPerGroup < maxAmount && remain-- > 0)? 1 : 0))
            .AsTracking()
            .ToListAsync();

        while(students.Any())
        {
            var instructor = await _instructorManager.FindAll(x => x.InternGroup == null).FirstOrDefaultAsync();
            if(instructor == null)
            {
                break;
            }

            var newGroup = new InternGroup() { 
                InstructorId = instructor.Id,
                Title = $"{instructor.FullName}_Group"
            };

            _internGroupRepository.Add(newGroup);
            await _internGroupRepository.SaveChangesAsync();

            //students.ForEach(x =>
            //{
            //    x.InternGroup = newGroup;
            //});
            
            newGroup = await _internGroupRepository.FindAll(x => x.InstructorId == instructor.Id).Include(x => x.Students).AsTracking().FirstOrDefaultAsync();
            if(newGroup == null)
            {
                break;
            }
            
            foreach(var student in students)
            {
                newGroup.Students.Add(student);
            }
            _internGroupRepository.Update(newGroup);
            await _internGroupRepository.SaveChangesAsync();

            students = await _studentManager.FindAll(x => x.InternGroup == null)
                .Take(amountPerGroup + ((amountPerGroup < maxAmount && remain-- > 0) ? 1 : 0))
                .AsTracking()
                .ToListAsync();
        }
    }

    private Task<int> CountStudent()
    {
        return _studentManager.FindAll(x => x.InternGroup == null).CountAsync();
    }

    private Task<int> CountInstructor()
    {
        return _instructorManager.FindAll(x => x.InternGroup == null).CountAsync();
    }
    #endregion
}
