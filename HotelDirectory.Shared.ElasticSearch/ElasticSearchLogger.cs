using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Shared.ElasticSearch.Model;
using Nest;

namespace HotelDirectory.Shared.ElasticSearch
{
    public interface IElasticSearchLogger<T> where T : class
    {
        object AddLog(T logModel);
    }

    public class ElasticSearchLogger<T> : IElasticSearchLogger<T> where T : class
    {
        private readonly IElasticClient _elasticClient;
        public ElasticSearchLogger(IElasticClient elasticClient) => _elasticClient = elasticClient;
        public object AddLog(T logModel)
        {
            return _elasticClient.IndexDocument(logModel);
        }

    }

}
