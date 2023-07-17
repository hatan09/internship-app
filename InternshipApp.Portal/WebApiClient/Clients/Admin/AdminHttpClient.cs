using Blazored.LocalStorage;

namespace InternshipApp.Portal.WebApi;

public class AdminHttpClient : BaseEntityHttpClient
{
    #region [ CTor ]
    public AdminHttpClient(IHttpClientFactory clientFactory, ILocalStorageService localStorageService) : base(clientFactory, localStorageService) { }
    #endregion

    #region [ Methods - Custom ]

    #endregion
}
