using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class NewLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class NewGeometry
    {
        public Location location { get; set; }
    }

    public class Close
    {
        public int day { get; set; }
        public string time { get; set; }
    }

    public class Open
    {
        public int day { get; set; }
        public string time { get; set; }
    }

    public class Period
    {
        public Close close { get; set; }
        public Open open { get; set; }
    }

    public class NewOpeningHours
    {
        public bool open_now { get; set; }
        public List<Period> periods { get; set; }
        public List<string> weekday_text { get; set; }
    }

    public class NewPhoto
    {
        public int height { get; set; }
        public List<string> html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }

    public class Aspect
    {
        public int rating { get; set; }
        public string type { get; set; }
    }

    public class Review
    {
        public List<Aspect> aspects { get; set; }
        public string author_name { get; set; }
        public string author_url { get; set; }
        public string language { get; set; }
        public int rating { get; set; }
        public string relative_time_description { get; set; }
        public string text { get; set; }
        public int time { get; set; }
    }

    public class NewResult
    {
        public List<AddressComponent> address_components { get; set; }
        public string adr_address { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string international_phone_number { get; set; }
        public string name { get; set; }
        public OpeningHours opening_hours { get; set; }
        public List<Photo> photos { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public List<Review> reviews { get; set; }
        public string scope { get; set; }
        public List<string> types { get; set; }
        public string url { get; set; }
        public int utc_offset { get; set; }
        public string vicinity { get; set; }
    }

    public class NewRootObject
    {
        public List<object> html_attributions { get; set; }
        public NewResult result { get; set; }
        public string status { get; set; }
    }
}
