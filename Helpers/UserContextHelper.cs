using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Gestion_Salle_classe.Helpers
{
    public class UserContext
    {
        public int? UserId { get; set; }
        public string Role { get; set; }
        public bool IsAuthenticated => UserId.HasValue && !string.IsNullOrEmpty(Role);
    }

    public static class UserContextHelper
    {
        public static UserContext FromRequest(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            int? userId = null;
            if (headers.Contains("X-User-Id"))
            {
                int parsed;
                if (int.TryParse(headers.GetValues("X-User-Id").FirstOrDefault(), out parsed))
                    userId = parsed;
            }

            string role = null;
            if (headers.Contains("X-User-Role"))
                role = headers.GetValues("X-User-Role").FirstOrDefault()?.ToLowerInvariant();

            return new UserContext { UserId = userId, Role = role };
        }
    }
}
