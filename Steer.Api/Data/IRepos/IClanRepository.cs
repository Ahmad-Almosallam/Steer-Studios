using MongoDB.Bson;
using Steer.Api.Dtos;
using Steer.Api.Entities;

namespace Steer.Api.Data.IRepos
{
    public interface IClanRepository : IRepository<Clan>
    {
        Task<List<CurrentContributionsDto>> ListCurrentContributionsAsync(ObjectId clanId);
        Task<bool> UpdateAsync(Clan entity);
    }
}
