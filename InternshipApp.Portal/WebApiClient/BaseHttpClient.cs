using System.Text.Json;
using Blazored.LocalStorage;

namespace InternshipApp.Portal.WebApi;

public abstract class BaseHttpClient
{
    #region [ Fields ]
    protected readonly IHttpClientFactory HttpClientFactory;
    protected readonly ILocalStorageService _localStorage;
    protected readonly string _baseApiUrl = "https://app-internship-app.azurewebsites.net/api/";
    #endregion

    #region [ CTor ]
    public BaseHttpClient(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
    {
        this.HttpClientFactory = clientFactory;
        this._localStorage = localStorage;
    }
    #endregion

    #region [ Protected Methods - Get ]
    protected async Task<List<TResult>> GetListAsync<TResult>(string url)
    {
        try
        {
            var client = await this.CreateClientAsync();
            return await client.GetAsync<List<TResult>>(url);

        }
        catch (Exception ex)
        {
            throw;

        }
    }

    protected async Task<TResult> GetAsync<TResult>(string url)
    {
        try
        {
            var client = await this.CreateClientAsync();
            return await client.GetAsync<TResult>(url);

        }
        catch (Exception ex)
        {
            throw;

        }
    }
    #endregion

    #region [ Protected Methods - Post ]
    /// <summary>
    /// Post a string to the server and ensures a success status code.
    /// This method disregards response content.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="message"></param>
    /// <param name="parentTrace"></param>
    /// <returns></returns>
    protected async Task PostStringAsync(string url, string message)
    {
        try
        {
            var content = new StringContent(message);
            var client = await this.CreateClientAsync();
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

        }
        catch (Exception ex)
        {
            throw;

        }
    }

    protected async Task<TResult> PostStringAsync<TResult>(string url, string message)
    {
        try
        {
            var content = new StringContent(message);
            var client = await this.CreateClientAsync();
            var response = await client.PostAsync(url, content);
            return await response.GetResponseAsync<TResult>(true);
        }
        catch (Exception ex) { throw; }
    }

    protected async Task PostAsJsonAsync<TPayLoad>(string url, TPayLoad payload)
    {
        try
        {
            var client = await this.CreateClientAsync();
            var response = await client.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();

        }
        catch (Exception ex) { throw; }
    }

    protected async Task<TResult> PostAsJsonAsync<TResult, TPayLoad>(string url, TPayLoad payload)
    {
        try
        {
            var client = await this.CreateClientAsync();
            return await client.PostAsJsonAsync<TResult, TPayLoad>(url, payload);

        }
        catch (Exception ex)
        {
            throw;

        }
    }
    #endregion

    #region [ Protected Methods - Put ]
    protected async Task PutAsJsonAsync<TPayLoad>(string url, TPayLoad payload)
    {
        try
        {
            var client = await this.CreateClientAsync();
            var response = await client.PutAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected async Task<TResult> PutAsJsonAsync<TResult, TPayLoad>(string url, TPayLoad payload)
    {
        try
        {
            var client = await this.CreateClientAsync();
            return await client.PutAsJsonAsync<TResult, TPayLoad>(url, payload);

        }
        catch (Exception ex)
        {
            throw;

        }
    }
    #endregion

    #region [ Protected Methods - Helpers ]
    protected TResult Deserialize<TResult>(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            return default;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var result = JsonSerializer.Deserialize<TResult>(response, options);
        return result;
    }
    #endregion

    #region [ Public Methods - Client ]
    protected async Task<HttpClient> CreateClientAsync()
    {
        var accessToken = await _localStorage.GetItemAsStringAsync("accessToken");
        var client = this.HttpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_baseApiUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("authentication", $"Bearer {accessToken}");
        return client;
    }
    #endregion
}