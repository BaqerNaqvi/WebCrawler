using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebCrawler.DataCore;

namespace WebCrawler.Controllers
{
    public class AdvertiserController : ApiController
    {

        private static readonly BusinessCoreController Bcontroller = new BusinessCoreController();

        public Vendor GetProfile(int vendorId)
        {
            var obj = Bcontroller.GetAdvertiserProfile(vendorId);
            return obj;
        }

        // POST api/<controller>
        public Vendor Post(Vendor obj)
        {
            var result = Bcontroller.UpdateAdvertiserProfile(obj);
            return result;
        }

        
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}