using MongoDB.Driver;
using Steer.Api.Entities.Base;
using System.Linq.Expressions;

namespace Steer.Api.Data.Repos
{
    public class Repository<TDocument> : IRepository<TDocument> where TDocument : IDocument
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public Repository(MongoClient client, IMongoDatabase database)
        {
            _client = client;
            _database = database;
        }

        public async Task<TDocument> GetAsync(Expression<Func<TDocument, bool>> predicate)
        {
            var collection = _database.GetCollection<TDocument>(typeof(TDocument).Name);
            var q = await collection.FindAsync(predicate);
            return q.FirstOrDefault();
        }

        public async Task<IMongoCollection<TDocument>> GetCollectionAsync()
        {
            return await Task.Run(() => _client.GetDatabase("Steer").GetCollection<TDocument>(typeof(TDocument).Name));
        }

        public async Task UpdateAsync(TDocument entity)
        {
            var collection = _database.GetCollection<TDocument>(typeof(TDocument).Name);

            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, entity.Id);

            await collection.ReplaceOneAsync(filter, entity);
        }
    }
}
