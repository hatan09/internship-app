using System.Text.Json;

namespace InternshipApp.Portal.WebApi;

public static class HttpClientExtensions
{
    #region [ Public Methods - HttpClient ]
    public static async Task<TResult> GetAsync<TResult>(this HttpClient httpClient, string url, bool ensureSuccessStatusCode = true)
    {
        var response = await httpClient.GetAsync(url);
        return await response.GetResponseAsync<TResult>(ensureSuccessStatusCode);
    }

    public static async Task<TResult> PostAsJsonAsync<TResult, TPayLoad>(this HttpClient httpClient, string url, TPayLoad payload, bool ensureSuccessStatusCode = true)
    {
        var response = await httpClient.PostAsJsonAsync(url, payload);
        return await response.GetResponseAsync<TResult>(ensureSuccessStatusCode);
    }

    public static async Task<TResult> PutAsJsonAsync<TResult, TPayLoad>(this HttpClient httpClient, string url, TPayLoad payload, bool ensureSuccessStatusCode = true)
    {
        var response = await httpClient.PutAsJsonAsync(url, payload);
        return await response.GetResponseAsync<TResult>(ensureSuccessStatusCode);
    }
    #endregion

    #region [ Public Methods - HttpResponseMessage ]
    public static async Task<TResult> GetResponseAsync<TResult>(this HttpResponseMessage response, bool ensureSuccessStatusCode = true)
    {

        if (ensureSuccessStatusCode)
        {
            response.EnsureSuccessStatusCode();
        }

        var stream = await response.Content.ReadAsStreamAsync();

        var options = new JsonSerializerOptions();
        options.PropertyNameCaseInsensitive = true;

        return JsonSerializer.Deserialize<TResult>(stream, options);
    }
    #endregion
}