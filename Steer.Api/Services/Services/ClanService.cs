using MassTransit;
using MongoDB.Bson;
using MongoDB.Driver;
using Steer.Api.Data;
using Steer.Api.Data.IRepos;
using Steer.Api.Dtos;
using Steer.Api.Entities;
using Steer.Api.Helpers;
using Steer.Api.MessageContracts;
using Steer.Api.Services.IServices;

namespace Steer.Api.Services.Services
{
    public class ClanService : IClanService
    {
        private readonly IClanRepository _clanRepository;
        private readonly IRepository<SteerUser> _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ApplicationUserHelper _applicationUserHelper;

        public ClanService(IClanRepository clanRepository,
                           IRepository<SteerUser> userRepository,
                           IPublishEndpoint publishEndpoint,
                           ApplicationUserHelper applicationUserHelper)
        {
            _clanRepository = clanRepository;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _applicationUserHelper = applicationUserHelper;
        }


        public async Task<Clan> GetAsync(ObjectId clanId)
        {
            var clan = await _clanRepository.GetAsync(c => c.Id == clanId);
            return clan;
        }

        public async Task<bool> JoinClanAsync(ObjectId clanId)
        {
            ObjectId steerUserId = _applicationUserHelper.GetUserId();
            var clan = await _clanRepository.GetAsync(c => c.Id == clanId);
            if (clan == null)
            {
                throw new InvalidOperationException("Clan not found.");
            }


            var user = await _userRepository.GetAsync(u => u.Id == steerUserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (user.JoinedClanId.HasValue)
            {
                return false; // User is already in a clan.
            }

            var hasMemberJoinedTheClanAndLeft = clan.Members.FirstOrDefault(x => x.SteerUserId == steerUserId);
            if (hasMemberJoinedTheClanAndLeft == null)
            {
                var member = new Member
                {
                    Id = new ObjectId(),
                    SteerUserId = steerUserId,
                    JoinedAt = DateTime.UtcNow
                };

                clan.Members.Add(member);
            }
            else
            {
                hasMemberJoinedTheClanAndLeft.LeftAt = null;
            }

            user.JoinedClanId = clanId;

            await _clanRepository.UpdateAsync(clan);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> LeaveClanAsync(ObjectId clanId)
        {
            ObjectId steerUserId = _applicationUserHelper.GetUserId();
            var clan = await _clanRepository.GetAsync(c => c.Id == clanId);
            if (clan == null) return false;

            var steerUser = await _userRepository.GetAsync(x => x.Id == steerUserId);
            if (steerUser == null) return false;

            steerUser.JoinedClanId = null;

            var steerMemberToLeave = clan.Members.FirstOrDefault(x => x.SteerUserId == steerUserId);
            if (steerMemberToLeave == null) return false;

            steerMemberToLeave.LeftAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(steerUser);
            await _clanRepository.UpdateAsync(clan);
            return true;
        }

        public async Task<List<Clan>> ListAsync()
        {
            var list = await (await _clanRepository.GetCollectionAsync()).FindAsync(_ => true);
            return list.ToList();
        }

        public async Task<List<CurrentContributionsDto>> ListCurrentContributionsAsync(ObjectId clanId)
        {
            return await _clanRepository.ListCurrentContributionsAsync(clanId);
        }
        public async Task<bool> AddPointsAsync(ObjectId clanId, int points)
        {
            await _publishEndpoint.Publish(new AddPoints()
            {
                ClanId = clanId,
                Points = points,
                UserId = _applicationUserHelper.GetUserId()
            });
            return true;
        }

        public async Task<bool> RemovePointsAsync(ObjectId clanId, int points)
        {
            await _publishEndpoint.Publish(new RemovePoints()
            {
                ClanId = clanId,
                Points = points,
                UserId = _applicationUserHelper.GetUserId()
            });
            return true;
        }

        public async Task<bool> SetPointsAsync(ObjectId clanId, int points)
        {
            await _publishEndpoint.Publish(new SetPoints()
            {
                ClanId = clanId,
                Points = points,
                UserId = _applicationUserHelper.GetUserId()
            });

            return true;
        }

        public async Task<bool> SeedData()
        {
            var clanCollection = await _clanRepository.GetCollectionAsync();

            if (clanCollection == null) { return false; }

            if (await clanCollection.CountDocumentsAsync(_ => true) <= 0)
            {
                await clanCollection.InsertOneAsync(new Clan()
                {
                    Name = "Clan 1",
                    TotalPoints = 0,
                });
                await clanCollection.InsertOneAsync(new Clan()
                {
                    Name = "Clan 2",
                    TotalPoints = 0,
                });
            }


            var userCollection = await _userRepository.GetCollectionAsync();

            if (userCollection == null) { return false; }

            if (await userCollection.CountDocumentsAsync(_ => true) <= 0)
            {
                await userCollection.InsertOneAsync(new SteerUser() { UserName = "Ahmad" });
            }

            return true;
        }

        public async Task<bool> RaceCondition()
        {
            var clan = new Clan() { Name = "race" };
            var clanCollection = await _clanRepository.GetCollectionAsync();
            await clanCollection.InsertOneAsync(clan);
            clan.TotalPoints = 1;

            var sameClan = await _clanRepository.GetAsync(x => x.Id == clan.Id);
            sameClan.TotalPoints = 100;

            var q = await _clanRepository.UpdateAsync(sameClan);
            var w = await _clanRepository.UpdateAsync(clan);

            return true;
        }
    }
}
