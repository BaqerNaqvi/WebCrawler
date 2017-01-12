using WebCrawler;
using WebCrawler.DataCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mail;
using Utilities;
using WebCrawler.DataCore.Managers;

using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace WebCrawler.Controllers
{
    public class ValuesController : ApiController
    {

        private static BusinessCoreController controller = new BusinessCoreController();
        List<includedWord> requiredKeywoard = new List<includedWord>();// controller.GetIncludedWords(); //{ "test", "test2", "Educators", "Live", "Shows", "Show Times", "Events", "Coupons", "Beer", "Draft", "Performance", "Exhibit", "Sale", "Meetings", "Rides", "Amenities", "Services", "Specialties", "Bands", "Coming Soon", "Don't Miss", "Fees" };
        List<excludedWord> restrictedKeywords = new List<excludedWord>();//controller.GetExcludedWords();
        string apiKey = "AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8"; // Your api key
        // GET api/<controller>
        [HttpGet]
        public string Get()
        {
            //return new string[] { "value1", "value2" };
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string status = nvc["status"];
            List<includedWord> requiredKeywoard = controller.GetIncludedWords(); //{ "test", "test2", "Educators", "Live", "Shows", "Show Times", "Events", "Coupons", "Beer", "Draft", "Performance", "Exhibit", "Sale", "Meetings", "Rides", "Amenities", "Services", "Specialties", "Bands", "Coming Soon", "Don't Miss", "Fees" };
            List<excludedWord> restrictedKeywords = controller.GetExcludedWords();


            if (status == "2")
            {
                string sUrl = "";

                if (nvc["crawlUrl"] != "")
                {
                    sUrl = nvc["crawlUrl"];
                }
                else
                {
                    var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return sr.Serialize("Data Not Found");
                }

                //Get All URLs from page

                WebClient w = new WebClient();
                LinkItem linkItemObj = new LinkItem();
                List<string> anchorList = new List<string>();
                string file = "";
                anchorList.Add(sUrl);


                try
                {

                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("Accept-Language", " en-US");
                        client.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
                        client.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                        file = client.DownloadString(sUrl);
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                    {
                        var resp = (HttpWebResponse)ex.Response;
                        if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                        {
                            //Handle it
                        }
                    }
                    //Handle it
                }

                // Find all matches in file.
                MatchCollection urlCollection = Regex.Matches(file, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);
                // Loop over each url.
                foreach (Match m in urlCollection)
                {
                    string value = m.Groups[1].Value;

                    // Get href attribute.
                    Match hrefValue = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);

                    //Check if Url is complete.
                    if (CheckValidUrl(hrefValue.Groups[1].Value))
                    {
                        linkItemObj.Href = hrefValue.Groups[1].Value;
                    }
                    else
                    {
                        linkItemObj.Href = sUrl + hrefValue.Groups[1].Value;
                    }

                    // Remove inner tags from text.
                    string hrefText = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
                    linkItemObj.Text = hrefText.ToLower();



                    foreach (var x in requiredKeywoard)
                    {
                        if (linkItemObj.Text.Contains(x.word.ToString().ToLower()))
                        {

                            anchorList.Add(linkItemObj.Href);

                        }
                    }

                }

                //At this point we have all the href tags in anchorList of type string

                List<string> json = new List<string>();
                List<string> selectedKeywords = new List<string>();
                List<string> distinctAnchorList = new List<string>();
                distinctAnchorList = anchorList.Distinct().ToList();
                List<string> headerValue = new List<string>();
                string file2 = "";
                foreach (string hrefValue in distinctAnchorList)
                {

                    try
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add("Accept-Language", " en-US");
                            client.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
                            client.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                            file2 = client.DownloadString(hrefValue);
                        }
                    }
                    catch (WebException ex)
                    {
                        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                        {
                            var resp = (HttpWebResponse)ex.Response;
                            if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                            {
                                //Handle it
                            }
                        }
                        //Handle it
                    }
                    // Find all matches in file.
                    String h1Regex = "<h[1-6][^>]*?>(?<TagText>.*?)</h[1-6]>";
                    MatchCollection matchedTextHeaders = Regex.Matches(file2, h1Regex, RegexOptions.Singleline);
                    // Loop over each match.
                    foreach (Match m in matchedTextHeaders)
                    {
                        string value = m.Groups[1].Value;
                        // Remove inner tags from text.
                        string t = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
                        headerValue.Add(t);
                    }

                }

                //Remove Restricted Keywords
                foreach (var x in restrictedKeywords)
                {
                    headerValue.RemoveAll(u => u.Contains(x.word.ToString()));
                    headerValue.RemoveAll(u => u.Contains(x.word.ToString().ToLower()));
                    headerValue.RemoveAll(u => u.Contains(x.word.ToString().ToUpper()));
                }

                //return passedUrl;
                List<string> distinct = headerValue.Distinct().ToList();

                var result = string.Join(",", distinct.Select(o => string.Concat("'", o, "'")));
                var seri = new System.Web.Script.Serialization.JavaScriptSerializer();
                return seri.Serialize(distinct);

            }
            if (status == "3")
            {
                int Id = 0;
                List<includedWord> inc = new List<includedWord>();
                List<excludedWord> ex = new List<excludedWord>();
                if (nvc["table"] == "include")
                {
                    //add in included
                    includedWord inc1 = new includedWord();
                    inc1.word = nvc["word"];
                    inc = controller.IncludeWord(inc1);
                    var seri = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return seri.Serialize(inc);
                }
                else
                {
                    //add in excluded
                    excludedWord ex1 = new excludedWord();
                    ex1.word = nvc["word"];
                    ex = controller.AddExcludeWord(ex1);
                    var seri = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return seri.Serialize(ex);
                }
            }
            if (status == "33")
            {

                //Get words
                if (nvc["table"] == "include")
                {
                    //Get included
                    List<includedWord> inc = new List<includedWord>();
                    inc = controller.GetIncludedWords();
                    var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return sr.Serialize(inc);
                }
                else
                {
                    //Get excluded
                    List<excludedWord> ex = new List<excludedWord>();
                    ex = controller.GetExcludedWords();
                    var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return sr.Serialize(ex);
                }

            }
            if (status == "333")
            {

                //Delete words
                if (nvc["table"] == "include")
                {
                    //Delete included

                    List<includedWord> inc = new List<includedWord>();
                    inc = controller.RemoveIncludedWord(int.Parse(nvc["Id"].ToString()));
                    var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return sr.Serialize(inc);
                }
                else
                {
                    //Delete excluded
                    List<excludedWord> ex = new List<excludedWord>();
                    ex = controller.RemoveExcludedWord(int.Parse(nvc["Id"].ToString()));
                    var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return sr.Serialize(ex);
                }

            }
            if (status == "4")
            {
                string u = "http://xevensolutions.com/cpls/test.html";
                return CrawlDataMain(u, "afjafjasljaslfjaslfi");


            }
            else
            {
                return "Invalid Request";
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]JObject data)
        {
            string results = "";
            InputJson result = new InputJson();


            if(data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }

            try
            {
                results = data.ToString(Formatting.None);
                result = new JavaScriptSerializer().Deserialize<InputJson>(results);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle it
                    }
                }
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
                //Handle it
            }
            

            var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<crawler> finalResult = new List<crawler>();
            List<crawler> allData = new List<crawler>();
            allData = controller.GetAllCrawlData();
            crawler cr = new crawler();
            string deviceId = result.DeviceID;
            string metaInfo = "";
            string website = "";
            string file21 = "";

            requiredKeywoard = controller.GetIncludedWords(); //{ "test", "test2", "Educators", "Live", "Shows", "Show Times", "Events", "Coupons", "Beer", "Draft", "Performance", "Exhibit", "Sale", "Meetings", "Rides", "Amenities", "Services", "Specialties", "Bands", "Coming Soon", "Don't Miss", "Fees" };
            restrictedKeywords = controller.GetExcludedWords();
            for (int i = 0; i < result.ListPlaces.Count; i++)
            {
                string placeId = result.ListPlaces[i].PlaceId;

                //check if there is any data exist agains DeviceId and coordinates. If this data is less then 7 days old just return data do not crawl

                cr = allData.Find(r => r.placeId == placeId);
                //crawler cr = controller.GetCrawlerByPlaceId(placeId);
                if (cr != null)
                {

                    //Check if htmldata field is empty and age of data
                    if (CheckDataAge(DateTime.Parse(cr.createdAt.ToString()), int.Parse(cr.Id.ToString())))
                    {
                        finalResult.Add(cr);
                    }
                    else
                    {
                        metaInfo = getMetaTags(cr.placeWebsite);
                        if (!string.IsNullOrEmpty(metaInfo))
                        {
                            cr.htmlData = metaInfo;
                            finalResult.Add(cr);
                        }
                        else
                        {
                            cr.htmlData = CrawlDataMain(cr.placeWebsite, cr.placeId);
                            finalResult.Add(cr);
                        }
                    }
                }
                else
                {

                    string p = "ChIJFXjIjXpO4DsR6IVFLCyThgY";
                    string url123 = string.Format(@"https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}", placeId, apiKey);

                    try
                    {

                        using (WebClient c = new WebClient())
                        {
                            c.Headers.Add("Accept-Language", " en-US");
                            c.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                            c.Headers.Add("Content-Type", "application/json");
                            c.Headers.Add("Accept", "application/json");
                            file21 = c.DownloadString(url123);
                        }
                    }
                    catch (WebException ex)
                    {
                        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                        {
                            var resp = (HttpWebResponse)ex.Response;
                            if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                            {
                                //Handle it
                            }
                        }
                        //Handle it
                        HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                        return error;
                    }
                    //string replacement = file21.Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty);
                    RootObject root = new JavaScriptSerializer().Deserialize<RootObject>(file21);
                    if (root.result != null)
                    {
                        if (string.IsNullOrEmpty(root.result.website))
                        {
                            website = "";
                        }
                        else
                        {
                            website = root.result.website;
                        }
                    }
                    else
                    {
                        website = "";
                    }
                    

                    metaInfo = getMetaTags(website);
                    if (!string.IsNullOrEmpty(metaInfo))
                    {
                        //cr.htmlData = metaInfo;
                        crawler addCrawler = new crawler();

                        addCrawler.placeId = placeId;
                        addCrawler.placeWebsite = website;
                        addCrawler.deviceId = result.DeviceID;
                        addCrawler.deviceLati = result.device_lat;
                        addCrawler.deviceLongi = result.device_lng;
                        addCrawler.htmlData = metaInfo;
                        addCrawler.createdAt = DateTime.Now;
                        addCrawler.googleData = file21;



                        int cId = controller.AddHtmlData(addCrawler);
                        finalResult.Add(addCrawler);
                    }
                    else
                    {

                        crawler addCrawler = new crawler();

                        addCrawler.placeId = placeId;
                        addCrawler.placeWebsite = website;
                        addCrawler.deviceId = result.DeviceID;
                        addCrawler.deviceLati = result.device_lat;
                        addCrawler.deviceLongi = result.device_lng;
                        addCrawler.htmlData = CrawlDataMain(website, placeId);
                        addCrawler.createdAt = DateTime.Now;
                        addCrawler.googleData = file21;



                        int cId = controller.AddHtmlData(addCrawler);
                        finalResult.Add(addCrawler);

                    }
                }


                //Check if website contain qlumi meta tag. If yes return just meta not the entire page result.


                //If there is no mata tag and existing data crawl site, save and return results.


                //crawlResult += CrawlDataMain(placeResult.Website);
            }


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, finalResult);


            return response;
        }


        public bool CheckDataAge(DateTime currentDate, int recordId)
        {
            DateTime d2 = DateTime.Now.AddDays(-7);
            double Day = (currentDate - d2).TotalDays;

            if (Day > 7)
            {
                controller.DeleteData(recordId);
                return false;
            }
            else
            {

                return true;
            }
        }
        public bool CheckValidUrl(string url)
        {

            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }
        public struct LinkItem
        {
            public string Href;
            public string Text;

            public override string ToString()
            {
                return Href + "\n\t" + Text;
            }
        }

        public class MetaTags
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }

        public string getMetaTags(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var metaTagsItem = new List<MetaTags>();
                var mItems = new List<MetaTags>();
                MetaTags mjson = new MetaTags();
                var metaTags = (HtmlAgilityPack.HtmlNodeCollection)null;
                var seri = new System.Web.Script.Serialization.JavaScriptSerializer();
                var jss = new JavaScriptSerializer();

                //Get meta tags of website
                try
                {

                    var htmlWeb = new HtmlWeb();
                    var document = htmlWeb.Load(url);
                    metaTags = document.DocumentNode.SelectNodes("//meta");
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                    {
                        var resp = (HttpWebResponse)ex.Response;
                        if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                        {
                            //Handle it
                        }
                    }
                    //Handle it
                }



                if (metaTags != null)
                {
                    metaTagsItem.AddRange(metaTags.Where(tag => tag.Attributes["name"] != null && tag.Attributes["content"] != null)
                        .Select(tag => new MetaTags
                        {
                            Name = tag.Attributes["name"].Value,
                            Content = tag.Attributes["content"].Value
                        }));


                    foreach (MetaTags m in metaTagsItem)
                    {
                        if (m.Name == "Qlumi")
                        {


                            mjson.Name = m.Name;
                            mjson.Content = m.Content;
                            mItems.Add(mjson);

                        }
                    }
                    if (mItems.Count > 0)
                    {
                        return seri.Serialize(mItems);
                    }
                    else
                    {
                        return "";
                    }



                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }




        }


        public string CrawlDataMain(string sUrl, string placeId)
        {
            requiredKeywoard = controller.GetIncludedWords(); //{ "test", "test2", "Educators", "Live", "Shows", "Show Times", "Events", "Coupons", "Beer", "Draft", "Performance", "Exhibit", "Sale", "Meetings", "Rides", "Amenities", "Services", "Specialties", "Bands", "Coming Soon", "Don't Miss", "Fees" };
            restrictedKeywords = controller.GetExcludedWords();

            if (sUrl == "")
            {
                return "";
            }

            //Get All URLs from page

            WebClient w = new WebClient();
            LinkItem linkItemObj = new LinkItem();
            List<string> anchorList = new List<string>();
            string file = "";
            anchorList.Add(sUrl);
            
            try
            {

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Accept-Language", " en-US");
                    client.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                    file = client.DownloadString(sUrl);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle it
                    }
                }
                //Handle it
            }

            // Find all matches in file.
            MatchCollection urlCollection = Regex.Matches(file, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);
            // Loop over each url.
            foreach (Match m in urlCollection)
            {
                string value = m.Groups[1].Value;

                // Get href attribute.
                Match hrefValue = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);

                //Check if Url is complete.
                if (CheckValidUrl(hrefValue.Groups[1].Value))
                {
                    linkItemObj.Href = hrefValue.Groups[1].Value;
                }
                else
                {
                    linkItemObj.Href = sUrl + hrefValue.Groups[1].Value;
                }

                // Remove inner tags from text.
                string hrefText = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
                linkItemObj.Text = hrefText.ToLower();



                //foreach (var x in requiredKeywoard)
                //{
                //    if (linkItemObj.Text.Contains(x.word.ToString().ToLower()))
                //    {

                //        anchorList.Add(linkItemObj.Href);

                //    }
                //}

            }

            //At this point we have all the href tags in anchorList of type string

            List<string> json = new List<string>();
            List<string> selectedKeywords = new List<string>();
            List<string> distinctAnchorList = new List<string>();
            distinctAnchorList = anchorList.Distinct().ToList();
            List<string> headerValue = new List<string>();
            string file2 = "";
            foreach (string hrefValue in distinctAnchorList)
            {

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("Accept-Language", " en-US");
                        client.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
                        client.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                        file2 = client.DownloadString(hrefValue);
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                    {
                        var resp = (HttpWebResponse)ex.Response;
                        if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                        {
                            //Handle it
                        }
                    }
                    //Handle it
                }
                // Find all matches in file.
                String h1Regex = "<h[1-6][^>]*?>(?<TagText>.*?)</h[1-6]>";
                MatchCollection matchedTextHeaders = Regex.Matches(file2, h1Regex, RegexOptions.Singleline);
                // Loop over each match.
                foreach (Match m in matchedTextHeaders)
                {
                    string value = m.Groups[1].Value;
                    // Remove inner tags from text.
                    string t = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
                    string withoutSpecial = Regex.Replace(t, @"\r\n\t?/|\n", "").Replace("&", "").Replace(";", "").Replace("#", "").Replace("  ", " ").Replace("nbsp","");
                    if(withoutSpecial.Length > 17)
                    {
                        headerValue.Add(withoutSpecial);
                    }
                    
                }

            }


            string selectedkeyword = "";
            foreach (excludedWord ex in restrictedKeywords)
            {
                selectedkeyword = ex.word.ToString();
                if (!string.IsNullOrEmpty(selectedkeyword))
                {
                    headerValue.RemoveAll(item => item.Contains(selectedkeyword));
                    headerValue.RemoveAll(item => item.Contains(selectedkeyword.ToLower()));
                    headerValue.RemoveAll(item => item.Contains(selectedkeyword.ToUpper()));
                    headerValue.RemoveAll(item => item.Contains(UppercaseFirst(selectedkeyword)));
                }
            }

            //return passedUrl;
            var distinct = headerValue.Distinct();
            string valueResult = string.Join("|", distinct.ToArray());
            //var seri = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string jsonResult = seri.Serialize(distinct);
            controller.UpdateCrawlDataByPlaceId(valueResult, placeId);
            return valueResult;
        }
        //Fetching Json from app


        public class Result
        {

            public string website { get; set; }
        }

        public class RootObject
        {
            public List<object> html_attributions { get; set; }
            public Result result { get; set; }
            public string status { get; set; }
        }
        class InputJson
        {
            public string DeviceID { get; set; }
            public string device_lat { get; set; }
            public string device_lng { get; set; }
            public string google_browserKey { get; set; }
            public string Type { get; set; }
            public List<Result> ListPlaces { get; set; }

            public class Result
            {
                public string PlaceId { get; set; }
                public string Name { get; set; }
                public string address { get; set; }
                public string place_lat { get; set; }
                public string place_lng { get; set; }
            };
        }
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}