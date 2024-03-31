using MongoDB.Bson;
using Steer.Api.Dtos;
using Steer.Api.Entities;

namespace Steer.Api.Services.IServices
{
    public interface IClanService
    {
        Task<List<Clan>> ListAsync();
        Task<Clan> GetAsync(ObjectId clanId);
        Task<bool> JoinClanAsync(ObjectId clanId);
        Task<bool> LeaveClanAsync(ObjectId clanId);
        Task<bool> AddPointsAsync(ObjectId clanId, int points);
        Task<bool> RemovePointsAsync(ObjectId clanId, int points);
        Task<bool> SetPointsAsync(ObjectId clanId, int points);
        Task<List<CurrentContributionsDto>> ListCurrentContributionsAsync(ObjectId clanId);
        Task<bool> SeedData();
        Task<bool> RaceCondition();
    }
}
