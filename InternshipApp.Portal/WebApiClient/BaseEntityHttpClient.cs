using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.WebApi;

public abstract class BaseEntityHttpClient<TEntity> : BaseHttpClient
    where TEntity : BaseEntity<int>
{
    #region [ Fields ]
    protected string EntityApiPath;
    #endregion

    #region [ CTor ]
    public BaseEntityHttpClient(
        IHttpClientFactory clientFactory)
        : base(clientFactory)
    {

    }
    #endregion

    #region [ Public Methods - Add/ Update/ Delete ]
    public async Task AddAsync(TEntity entity)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}";
            await base.PostAsJsonAsync(url, entity);
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}";
            await base.PostAsJsonAsync(url, entity);
        }
        catch (Exception ex) { throw; }
    }

    public async Task DeleteAsync(string id)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/{id}";
            var client = await this.CreateClientAsync();
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;

        }
    }
    #endregion

    #region [ Public Methods - Activate ]
    public async Task ActivateAsync(string id)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/activate/{id}";
            var client = await this.CreateClientAsync();
            var response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public async Task DeactivateAsync(string id)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/deactivate/{id}";
            var client = await this.CreateClientAsync();
            var response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;

        }
    }
    #endregion

    #region [ Methods - Get Single ]
    public Task<TEntity> GetAsync(string id)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/{id}";
            return base.GetAsync<TEntity>(url);
        }
        catch (Exception)
        {
            throw;

        }
    }
    #endregion

    #region [ Methods - Get List ]
    public Task<List<TEntity>> GetByFilterAsync<TFilterOptions>(TFilterOptions options)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/filter";
            return base.PostAsJsonAsync<List<TEntity>, TFilterOptions>(url, options);
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/all";
            return base.GetListAsync<TEntity>(url);
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public Task<List<TEntity>> GetActiveAsync()
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/active";
            return base.GetListAsync<TEntity>(url);
        }
        catch (Exception ex) { throw; }
    }

    public Task<List<TEntity>> GetInActiveAsync()
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/inactive";
            return base.GetListAsync<TEntity>(url);
        }
        catch (Exception ex) { throw; }
    }

    public Task<List<TEntity>> GetBatchAsync(List<string> entityIds)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/batch";
            return base.PostAsJsonAsync<List<TEntity>, List<string>>(url, entityIds);
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public Task<List<TEntity>> GetChangesAsync(DateTime date)
    {
        try
        {
            var url = $"{base._baseApiUrl}{this.EntityApiPath}/changes";
            return base.PostAsJsonAsync<List<TEntity>, DateTime>(url, date);
        }
        catch (Exception ex) { throw; }
    }
    #endregion
}
