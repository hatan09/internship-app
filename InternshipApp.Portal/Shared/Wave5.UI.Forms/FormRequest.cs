using InternshipApp.Core.Entities;
using RCode;

namespace Wave5.UI.Forms;

public class FormRequest : FormRequest<FormAction, BaseEntity<int>>
{
    #region [ CTor ]
    public FormRequest() {

    }

    public FormRequest(FormAction action, BaseEntity<int> entity) : base(action, entity) { 
    
    }
    #endregion
}

public class FormRequest<TAction, TData>
{
    #region [ CTor ]
    public FormRequest() {

    }

    public FormRequest(TAction action, TData data) {
        this.Action = action;
        this.Data = data;
    }
    #endregion

    #region [ Properties ]
    public TAction Action { get; set; }

    public TData Data { get; set; }
    #endregion
}