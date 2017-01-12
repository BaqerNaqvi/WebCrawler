using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class BillingHistory_Temp
    {
        public int Id { get; set; }

        public string InvoiceNo { get; set; }

        public string TranslationId { get; set; }

        public DateTime TransDateTime { get; set; }
        public string TransDateTimeStr { get; set; }

        public double Amount { get; set; }
    }
}
