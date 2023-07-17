using Blazored.LocalStorage;

namespace InternshipApp.Portal.WebApi;

public class InstructorHttpClient : BaseEntityHttpClient
{
    #region [ CTor ]
    public InstructorHttpClient(IHttpClientFactory clientFactory, ILocalStorageService localStorageService) : base(clientFactory, localStorageService) { }
    #endregion

    #region [ Methods - Custom ]

    #endregion
}
