using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class UpdatePassword
    {
        public string CurrentPass { get; set; }

        public string NewPass { get; set; }

        public string RepeatNewPass { get; set; }

        public string UserId { get; set; }
    }
}
