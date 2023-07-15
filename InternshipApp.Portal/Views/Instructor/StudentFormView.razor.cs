using Blazored.LocalStorage;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class StudentFormView
{
    #region [ Properties ]
    [Inject]
    public ILocalStorageService LocalStorage { get; private set; }

    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public InstructorManager Instructors { get; set; }

    [Inject]
    public RecruiterManager Recruiters { get; private set; }
    #endregion

    #region [ Properties ]
    //[Parameter]

    public bool IsStudentViewing { get; set; }
    public bool IsTeacherViewing { get; set; }
    public bool IsRecruiterViewing { get; set; }

    public StudentForm StudentForm { get; set; } = new();

    public string Options1 { get; set; }
    #endregion

    #region [ Methods - Override ]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var role = await LocalStorage.GetItemAsStringAsync("role");
            switch (role)
            {
                case "STUDENT":
                    IsStudentViewing = true;
                    break;
                case "INSTRUCTOR":
                    IsTeacherViewing = true;
                    break;
                case "RECRUITER":
                    IsRecruiterViewing = true;
                    break;
                default:
                    break;
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion
}
