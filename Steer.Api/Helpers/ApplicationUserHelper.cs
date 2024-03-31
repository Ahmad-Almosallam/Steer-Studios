using MongoDB.Bson;
using System.Security.Claims;

namespace Steer.Api.Helpers
{
    public class ApplicationUserHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApplicationUserHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public ObjectId GetUserId()
        {
            var objectId = _contextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (objectId != null)
            {
                return ObjectId.Parse(objectId.Value);
            }
            return ObjectId.Empty;
        }
    }
}
