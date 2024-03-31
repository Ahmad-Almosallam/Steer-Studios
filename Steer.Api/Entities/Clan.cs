using Steer.Api.Entities.Base;

namespace Steer.Api.Entities
{
    public class Clan : VersionedDocument
    {
        public required string Name { get; set; }
        public int TotalPoints { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();
    }
}
