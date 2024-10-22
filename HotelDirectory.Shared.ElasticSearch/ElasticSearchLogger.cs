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
        object AddLogList(List<T> logModel);
        object GetByCustomId(GenericLogModel genericLogModel);
    }

    public class ElasticSearchLogger<T> : IElasticSearchLogger<T> where T : class
    {
        private readonly IElasticClient _elasticClient;
        public ElasticSearchLogger(IElasticClient elasticClient) => _elasticClient = elasticClient;
        public object AddLog(T logModel)
        {
            return _elasticClient.IndexDocument(logModel);
        }
        public object AddLogList(List<T> logModel)
        {
            return _elasticClient.IndexMany(logModel);
        }
        public object GetByCustomId(GenericLogModel genericLogModel)
        {
            var c = _elasticClient.Search<GenericLogModel>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(f => f
                            .Term(r => r
                                .Field(fld => fld.LogCustomId.Trim())
                                .Value(genericLogModel.LogCustomId)
                            )
                        )
                    )
                ).Size(100)
            );
            List<GenericLogModel> logModels = c.Documents.ToList();
            return logModels;
        }
    }

}
