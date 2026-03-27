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

    // POST
    public async Task PostAsync<T>(string endpoint, T data)
    {
        await SetAccessToken();
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        await HandleResponse<object>(response);
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

    // Central håndtering af adgang og fejl
    private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            // Brugeren er logget ind, men har ikke den rigtige rolle (Admin/Personale)
            throw new UnauthorizedAccessException("Du har ikke de nødvendige rettigheder til denne handling.");
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Brugeren er slet ikke logget ind eller token er udløbet
            throw new HttpRequestException("Log venligst ind igen.");
        }

        response.EnsureSuccessStatusCode();

        if (response.StatusCode == HttpStatusCode.NoContent) return default;

        return await response.Content.ReadFromJsonAsync<T>();
    }
}