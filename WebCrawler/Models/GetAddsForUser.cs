using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class GetAddsForUser
    {
        public string location_lat { get; set; }
        public string location_lng { get; set; }
        public string user_id { get; set; }
    }
}
