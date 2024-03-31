using MongoDB.Bson;
using MongoDB.Driver;
using Steer.Api.Data.IRepos;
using Steer.Api.Dtos;
using Steer.Api.Entities;

namespace Steer.Api.Data.Repos
{
    public class ClanRepository : Repository<Clan>, IClanRepository
    {
        public ClanRepository(MongoClient client, IMongoDatabase database) : base(client, database)
        {
        }

        public async Task<List<CurrentContributionsDto>> ListCurrentContributionsAsync(ObjectId clanId)
        {
            var query = await GetCollectionAsync();
            var pipeline = new BsonDocument[]
                    {
                        new BsonDocument("$match", new BsonDocument("_id", clanId)),
                        new BsonDocument("$unwind", "$Members"),
                        new BsonDocument("$lookup",
                            new BsonDocument
                            {
                                { "from", "SteerUser" },
                                { "localField", "Members.SteerUserId" },
                                { "foreignField", "_id" },
                                { "as", "MemberInfo" }
                            }),
                        new BsonDocument("$unwind", "$MemberInfo"),
                        new BsonDocument("$project",
                            new BsonDocument
                            {
                                { "_id", 0 },
                                { "UserName", "$MemberInfo.UserName" },
                                { "LeftAt", "$Members.LeftAt" },
                                { "Points", "$Members.Points" }
                            })
                    };

            var options = new AggregateOptions { AllowDiskUse = true };

            var results = await query.AggregateAsync<CurrentContributionsDto>(pipeline, options);

            return await results.ToListAsync();
        }

        async Task<bool> IClanRepository.UpdateAsync(Clan entity)
        {
            var collection = await GetCollectionAsync();
            var filter = Builders<Clan>.Filter.And(
                    Builders<Clan>.Filter.Eq(e => e.Id, entity.Id),
                    Builders<Clan>.Filter.Eq("Version", entity.Version)
                );


            if (entity.Version.HasValue)
                entity.Version = entity.Version.Value + 1;
            else
                entity.Version = 1;


            var result = await collection.ReplaceOneAsync(filter, entity);

            return result.ModifiedCount == 1;
        }
    }
}
