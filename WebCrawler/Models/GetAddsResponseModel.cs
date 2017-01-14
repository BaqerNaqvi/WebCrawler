using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class GetAddsResponseModel
    {
        public int Count { get; set; }

        public string Status { get; set; }

        public List<AdInfoPlaceInfoObj> Ads { get; set; }
    }
}
