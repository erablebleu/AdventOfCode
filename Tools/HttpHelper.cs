using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode.Tools;

public static class HttpHelper
{
    public static string DownloadString(string cookieSession, string url)
    {
        Uri baseAddress = new("https://adventofcode.com/");
        using HttpClientHandler handler = new() { UseCookies = false };
        using HttpClient client = new(handler) { BaseAddress = baseAddress };
        HttpRequestMessage message = new(HttpMethod.Post, url);
        message.Headers.Add("Cookie", $"session={cookieSession}");
        HttpResponseMessage response = client.Send(message);
        Task<string> result = response.Content.ReadAsStringAsync();
        result.Wait();
        return result.Result;
    }
}