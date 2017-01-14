using System.Web.Http;
using System.Web.Script.Serialization;
using WebCrawler.DataCore.Managers;
using WebCrawler.Models;

namespace WebCrawler.Controllers.api
{
    public class GetAddsController : ApiController
    {
        // GET api/<controller>
        public GetAddsResponseModel Get(string uid, string lat, string lng)
        {
            var addmanager = new AddManager();
            var adds= addmanager.GetAdss(new GetAddsForUser {location_lat = lat, location_lng = lng, user_id = uid});
            return new GetAddsResponseModel {Status = "OK", Count = adds.Count, Ads = adds};
            //JavaScriptSerializer js = new JavaScriptSerializer();
            // return new GetAddsResponseModel { };
            //  return js.Serialize(new GetAddsResponseModel {Status = "OK", Count = adds.Count, Ads = adds});
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}