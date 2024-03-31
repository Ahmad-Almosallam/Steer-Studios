using MongoDB.Bson;
using Steer.Api.Entities.Base;

namespace Steer.Api.Entities
{
    public class Token : Document
    {
        public ObjectId SteerUserId { get; set; }
        public string AccessToken { get; set; }
        public bool IsValid { get; set; }
    }
}
