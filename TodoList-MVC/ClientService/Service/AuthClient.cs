using Newtonsoft.Json;
using RestSharp;
using TodoList_MVC.ClientService.Interface;
using TodoList_MVC.Models;

namespace TodoList_MVC.ClientService.Service;

public class AuthClient : IAuthClient
{
    private readonly string url = "https://localhost:7083";

    public RestClient _client;

    public AuthClient()
    {
        _client = new RestClient(url);
    }

    public async Task<string> LoginAsync(LoginModel loginModel)
    {
        var request = new RestRequest("/auth/login", Method.Post).AddJsonBody(loginModel);

        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            return tokenResponse.Token;
        }

        throw new Exception("Login failed: " + response.Content);
    }
}
