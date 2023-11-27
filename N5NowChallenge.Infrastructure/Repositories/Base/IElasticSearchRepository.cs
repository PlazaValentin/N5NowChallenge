using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Domain.Entities.Base;
using Nest;

namespace N5NowChallenge.Infrastructure.Repositories.Base;

public interface IElasticSearchRepository<T> : IRepository<T> where T : Entity
{
    Task<bool> UpdateDocumentInIndexAsync(UpdateRequest<T, T> updateRequest, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> SearchDocumentsInIndexAsync(SearchRequest<T> searchRequest, CancellationToken cancellationToken = default);
    Task<bool> AddDocumentToIndexAsync(T permission, CancellationToken cancellationToken = default);
}
