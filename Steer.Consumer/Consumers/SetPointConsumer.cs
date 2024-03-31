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
    public class SetPointConsumer : IConsumer<SetPoints>
    {
        private readonly IClanRepository _clanRepository;

        public SetPointConsumer(IClanRepository repository)
        {
            _clanRepository = repository;
        }

        public async Task Consume(ConsumeContext<SetPoints> context)
        {
            var clan = await _clanRepository.GetAsync(c => c.Id == context.Message.ClanId);
            if (clan == null) return;

            foreach (var member in clan.Members)
            {
                if (member.SteerUserId == context.Message.UserId)
                    member!.Points = context.Message.Points;
                
                else
                    member!.Points = 0;
            }

            var steerMemberContributed = clan.Members.FirstOrDefault(x => x.SteerUserId == context.Message.UserId);
            if (steerMemberContributed == null || steerMemberContributed.LeftAt != null) return;
            steerMemberContributed!.Points = context.Message.Points;

            clan.TotalPoints = context.Message.Points;
            await _clanRepository.UpdateAsync(clan);
        }
    }
}
