using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly string _scope = "api://bd20e841-d13e-49bc-98be-6e4c60872fd2/access_as_user";

    public ApiService(IHttpClientFactory clientFactory, ITokenAcquisition tokenAcquisition)
    {
        _httpClient = clientFactory.CreateClient("MyAPI");
        _tokenAcquisition = tokenAcquisition;
    }

    private async Task SetAccessToken()
    {
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _scope });
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // GET - Single object
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        await SetAccessToken();
        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponse<T>(response);
    }

    // GET - List of objects
    public async Task<List<T>?> GetListAsync<T>(string endpoint)
    {
        await SetAccessToken();
        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponse<List<T>>(response);
    }

    // POST (Without expecting a return value)
    public async Task PostAsync<T>(string endpoint, T data)
    {
        await SetAccessToken();
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        await HandleResponse<object>(response);
    }
    
    // POST (Overload with a generic return value)
    // Returns "Tresponse DTO" to frontend, and sends "Trequest DTO" to the API (db)
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        await SetAccessToken();
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        return await HandleResponse<TResponse>(response);
    }

    // PUT
    public async Task PutAsync<T>(string endpoint, T data)
    {
        await SetAccessToken();
        var response = await _httpClient.PutAsJsonAsync(endpoint, data);
        await HandleResponse<object>(response);
    }

    // DELETE
    public async Task DeleteAsync(string endpoint)
    {
        await SetAccessToken();
        var response = await _httpClient.DeleteAsync(endpoint);
        await HandleResponse<object>(response);
    }

    // Error handling for all API calls which checks status codes and returns error messages to the frontend
    private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            // User doesnt have the required roles (Admin/Personale)
            throw new UnauthorizedAccessException("Du har ikke de nødvendige rettigheder til denne handling.");
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // User not logged in or token expired
            throw new HttpRequestException("Log venligst ind igen.");
        }

        if (!response.IsSuccessStatusCode)
        {
            // Catches errors from the API
            var error = await response.Content.ReadAsStringAsync();

            // Sends the error message to the Frontend
            throw new Exception(string.IsNullOrWhiteSpace(error) ? $"Fejl: {response.StatusCode}" : error);
        }
        
        if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0) 
        {
            return default;
        }   

        if (response.StatusCode == HttpStatusCode.NoContent) 
            return default;

        return await response.Content.ReadFromJsonAsync<T>();
    }
}