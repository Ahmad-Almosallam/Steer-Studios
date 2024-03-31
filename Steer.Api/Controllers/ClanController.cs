using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Steer.Api.Attributes;
using Steer.Api.Dtos;
using Steer.Api.Entities;
using Steer.Api.Services.IServices;

namespace Steer.Api.Controllers
{
    [Route("api/[controller]")]
    public class ClanController : Controller
    {
        private readonly IClanService _clanService;

        public ClanController(IClanService clanService)
        {
            _clanService = clanService;
        }

        [HttpPost("race")]
        public async Task<bool> RaceCondition()
        {
            return await _clanService.RaceCondition();
        }

        [HttpPost("seed-data")]
        public async Task<bool> SeedData()
        {
            return await _clanService.SeedData();
        }

        [SteerAuthorize]
        [HttpGet("list")]
        public async Task<List<Clan>> ListAsync()
        {
            return await _clanService.ListAsync();
        }

        [SteerAuthorize]
        [HttpGet("{clanId}")]
        public async Task<ActionResult<Clan>> GetAsync(ObjectId clanId)
        {
            var clan = await _clanService.GetAsync(clanId);

            if (clan == null)
            {
                return NotFound();
            }

            return clan;
        }

        [SteerAuthorize]
        [HttpPost("{clanId}/join")]
        public async Task<bool> JoinClanAsync(ObjectId clanId)
        {
            return await _clanService.JoinClanAsync(clanId);
        }

        [SteerAuthorize]
        [HttpPost("{clanId}/leave")]
        public async Task<bool> LeaveClanAsync(ObjectId clanId)
        {
            return await _clanService.LeaveClanAsync(clanId);
        }

        [SteerAuthorize]
        [HttpPost("{clanId}/points/add/{points}")]
        public async Task<bool> AddPointsAsync(ObjectId clanId, int points)
        {
            return await _clanService.AddPointsAsync(clanId, points);
        }

        [SteerAuthorize]
        [HttpPost("{clanId}/points/remove/{points}")]
        public async Task<bool> RemovePointsAsync(ObjectId clanId, int points)
        {
            return await _clanService.RemovePointsAsync(clanId, points);
        }

        [SteerAuthorize]
        [HttpPost("{clanId}/points/set/{points}")]
        public async Task<bool> SetPointsAsync(ObjectId clanId, int points)
        {
            return await _clanService.SetPointsAsync(clanId, points);
        }

        [SteerAuthorize]
        [HttpGet("{clanId}/contributions")]
        public async Task<List<CurrentContributionsDto>> ListCurrentContributionsAsync(ObjectId clanId)
        {
            return await _clanService.ListCurrentContributionsAsync(clanId);
        }

    }
}
