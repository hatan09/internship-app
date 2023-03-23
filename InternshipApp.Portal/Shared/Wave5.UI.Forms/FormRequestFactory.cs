namespace Wave5.UI.Forms;

public static class FormRequestFactory
{
    #region [ Public Methods - New ]
    /// <summary>
    /// Creates a new <see cref="FormActionRequest<TAction, TData>"/>
    /// </summary>
    /// <typeparam name="TAction"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="acton"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FormRequest<TAction, TData> New<TAction, TData>(TAction acton, TData data) {
        return new FormRequest<TAction, TData>(acton, data);
    }
    #endregion

    #region [ Public Methods - New FormAction ]
    /// <summary>
    /// Creates a new FormActionRequest<FormAction, TData> where the <see cref=" FormAction"/> is <see cref="FormAction.Add"/>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FormRequest<FormAction, TData> AddRequest<TData>(TData data) {
        return New(FormAction.Add, data);
    }

    /// <summary>
    /// Creates a new FormActionRequest<FormAction, TData> where the <see cref=" FormAction"/> is <see cref="FormAction.Edit"/>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FormRequest<FormAction, TData> EditRequest<TData>(TData data) {
        return New(FormAction.Edit, data);
    }

    /// <summary>
    /// Creates a new FormActionRequest<FormAction, TData> where the <see cref=" FormAction"/> is <see cref="FormAction.Details"/>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FormRequest<FormAction, TData> DetailsRequest<TData>(TData data) {
        return New(FormAction.Details, data);
    }

    /// <summary>
    /// Creates a new FormActionRequest<FormAction, TData> where the <see cref=" FormAction"/> is <see cref="FormAction.Delete"/>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FormRequest<FormAction, TData> DeleteRequest<TData>(TData data) {
        return New(FormAction.Delete, data);
    }

    /// <summary>
    /// Creates a new FormActionRequest<FormAction, TData> where the <see cref=" FormAction"/> is <see cref="FormAction.Cancel"/>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FormRequest<FormAction, TData> CancelRequest<TData>(TData data) {
        return New(FormAction.Cancel, data);
    }
    #endregion
}
