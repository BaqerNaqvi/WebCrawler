using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebCrawler.DataCore;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class BillingController : ApiController
    {
        public BillingContainer GetBillingHistory(int vendorId)
        {
            var controller = new BusinessCoreController();
            var obj = controller.GetBillingHistory(vendorId);
            return obj;
        }
    }
}