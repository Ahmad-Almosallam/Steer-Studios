using MongoDB.Bson;

namespace Steer.Api.MessageContracts
{
    public class AddPoints
    {
        public ObjectId UserId { get; set; }
        public ObjectId ClanId { get; set; }
        public int Points { get; set; }
    }
}
