using EncampmentAnalyticsApi.Models;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace EncampmentAnalyticsApi.Controllers
{
    [RoutePrefix("api")]
    public class ApiController : System.Web.Http.ApiController
    {
        [HttpGet]
        [Route("public")]
        public IHttpActionResult Public()
        {
            return Json(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
        }

        [HttpGet]
        [Route("encampment")]
        [Authorize]
        public IHttpActionResult EncampmentCounts()
        {
            var db = RegistrationDBEntities.CreateProduction();

            var result = db.getCountsByAgeAndStake();
            var viewable = result.ToList();

            return Ok(viewable);
        }

        [HttpGet]
        [Route("encampment/stake/{id}")]
        [Authorize]
        public IHttpActionResult StakeCounts(int id)
        {
            var db = RegistrationDBEntities.CreateProduction();

            var result = db.getCountsByAgeAndStakeAndWard(id);
            var viewable = result.ToList();

            return Ok(viewable);
        }

        [HttpGet]
        [Route("encampment/stake/ward/{id}")]
        [Authorize]
        public IHttpActionResult WardList(int id)
        {
            var db = RegistrationDBEntities.CreateProduction();

            var result = db.getListOfAttendees(id);
            var viewable = result.ToList();

            return Ok(viewable);
        }
    }
}