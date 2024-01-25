using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Domain.Services.Abstraction;

public interface IHttpClient
{
    Task<T?> Get<T>(string url, string accessToken);

    Task<T?> Post<T>(string url, List<KeyValuePair<string, string>> body);
}