using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class SettingView
{
    #region [ Properties - Inject ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IInternSettingsRepository Settings { get; set; }

    [Inject]
    public StudentManager Students { get; set; }
    #endregion

    #region [ Properties - States ]
    public InternSettingsDetailsViewStates States { get; set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadDataAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        var settings = await Settings.GetCurrentSemester();
        if(settings == null)
        {
            var title = "Internship ";
            var today = DateTime.Now.Date;
            if (new DateTime(today.Year, 8, 1) <= today && new DateTime(today.Year, 9, 30) >= today)
            {
                title += $"Semester 1 - academic year {today.Year} - {today.Year + 1}";
            }
            else if (new DateTime(today.Year - 1, 12, 1) <= today && new DateTime(today.Year, 1, 31) >= today)
            {
                title += $"Semester 2 - academic year {today.Year - 1} - {today.Year}";
            }
            else if (new DateTime(today.Year, 5, 1) <= today && new DateTime(today.Year, 6, 30) >= today)
            {
                title += $"Semester 3 - academic year {today.Year - 1} - {today.Year}";
            }
            settings = new InternSettings()
            {
                Title = title,
                StartTime = DateTime.Now,
                CloseRegistrationTime = DateTime.Now,
                JobDeadline = DateTime.Now,
                SummaryTime = DateTime.Now,
                EndTime = DateTime.Now,
            };
        }

        States = settings.ToDetailsViewStates();
        StateHasChanged();
    }

    private async void OnSave()
    {
        var setting = States.ToEntity();
        var settingFromDB = await Settings.FindAll(x => x.Id == setting.Id).AsTracking().FirstOrDefaultAsync();
        if (settingFromDB != null)
        {
            settingFromDB.StartTime = setting.StartTime;
            settingFromDB.CloseRegistrationTime = setting.CloseRegistrationTime;
            settingFromDB.JobDeadline = setting.JobDeadline;
            settingFromDB.SummaryTime = setting.SummaryTime;
            settingFromDB.EndTime = setting.EndTime;

            Settings.Update(settingFromDB);
            await Settings.SaveChangesAsync();
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
        else
        {
            Settings.Add(setting);
            await Settings.SaveChangesAsync();

            var students = await Students.FindAll(x => x.Stat == Stat.REJECTED).AsTracking().ToListAsync();
            students.ForEach(async x =>
            {
                x.Stat = Stat.WAITING;
                await Students.UpdateAsync(x);
            });

            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
    #endregion
}
