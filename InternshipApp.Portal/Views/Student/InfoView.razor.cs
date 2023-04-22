using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using RCode;
using Wave5.UI;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class InfoView
{
    #region [ Fields ]

    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string StudentId { get; set; }
    #endregion

    #region [ Properties ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Student> ApplicationFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected StudentDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentApplicationId = this.StudentId;
        var parameterApplicationId = parameters.GetValueOrDefault<string>(nameof(this.StudentId));

        await base.SetParametersAsync(parameters);

        if (currentApplicationId != parameterApplicationId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.StudentId, nameof(this.StudentId));

        try
        {
            var item = await this.Students.FindByIdAsync(this.StudentId);

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

    public string GetOrderFromInt(int order)
    {
        switch(order)
        {
            case 1:
                {
                    return "First";
                }
            case 2:
                {
                    return "Second";
                }
            case 3:
                {
                    return "Third";
                }
            case 4:
                {
                    return "Fourth";
                }
            case 5:
                {
                    return "Fifth";
                }
            case 6:
                {
                    return "Sixth";
                }
            default:
                {
                    return "";
                }
        }
    }

    public async void OnChat()
    {

    }
    #endregion
}
