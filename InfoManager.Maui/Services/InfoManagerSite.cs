using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using InfoManager.Maui.Services.ServerTypes;
using InfoManager.Shared.Requests;

using OneOf;
using OneOf.Types;

namespace InfoManager.Maui.Services;

public class InfoManagerServices : IDisposable
{
    public const string authCookieAddress = ".AspNetCore.Cookies=";
    const string serverUri =
#if DEBUG
        "https://localhost:7178";
        //"https://localhost:5000";
#else
        "";
#endif
    private HttpClient _httpClient;
    private bool disposedValue;

    public string AuthCookie { get; set; }

    const string serverUserApiUrl = $"{serverUri}/api/user";
    public InfoManagerServices() 
    {
        _httpClient = HttpClientFactory.Create(new CookieAuthHandler(() => AuthCookie));
    }
    public byte[] AuthCode { get; set; }
    public async Task<OneOf<Success,NotFound,Error>> LoginAsync(LoginRequest request,CancellationToken cancellationToken = default)
    {
        HttpRequestMessage requestMessage = new HttpRequestMessage();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{serverUserApiUrl}/login");
        requestMessage.Content = GetFormContentFromObject(request);
        try
        {
            using var result = await _httpClient.SendAsync(requestMessage, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string str = result.Headers.GetValues("Set-Cookie").FirstOrDefault(x => x.StartsWith(authCookieAddress));
                if (string.IsNullOrWhiteSpace(str) == false)
                {
                    int start = str.IndexOf('=');
                    int end = str.IndexOf(';', start);
                    AuthCookie = str[(start + 1)..end];
                    return new Success();
                }

            }
        }
        catch (HttpRequestException ex)
        {
            return new Error();
        }
        return new NotFound();
    }
    public async Task<OneOf<Success,Error>> SignUpAsync(SignUpRequest registerRequest,CancellationToken cancellationToken = default)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
        httpRequestMessage.Method = HttpMethod.Post;
        httpRequestMessage.RequestUri = new Uri($"{serverUserApiUrl}/SignUp");
        httpRequestMessage.Content = GetFormContentFromObject(registerRequest);
        try
        {
            using var result = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new Success();
            }
            else if(result.StatusCode == System.Net.HttpStatusCode.Conflict)
            {

            }
            return new Error();
        }
        catch (HttpRequestException)
        {
            return new Error();
        }
    }
    public async Task<OneOf<Success<CurrentUserInfo>,Error>> GetCurrentUserInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _httpClient.GetAsync($"{serverUserApiUrl}/getme", cancellationToken);
            return new Success<CurrentUserInfo>(await result.Content.ReadFromJsonAsync<CurrentUserInfo>(cancellationToken: cancellationToken));
        }
        catch (HttpRequestException ex)
        {
            return new Error();
        }
    }
    public static FormUrlEncodedContent GetFormContentFromObject<T>(T obj)
    {
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.CanRead && x.GetCustomAttribute<JsonIgnoreAttribute>() is null)
            .ToArray();
        Dictionary<string,string> contentDict = new Dictionary<string,string>();
        foreach (var prop in props)
        {
            contentDict.Add(prop.Name,prop.GetValue(obj)?.ToString() ?? "");
        }
        return new FormUrlEncodedContent(contentDict);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~InfoManagerServices()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
class CookieAuthHandler : DelegatingHandler
{
    private readonly Func<string> cookie;

    public CookieAuthHandler(Func<string> cookie)
    {
        this.cookie = cookie;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string c = cookie();
        if (string.IsNullOrWhiteSpace(c) == false)
        {
            request.Headers.Add("Cookie", InfoManagerServices.authCookieAddress + cookie + ";");
        }
        return base.SendAsync(request, cancellationToken);
    }
}
