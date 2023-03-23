namespace Wave5.UI.Forms
{
    public static class FormResultFactory
    {
        #region [ Public Methods - New ]
        /// <summary>
        /// Creates a new <see cref="FormResult<TAction, TData>"/>.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="state"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormResult<TState, TData> New<TState, TData>(TState state, TData data) {
            return new FormResult<TState, TData> (state, data);
        }

        /// <summary>
        /// Creates a new <see cref="FormResult<TData>"/>.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="state"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormResult<TData> New<TData>(FormResultState state, TData data) {
            return new FormResult<TData>(state, data);
        }
        #endregion

        #region [ Public Methods - New FormResult ]
        /// <summary>
        /// Creates a new <see cref="FormResult<TData>"/> where the <see cref=" FormResultState"/> is <see cref="FormResultState.Added"/>
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormResult<TData> AddedResult<TData>(TData data) {
            return New(FormResultState.Added, data);
        }

        /// <summary>
        /// Creates a new <see cref="FormResult<TData>"/> where the <see cref=" FormResultState"/> is <see cref="FormResultState.Updated"/>
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormResult<TData> UpdatedResult<TData>(TData data) {
            return New(FormResultState.Updated, data);
        }

        /// <summary>
        /// Creates a new <see cref="FormResult<TData>"/> where the <see cref=" FormResultState"/> is <see cref="FormResultState.Deleted"/>
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormResult<TData> DeletedResult<TData>(TData data) {
            return New(FormResultState.Deleted, data);
        }

        /// <summary>
        /// Creates a new <see cref="FormResult<TData>"/> where the <see cref=" FormResultState"/> is <see cref="FormResultState.Canceled"/>
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FormResult<TData> CanceledResult<TData>(TData data) {
            return New(FormResultState.Canceled, data);
        }
        #endregion
    }
}
