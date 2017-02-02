using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class AdInfoForApi
    {
        public int type { get; set; }
        public string text { get; set; }
        public string symbol_url { get; set; }

        public string advertiser_url { get; set; }
        public string advertiser_logo { get; set; }

        public string couponUrl { get; set; }

        public string advertiser_tagline { get; set; }
        public string phone_number { get; set; }

        public int landmarkType { get; set; }
        public string landmark_detail_url { get; set; }

        public Nullable<int> interestId { get; set; }

        public Nullable<int> isCustom { get; set; }
        public string customInterest { get; set; }
    }
}
