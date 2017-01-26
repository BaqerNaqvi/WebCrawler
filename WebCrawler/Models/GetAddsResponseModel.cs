using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace WebCrawler.Models
{
    public class GetAddsResponseModel
    {
        public string status { get; set; }

        public string message { get; set; }

        //public List<AdInfoContainer> ads { get; set; }
        public List<AdInfoContainer> ads { get; set; }
    }
}
