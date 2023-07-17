using Blazored.LocalStorage;

namespace InternshipApp.Portal.WebApi;

public class StudentHttpClient : BaseEntityHttpClient
{
    #region [ CTor ]
    public StudentHttpClient(IHttpClientFactory clientFactory, ILocalStorageService localStorageService) : base(clientFactory, localStorageService) { }
    #endregion

    #region [ Methods - Custom ]

    #endregion
}
