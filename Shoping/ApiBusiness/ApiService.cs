using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.ApiBusiness
{
    public class ApiService : IApiService
    {
        private readonly IConfiguration IConfiguration;
        public string UserUrl { get; private set; }

        public ApiService()
        {
            IConfiguration = App.iConfiguration;
            UserUrl = IConfiguration.GetSection("Url").GetSection("User").Value;
        }
        public async Task<TEntity> Post<TEntity>(string url, Dictionary<string, object> body) where TEntity : class
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var sResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TEntity>(sResponse);
                }
                return null;
            }
            return null;
        }
    }
}
