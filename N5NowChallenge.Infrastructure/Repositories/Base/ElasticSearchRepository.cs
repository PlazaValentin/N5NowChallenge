using Microsoft.EntityFrameworkCore;
using N5NowChallenge.Domain.Entities.Base;
using Nest;

namespace N5NowChallenge.Infrastructure.Repositories.Base
{
    public class ElasticSearchRepository<T> : Repository<T>, IElasticSearchRepository<T> where T : Entity
    {
        private readonly IElasticClient _elasticClient;
        public ElasticSearchRepository(IElasticClient elasticClient, DbContext dbContext) : base(dbContext)
        {
            _elasticClient = elasticClient;
        }

        public async Task<bool> UpdateDocumentInIndexAsync(UpdateRequest<T, T> updateRequest, CancellationToken cancellationToken = default)
        {

            var updateResponse = await _elasticClient.UpdateAsync(updateRequest, cancellationToken);

            if (!updateResponse.IsValid)
                Console.WriteLine(updateResponse);

            return updateResponse.IsValid;
        }

        public async Task<IEnumerable<T>> SearchDocumentsInIndexAsync(SearchRequest<T> searchRequest, CancellationToken cancellationToken = default)
        {
            var searchResponse = await _elasticClient.SearchAsync<T>(searchRequest, cancellationToken);

            if(!searchResponse.IsValid)
                Console.WriteLine(searchResponse);

            return searchResponse.Documents;
        }
        public async Task<bool> AddDocumentToIndexAsync(T permission, CancellationToken cancellationToken = default)
        {
            var indexResponse = await _elasticClient.IndexDocumentAsync(permission, cancellationToken);

            if (!indexResponse.IsValid)
                Console.WriteLine(indexResponse);

            return indexResponse.IsValid;
        }
    }
}
