﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;
using Wave5.UI;
using InternshipApp.Repository;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class StudentInfoFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public RoleManager<Role> Roles { get; private set; }

    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, Student> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<Student>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected StudentFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, Student>>(nameof(this.FormRequest));
        var currentFormRequest = this.FormRequest;

        await base.SetParametersAsync(parameters);

        if (newFormRequest != null && newFormRequest != currentFormRequest)
        {
            if (newFormRequest.Action == FormAction.Cancel)
            {
                await this.CanceledAsync();
            }
            else
            {
                await this.LoadDataAsync();
            }
            return;
        }
    }
    #endregion

    #region [ Event Handlers - Form ]
    private void OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        if (e.FieldIdentifier.FieldName.Equals(nameof(States.StudentId)))
        {
            if(string.IsNullOrWhiteSpace(States.Username) && !string.IsNullOrWhiteSpace(States.StudentId))
            {
                States.Username = States.StudentId;
            }
        }
        this.StateHasChanged();
    }

    private async void OnFormSubmit(object args)
    {
        switch (this.FormRequest.Action)
        {
            case FormAction.Add:
                await AddedAsync();
                break;
            case FormAction.Edit:
                if (!this.Context.IsModified())
                {
                    await this.CanceledAsync();
                }
                else
                {
                    await this.UpdatedAsync();
                }
                break;
        }
    }

    private async void OnCancel(object args)
    {
        await this.CanceledAsync();
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            var student = await Students.FindAll(x => x.Id == FormRequest.Data.Id).AsNoTracking().FirstOrDefaultAsync();
            if(student != null)
            {
                States = student.ToFormViewStates();
            }

            switch (this.FormRequest.Action)
            {
                case FormAction.Add:
                    {
                        States = FormRequest.Data.ToFormViewStates();
                        break;
                    }
                case FormAction.Edit:
                    {
                        // Edit action logic
                        break;
                    }
                case FormAction.Delete:
                case FormAction.Details:
                default:
                    break;
            }

            this.Context = new EditContext(this.States);
            this.Context.OnFieldChanged += this.OnFieldChanged;
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

    #region [ Private Methods - CRUD ]
    private async Task AddedAsync()
    {
        try
        {
            States.Status = Stat.WAITING.ToString();
            var student = States.ToEntity();
            //student.Id = Guid.NewGuid().ToString();
            var result = await Students.CreateAsync(student, States.Password);
            if (!result.Succeeded)
            {
                return;
            }

            result = await Students.AddToRoleAsync(student, "student");
            if (!result.Succeeded)
            {
                return;
            }

            await this.InvokeFormResultCallbackAsync(FormResultState.Updated);
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    private async Task UpdatedAsync()
    {
        try
        {
            var student = await Students.FindAll(x => x.Id == FormRequest.Data.Id).AsTracking().FirstOrDefaultAsync();
            
            if (student != null)
            {
                student.Credit = States.Credits;
                student.GPA = States.Gpa;
                student.Bio = States.Bio;
                student.Email = States.Email;
                student.Year = States.Year;
                student.GitProfileUrl = States.GitUrl;
                student.CVUrl = States.CVUrl;
                student.ImgUrl = States.ImgUrl;
            }
            await this.Students.UpdateAsync(student);

            await this.InvokeFormResultCallbackAsync(FormResultState.Updated);
        }
        catch (Exception ex)
        {
        }
        finally
        { 
        }
    }

    private async Task CanceledAsync()
    {
        await this.InvokeFormResultCallbackAsync(FormResultState.Canceled);
    }

    private void Reset()
    {
        this.States = new Student().ToFormViewStates();
        this.Context = new EditContext(this.States);

        this.StateHasChanged();
    }

    private void OnGpaChanged(ChangeEventArgs args)
    {
        var value = args.Value.ToString();
        var isDouble = double.TryParse(value, out var doubleValue);
        if(isDouble)
        {
            this.States.Gpa = doubleValue;
        }
        StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Callbacks ]
    private async Task InvokeFormResultCallbackAsync(FormResultState state)
    {
        var result = FormResultFactory.New(state, this.FormRequest.Data);
        await this.FormResultCallback.InvokeAsync(result);

        this.Reset();
    }
    #endregion
}
