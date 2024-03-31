using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Steer.Api.Entities.Base
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedTime { get; }
    }
}
