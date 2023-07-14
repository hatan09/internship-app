using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.WebApi;

public abstract class BaseEntityHttpClient : BaseHttpClient
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

    #region [ Public Methods - Add/Update Users ]
    public async Task AddUserAsync<TUser>(TUser user) where TUser : User
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TUser)}/create";
            await base.PostAsJsonAsync(url, user);
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public async Task UpdateUserAsync<TUser>(TUser user) where TUser : User
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TUser)}/update";
            await base.PutAsJsonAsync(url, user);
        }
        catch (Exception) { throw; }
    }
    #endregion

    #region [ Methods - Get Single User ]
    public Task<TUser> GetUserAsync<TUser>(string id) where TUser : User
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TUser)}/getById/{id}";
            return base.GetAsync<TUser>(url);
        }
        catch (Exception)
        {
            throw;

        }
    }
    #endregion

    #region [ Methods - Get List ]
    public Task<List<TUser>> GetAllUserAsync<TUser>() where TUser : User
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TUser)}/getAll";
            return base.GetListAsync<TUser>(url);
        }
        catch (Exception)
        {
            throw;

        }
    }

    public Task<List<TUser>> GetUserBatchAsync<TUser>(List<string> entityIds) where TUser : User
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TUser)}/batch";
            return base.PostAsJsonAsync<List<TUser>, List<string>>(url, entityIds);
        }
        catch (Exception)
        {
            throw;

        }
    }
    #endregion

    #region [ Public Methods - Add/ Update/ Delete BaseEntities ]
    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity<int>
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TEntity)}/create";
            await base.PostAsJsonAsync(url, entity);
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity<int>
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TEntity)}/update";
            await base.PutAsJsonAsync(url, entity);
        }
        catch (Exception ex) { throw; }
    }

    public async Task DeleteAsync<TEntity>(string id) where TEntity : BaseEntity<int>
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TEntity)}/delete/{id}";
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

    #region [ Methods - Get Single ]
    public Task<TEntity> GetAsync<TEntity>(string id) where TEntity : BaseEntity<int>
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TEntity)}/getById/{id}";
            return base.GetAsync<TEntity>(url);
        }
        catch (Exception)
        {
            throw;

        }
    }
    #endregion

    #region [ Methods - Get List ]
    public Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : BaseEntity<int>
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TEntity)}/getAll";
            return base.GetListAsync<TEntity>(url);
        }
        catch (Exception ex)
        {
            throw;

        }
    }

    public Task<List<TEntity>> GetBatchAsync<TEntity>(List<string> entityIds) where TEntity : BaseEntity<int>
    {
        try
        {
            var url = $"{base._baseApiUrl}{nameof(TEntity)}/batch";
            return base.PostAsJsonAsync<List<TEntity>, List<string>>(url, entityIds);
        }
        catch (Exception ex)
        {
            throw;

        }
    }
    #endregion
}
