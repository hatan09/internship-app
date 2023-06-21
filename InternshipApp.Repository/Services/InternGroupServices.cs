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
    public async Task AutoAssign(int maxAmount = 0, bool assignToFreeInstructor = true)
    {
        var students = await _studentManager.FindAll(x => x.InternGroup == null).AsTracking().ToListAsync();
        if (students == null || students.Count <= 0)
        {
            return;
        }
        else if(students.Count == 1) {
            var student = students.FirstOrDefault();
            if(student == null)
            {
                return;
            }
            var group = await _internGroupRepository.FindAll()
                .OrderBy(x => x.Students.Count)
                .Take(1)
                .Include(x => x.Students)
                .AsTracking()
                .FirstOrDefaultAsync();

            if(group == null)
            {
                return;
            }

            group.Students.Add(student);
            _internGroupRepository.Update(group);
            await _internGroupRepository.SaveChangesAsync();
        }
        else
        {
            if (assignToFreeInstructor)
            {
                var freeInstructors = await _instructorManager.FindAll(x => x.InternGroup == null).ToListAsync();

                var requiredGroupAmount = Math.Min(freeInstructors.Count, students.Count);
                for(var i = 0; i < requiredGroupAmount; i++)
                {
                    var instructor = freeInstructors.ElementAt(i);
                    if(instructor == null)
                    {
                        break;
                    }

                    var newGroup = new InternGroup()
                    {
                        InstructorId = instructor.Id,
                        Title = $"{instructor.FullName}_Group"
                    };

                    _internGroupRepository.Add(newGroup);
                }
                await _internGroupRepository.SaveChangesAsync();
            }

            var availableGroups = await _internGroupRepository.FindAll().Include(x => x.Students).AsTracking().ToListAsync();
            var sortedAvailableGroups = availableGroups.OrderBy(x => x.Students.Count);

            var amountPerGroup = 0;
            while(students.Count > 0)
            {
                foreach (var group in sortedAvailableGroups)
                {
                    if(amountPerGroup > maxAmount || students.Count <= 0)
                    {
                        break;
                    }

                    if (amountPerGroup <= group.Students.Count)
                    {
                        amountPerGroup++;
                        break;
                    }

                    var student = students.FirstOrDefault();
                    if(student != null) {
                        group.Students.Add(student);
                        group.Students.Remove(student);
                        _internGroupRepository.Update(group);
                    }
                }
            }

            await _internGroupRepository.SaveChangesAsync();
        }
    }

    public async Task AutoCreateAndAssign(int maxAmount = 0)
    {
        var totalNoGroupStudentAmount = await CountNoGroupStudent();
        var totalFreeInstructorAmount = await CountFreeInstructor();

        var amountPerGroup = totalNoGroupStudentAmount / totalFreeInstructorAmount;
        var remain = totalNoGroupStudentAmount % totalFreeInstructorAmount;
        if(maxAmount > 0 && amountPerGroup + (remain > 0 ? 1 : 0) > maxAmount)
        {
            amountPerGroup = maxAmount;
            remain = 0;
        }

        var students = await _studentManager.FindAll(x => x.InternGroup == null)
            .Take(amountPerGroup + ((remain-- > 0)? 1 : 0))
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
                .Take(amountPerGroup + ((remain-- > 0) ? 1 : 0))
                .AsTracking()
                .ToListAsync();
        }
    }

    private Task<int> CountNoGroupStudent()
    {
        return _studentManager.FindAll(x => x.InternGroup == null && x.Stat != Stat.REJECTED).CountAsync();
    }

    private Task<int> CountFreeInstructor()
    {
        return _instructorManager.FindAll(x => x.InternGroup == null).CountAsync();
    }
    #endregion
}
