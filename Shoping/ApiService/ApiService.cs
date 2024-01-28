using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public class ApiService : IApiService
    {
        public async Task<TEntity> Post<TEntity>(string url, Dictionary<string, object> body) where TEntity : class
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                if (response != null && response.IsSuccessStatusCode) {
                    var sResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TEntity>(sResponse);
                }
                return null;
            }
            return null;
        }
    }
}
