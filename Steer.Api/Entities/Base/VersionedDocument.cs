using MongoDB.Bson;

namespace Steer.Api.Entities.Base
{
    public class VersionedDocument : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedTime => Id.CreationTime;
        public int? Version { get; set; }
    }
}
