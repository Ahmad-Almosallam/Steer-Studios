using MassTransit;
using Steer.Api.Data;
using Steer.Api.Data.IRepos;
using Steer.Api.Entities;
using Steer.Api.MessageContracts;

namespace Steer.Consumer.Consumers
{
    public class AddPointConsumer : IConsumer<AddPoints>
    {
        private readonly IClanRepository _clanRepository;

        public AddPointConsumer(IClanRepository repository)
        {
            _clanRepository = repository;
        }

        public async Task Consume(ConsumeContext<AddPoints> context)
        {
            var clan = await _clanRepository.GetAsync(c => c.Id == context.Message.ClanId);
            if (clan == null) return;

            var steerMemberContributed = clan.Members.FirstOrDefault(x => x.SteerUserId == context.Message.UserId);
            if (steerMemberContributed == null || steerMemberContributed.LeftAt != null) return;
            steerMemberContributed!.Points += context.Message.Points;

            clan.TotalPoints += context.Message.Points;
            await _clanRepository.UpdateAsync(clan);
        }
    }
}
