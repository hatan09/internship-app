namespace Wave5.UI.Forms;

public class FormResult<TData> : FormResult<FormResultState, TData>
{
    #region [ CTor ]
    public FormResult() {

    }

    public FormResult(FormResultState state, TData data) : base(state, data) {

    }
    #endregion
}

public class FormResult<TState, TData>
{
    #region [ CTor ]
    public FormResult() {

    }

    public FormResult(TState state, TData data) {
        this.State = state;
        this.Data = data;
    }
    #endregion

    #region [ Properties ]
    public TState State { get; set; }

    public TData Data { get; set; }
    #endregion
}