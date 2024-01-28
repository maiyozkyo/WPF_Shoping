using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public interface IApiService
    {
        public Task<TEntity> Post<TEntity>(string url, Dictionary<string, object> body) where TEntity : class;

    }
}
