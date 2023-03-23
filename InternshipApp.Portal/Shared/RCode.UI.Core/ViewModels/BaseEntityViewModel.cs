using InternshipApp.Core.Entities;
using System;

namespace RCode.ViewModels;

public class BaseEntityViewModel<TEntity> : BaseViewModel where TEntity : BaseEntity<int>
{
    #region [ Fields ]
    private int _id;
    #endregion

    #region [ CTor ]
    public BaseEntityViewModel() {

    }

    public BaseEntityViewModel(TEntity entity) {
        this.Id = entity.Id;
    }
    #endregion

    #region [ Properties ]

    public int Id {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }
    #endregion
}