using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class JobInfoView
{
    #region [ Fields ]

    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string JobId { get; set; }
    #endregion

    #region [ Properties ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Job> LinkedServiceFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected JobDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentLinkedServiceId = this.JobId;
        var parameterLinkedServiceId = parameters.GetValueOrDefault<string>(nameof(this.JobId));

        await base.SetParametersAsync(parameters);

        if (currentLinkedServiceId != parameterLinkedServiceId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async void OnApply()
    {
        var student = Students.FindAll().FirstOrDefaultAsync();

        var studentJob = new StudentJob()
        {
            StudentId = student.Id.ToString(),
            JobId = int.Parse(JobId)
        };

        await OnUpdateApplicationAsync(studentJob);
    }

    private async Task OnUpdateApplicationAsync(StudentJob studentJob)
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).Include(x => x.StudentJobs).FirstOrDefaultAsync();
        job?.StudentJobs.Add(studentJob);
        Jobs.Update(job);
        await Jobs.SaveChangesAsync();
    }

    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.JobId, nameof(this.JobId));

        try
        {
            var item = await this.Jobs.FindByIdAsync(int.Parse(this.JobId));

            if (item is null)
            {
                this.States = null;
                return;
            }

            this.States = item.ToDetailsViewStates();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.StateHasChanged();
        }
    }
    #endregion
}
