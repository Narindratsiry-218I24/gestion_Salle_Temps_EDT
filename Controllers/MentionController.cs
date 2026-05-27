using System.Linq;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Mention")]
    public class MentionController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetMentions()
        {
            var mentions = db.Mentions.OrderBy(m => m.NomMention).ToList();
            return Ok(mentions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
