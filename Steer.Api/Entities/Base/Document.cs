using MongoDB.Bson;

namespace Steer.Api.Entities.Base
{
    public class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedTime => Id.CreationTime;
    }
}
