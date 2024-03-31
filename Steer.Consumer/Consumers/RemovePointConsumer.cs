using MassTransit;
using Steer.Api.Data;
using Steer.Api.Data.IRepos;
using Steer.Api.Entities;
using Steer.Api.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steer.Consumer.Consumers
{
    public class RemovePointConsumer : IConsumer<RemovePoints>
    {
        private readonly IClanRepository _clanRepository;

        public RemovePointConsumer(IClanRepository repository)
        {
            _clanRepository = repository;
        }

        public async Task Consume(ConsumeContext<RemovePoints> context)
        {
            var clan = await _clanRepository.GetAsync(c => c.Id == context.Message.ClanId);
            if (clan == null) return;

            var steerMemberContributed = clan.Members.FirstOrDefault(x => x.SteerUserId == context.Message.UserId);
            if (steerMemberContributed == null || steerMemberContributed.LeftAt != null) return;
            steerMemberContributed!.Points = Math.Max(0, steerMemberContributed!.Points - context.Message.Points); // Prevent negative points

            clan.TotalPoints = Math.Max(0, clan.TotalPoints - context.Message.Points); // Prevent negative points
            await _clanRepository.UpdateAsync(clan);
        }
    }
}
