using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class BillingContainer
    {
        public List<BillingHistory_Temp> BillingHistory { get; set; }

        public double DuePayment { get; set; }

        public bool isPaid { get; set; }
    }
}
