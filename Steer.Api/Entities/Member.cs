using MongoDB.Bson;
using Steer.Api.Entities.Base;

namespace Steer.Api.Entities
{
    public class Member : Document
    {
        public ObjectId SteerUserId { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public int Points { get; set; }
    }
}
