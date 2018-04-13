using EncampmentAnalyticsApi.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace EncampmentAnalyticsApi.Controllers
{
    [RoutePrefix("api")]
    public class ApiController : System.Web.Http.ApiController
    {
        [HttpGet]
        [Route("encampment")]
        public HttpResponseMessage EncampmentCounts()
        {
            using (var db = RegistrationDBEntities.CreateProduction())
            {
                var allStakes = db.getCountsByAgeAndStake();

                var data = new StringBuilder();
                data.Append("Stake,11 Yr Old,Deacon,Teacher,Priest,Adult,Total\n");

                var C11YrOldTotal = 0;
                var DeaconTotal = 0;
                var TeacherTotal = 0;
                var PriestTotal = 0;
                var AdultTotal = 0;
                var GrandTotal = 0;

                foreach (var row in allStakes)
                {
                    var c11YrOld = row.C11_Yr_Old ?? 0;
                    var deacon = row.Deacon ?? 0;
                    var teacher = row.Teacher ?? 0;
                    var priest = row.Priest ?? 0;
                    var adult = row.Adult ?? 0;

                    var stakeTotal = c11YrOld + deacon + teacher + priest + adult;
                    data.Append($"{row.Stake},{row.C11_Yr_Old},{row.Deacon},{row.Teacher},{row.Priest},{row.Adult},{stakeTotal}\n");

                    C11YrOldTotal += row.C11_Yr_Old ?? 0;
                    DeaconTotal += row.Deacon ?? 0;
                    TeacherTotal += row.Teacher ?? 0;
                    PriestTotal += row.Priest ?? 0;
                    AdultTotal += row.Adult ?? 0;
                    GrandTotal += stakeTotal;
                }

                data.Append($"Totals,{C11YrOldTotal},{DeaconTotal},{TeacherTotal},{PriestTotal},{AdultTotal},{GrandTotal}\n");

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(data.ToString(), Encoding.UTF8, "text/csv")
                };

                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"EncampmentRegistration_{DateTime.Now.ToString("yyyy.MM.dd")}.csv"
                    };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

                return result;
            }
        }

        [HttpGet]
        [Route("encampment/stake/{id}")]
        [Authorize]
        public HttpResponseMessage StakeCounts(int id)
        {
            using (var db = RegistrationDBEntities.CreateProduction())
            {
                var stakeName = db.Groups.Where(g => g.Id == id).Select(g => g.Name).FirstOrDefault();

                var allWards = db.getCountsByAgeAndStakeAndWard(id);

                var data = new StringBuilder();
                data.Append("Stake,Ward,11 Yr Old,Deacon,Teacher,Priest,Adult,Total\n");

                var C11YrOldTotal = 0;
                var DeaconTotal = 0;
                var TeacherTotal = 0;
                var PriestTotal = 0;
                var AdultTotal = 0;
                var GrandTotal = 0;

                foreach (var row in allWards)
                {
                    var c11YrOld = row.C11_Yr_Old ?? 0;
                    var deacon = row.Deacon ?? 0;
                    var teacher = row.Teacher ?? 0;
                    var priest = row.Priest ?? 0;
                    var adult = row.Adult ?? 0;

                    var stakeTotal = c11YrOld + deacon + teacher + priest + adult;
                    data.Append($"{row.Stake},{row.Ward},{row.C11_Yr_Old},{row.Deacon},{row.Teacher},{row.Priest},{row.Adult},{stakeTotal}\n");

                    C11YrOldTotal += row.C11_Yr_Old ?? 0;
                    DeaconTotal += row.Deacon ?? 0;
                    TeacherTotal += row.Teacher ?? 0;
                    PriestTotal += row.Priest ?? 0;
                    AdultTotal += row.Adult ?? 0;
                    GrandTotal += stakeTotal;
                }

                data.Append($"Totals,,{C11YrOldTotal},{DeaconTotal},{TeacherTotal},{PriestTotal},{AdultTotal},{GrandTotal}\n");

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(data.ToString(), Encoding.UTF8, "text/csv")
                };

                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"EncampmentRegistration_{stakeName}_{DateTime.Now.ToString("yyyy.MM.dd")}.csv"
                    };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

                return result;
            }
        }

        [HttpGet]
        [Route("encampment/stake/ward/{id}")]
        [Authorize]
        public HttpResponseMessage WardList(int id)
        {
            using (var db = RegistrationDBEntities.CreateProduction())
            {
                var subgroup = db.Subgroups.FirstOrDefault(sg => sg.Id == id);
                var wardName = subgroup.Name;
                var stakeName = subgroup.Group.Name;

                var wardAttendees = db.getListOfAttendees(id);

                var data = new StringBuilder();
                data.Append("FirstName,LastName,IsAdult,AgeDuringEncampment,DateOfBirth,ShirtSize,Triathlon\n");

                foreach (var attendee in wardAttendees)
                {
                    var isAdult = attendee.isadult ? "Yes" : "No";
                    var triathlon = attendee.triathlon ? "Yes" : "No";
                    data.Append($"{attendee.firstname},{attendee.lastname},{isAdult},{attendee.age},{attendee.dateofbirth?.ToString("MM/dd/yyyy")},{attendee.size},{triathlon}\n");
                }

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(data.ToString(), Encoding.UTF8, "text/csv")
                };

                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"EncampmentRegistration_{stakeName}_{wardName}_{DateTime.Now.ToString("yyyy.MM.dd")}.csv"
                    };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

                return result;
            }
        }
    }
}