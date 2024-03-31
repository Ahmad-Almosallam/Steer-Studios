using MongoDB.Bson;
using Steer.Api.Entities.Base;

namespace Steer.Api.Entities
{
    public class SteerUser : Document
    {
        public required string UserName { get; set; }
        public ObjectId? JoinedClanId { get; set; }
    }
}
