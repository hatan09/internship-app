using InternshipApp.Core.Entities;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseCompanyViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private string _title;
    private string _address;
    private string _companyWebsite;
    private string _description;
    private string _imgUrl;
    private string _companyType;
    #endregion

    #region [ CTor ]
    public BaseCompanyViewStates()
    {

    }
    #endregion

    #region [ Properties ]
    public int Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public string Title
    {
        get { return this._title; }
        set { this.SetProperty(ref this._title, value); }
    }

    public string Address
    {
        get { return this._address; }
        set { this.SetProperty(ref this._address, value); }
    }

    public string CompanyWebsite
    {
        get { return this._companyWebsite; }
        set { this.SetProperty(ref this._companyWebsite, value); }
    }

    public string Description
    {
        get { return this._description; }
        set { this.SetProperty(ref this._description, value); }
    }

    public string ImgUrl
    {
        get { return this._imgUrl; }
        set { this.SetProperty(ref this._imgUrl, value); }
    }

    public string CompanyType
    {
        get { return this._companyType; }
        set { this.SetProperty(ref this._companyType, value); }
    }
    #endregion
}
