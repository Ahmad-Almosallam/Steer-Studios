using MongoDB.Driver;
using Steer.Api.Entities.Base;
using System.Linq.Expressions;

namespace Steer.Api.Data
{
    public interface IRepository<TDocument> where TDocument : IDocument
    {
        Task<IMongoCollection<TDocument>> GetCollectionAsync();
        Task<TDocument> GetAsync(Expression<Func<TDocument, bool>> predicate);
        Task UpdateAsync(TDocument entity);
    }
}
