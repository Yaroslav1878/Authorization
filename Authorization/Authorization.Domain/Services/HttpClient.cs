using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authorization.Domain.Services.Abstraction;
using Newtonsoft.Json;

namespace Authorization.Domain.Services;

public class HttpClient : IHttpClient
{
    private const string Bearer = "Bearer";
    private const string FromContentType = "application/x-www-form-urlencoded";

    public async Task<T?> Get<T>(string url, string accessToken)
    {
        using var client = new System.Net.Http.HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, accessToken);

#pragma warning disable CA2234
        var response = await client.GetAsync(string.Empty).ConfigureAwait(false);
#pragma warning restore CA2234

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(responseContent);

            return deserializedObject;
        }

        return default;
    }

    public async Task<T?> Post<T>(string url, List<KeyValuePair<string, string>> body)
    {
        using var httpClient = new System.Net.Http.HttpClient();

        httpClient.DefaultRequestHeaders.Clear();
        string contentType = FromContentType;
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

#pragma warning disable CA2000
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(body) };
#pragma warning restore CA2000
        HttpResponseMessage response = await httpClient.SendAsync(tokenRequest);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(responseContent);

            return deserializedObject;
        }

        return default;
    }
}
