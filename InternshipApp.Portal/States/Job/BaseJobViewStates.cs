﻿using InternshipApp.Core.Entities;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseJobViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private int _credit;
    private double _gpa;
    private int _slots;
    private int _remaining;
    private string _title;
    private int _year;
    private string _description;
    private string _companyName;
    private double _matching;
    private List<JobSkill> _jobSkills;
    private List<Skill> _skills;
    private bool _isAllowedApply;
    private string _address;
    private bool _isAccepted;
    #endregion

    #region [ CTor ]
    public BaseJobViewStates()
    {
        _jobSkills = new();
        _skills = new();
    }
    #endregion

    #region [ Properties ]
    public int Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public int Credit
    {
        get { return this._credit; }
        set { this.SetProperty(ref this._credit, value); }
    }

    public double Gpa
    {
        get { return this._gpa; }
        set { this.SetProperty(ref this._gpa, value); }
    }

    public int Remaining
    {
        get { return this._remaining; }
        set { this.SetProperty(ref this._remaining, value); }
    }

    public int Slots
    {
        get { return this._slots; }
        set { this.SetProperty(ref this._slots, value); }
    }

    public int Year
    {
        get { return this._year; }
        set { this.SetProperty(ref this._year, value); }
    }

    public string Title
    {
        get { return this._title; }
        set { this.SetProperty(ref this._title, value); }
    }

    public string Description
    {
        get { return this._description; }
        set { this.SetProperty(ref this._description, value); }
    }

    public string CompanyName
    {
        get { return this._companyName; }
        set { this.SetProperty(ref this._companyName, value); }
    }

    public double Matching
    {
        get { return this._matching; }
        set { this.SetProperty(ref this._matching, value); }
    }

    public List<JobSkill> JobSkills
    {
        get { return this._jobSkills; }
        set { this.SetProperty(ref this._jobSkills, value); }
    }

    public List<Skill> Skills
    {
        get { return this._skills; }
        set { this.SetProperty(ref this._skills, value); }
    }

    public bool IsAllowedApply
    {
        get { return this._isAllowedApply; }
        set { this.SetProperty(ref this._isAllowedApply, value); }
    }

    public string Address
    {
        get { return this._address; }
        set { this.SetProperty(ref this._address, value); }
    }

    public bool IsAccepted
    {
        get { return this._isAccepted; }
        set { this.SetProperty(ref this._isAccepted, value); }
    }
    #endregion
}
