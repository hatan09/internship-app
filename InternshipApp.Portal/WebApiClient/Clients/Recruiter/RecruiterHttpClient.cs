using Blazored.LocalStorage;

namespace InternshipApp.Portal.WebApi;

public class RecruiterHttpClient : BaseEntityHttpClient
{
    #region [ CTor ]
    public RecruiterHttpClient(IHttpClientFactory clientFactory, ILocalStorageService localStorageService) : base(clientFactory, localStorageService) { }
    #endregion

    #region [ Methods - Custom ]

    #endregion
}
