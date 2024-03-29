﻿using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class InternSettingsExtension
{
    #region [ Public Methods - ListRow ]
    //public static List<InternSettingListRowViewStates> ToListRowList(this List<InternSetting> list)
    //{
    //    var result = new List<InternSettingListRowViewStates>();
    //    list.ForEach(x => result.Add(x.ToListRow()));
    //    return result;
    //}

    //public static InternSettingListRowViewStates ToListRow(this InternSetting entity)
    //{

    //    var viewStates = ToViewStates<InternSettingListRowViewStates>(entity);

    //    return viewStates;
    //}

    //public static InternSetting ToEntity(this InternSettingListRowViewStates viewStates)
    //{
    //    return ToEntity<InternSetting>(viewStates);
    //}
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static InternSettingsFormViewStates ToFormViewStates(this InternSettings entity)
    {

        var viewmodel = ToViewStates<InternSettingsFormViewStates>(entity);

        return viewmodel;
    }

    public static InternSettings ToEntity(this InternSettingsFormViewStates viewStates)
    {
        return ToEntity<InternSettings>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static InternSettingsDetailsViewStates ToDetailsViewStates(this InternSettings entity)
    {

        var viewmodel = ToViewStates<InternSettingsDetailsViewStates>(entity);

        return viewmodel;
    }

    public static InternSettings ToEntity(this InternSettingsDetailsViewStates viewStates)
    {
        return ToEntity<InternSettings>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this InternSettings entity)

        where TBaseViewModel : BaseInternSettingsViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,
            Title = entity.Title,
            Start = entity.StartTime,
            CloseReg = entity.CloseRegistrationTime,
            JobDeadline = entity.JobDeadline,
            Summary = entity.SummaryTime,
            End = entity.EndTime,
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseInternSettingsViewStates viewModel)
        where TEntity : InternSettings, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            StartTime = viewModel.Start.GetDayStart(),
            CloseRegistrationTime = viewModel.CloseReg.GetDayEnd(),
            JobDeadline = viewModel.JobDeadline.GetDayStart(),
            SummaryTime = viewModel.Summary.GetDayEnd(),
            EndTime = viewModel.End.GetDayEnd(),
        };
    }
    #endregion
}