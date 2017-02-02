using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Utilities;
using WebCrawler.DataCore;
using WebCrawler.DataCore.Managers;
using PushSharp.Apple;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using WebCrawler.Models;
using System.Device;
using System.Device.Location;

namespace WebCrawler.Controllers
{
    public class VendorDashboardController : ApiController
    {
        private static BusinessCoreController controller = new BusinessCoreController();

        [HttpPost]
        public HttpResponseMessage CreateAd([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            int adId = 0;
            List<UserInfo> allUsers = new List<UserInfo>();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                var attrs = info.locationName.Split('|');
                info.locationName = attrs[0];
                var placeInfo = GetPlaceInfo(info.lati, info.longi);

               

                //var placeInf = GetPlaceInfo(attrs[1], info.lati, info.longi);
                 info.sponsorFacts = placeInfo;
                  adId = controller.CreateVendorAd(info);


                var addmanager = new AddManager();

                // To GReySQL
                addmanager.InsertAdInfo(adId, info.lati, info.longi, info.locationName);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, adId);
            return response;
        
        }
        public string GetPlaceInfo(string orgLati, string orgLongi)
        {
            var adCord = new GeoCoordinate(Convert.ToDouble(orgLati), Convert.ToDouble(orgLongi));
            using (var client = new WebClient())
            {               
                var response = client.DownloadString(string.Format("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location="+orgLati+","+orgLongi+"&radius=100&key=AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8"));
                var data=JsonConvert.DeserializeObject<GoogleData>(response);
                foreach (var obj in data.results)
                {
                    var tempMap = obj.geometry.location;
                    var newCoord = new GeoCoordinate(tempMap.lat, tempMap.lng);
                    var distance= adCord.GetDistanceTo(newCoord);
                    obj.distance = (int?) distance;
                }
                var newResult = data.results.OrderBy(obj => obj.distance);
                var firstObj = newResult.FirstOrDefault();
                if (firstObj != null)
                {
                    using (var newClient = new HttpClient())
                    {
                        var uri = firstObj.place_id;
                        var response1 = newClient.GetStringAsync(string.Format("https://maps.googleapis.com/maps/api/place/details/json?placeid=" + uri + "&key=AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8&"));
                        var placeInfo = response1.Result;
                        var newData = JsonConvert.DeserializeObject<NewRootObject>(placeInfo);
                        if (newData.result != null)
                        {
                            newData.result.adr_address = "";
                            newData.result.photos = new List<Photo> { new Photo() };

                        }
                        //    var fullTundata = JsonConvert.SerializeObject(newData.result);
                        //   return fullTundata;


                        // orginal
                        int pFrom = placeInfo.IndexOf("adr_address");
                        int pTo = placeInfo.IndexOf("formatted_address");
                        var result = placeInfo.Substring(pFrom - 1, pTo - pFrom - 3);
                        var refinedInfo = placeInfo.Remove(pFrom, result.Length + 3);
                        return refinedInfo;
                        // orginal ends

                    }                    
                }
            }
            return null;
        }

        [HttpPost]
        public HttpResponseMessage GetAdByVendorId([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            List<AdInfo> adList = new List<AdInfo>();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                adList = controller.GetAdByVendorId(info.vendorId.ToString());


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
            
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, adList);
            return response;

        }

        [HttpPost]
        public HttpResponseMessage GetAllVendorAds([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            List<AdInfo> adList = new List<AdInfo>();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                adList = controller.GetAllAds();


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, adList);
            return response;

        }



        [HttpPost]
        public HttpResponseMessage RemoveAd([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            List<AdInfo> adList = new List<AdInfo>();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                adList = controller.RemoveAd(info.Id.ToString(), info.vendorId.ToString());


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, adList);
            return response;

        }


        [HttpPost]
        public HttpResponseMessage UpdateAdVisibility([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();
            List<AdInfo> adList = new List<AdInfo>();
            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                adList = controller.UpdateAdVisibility(info.isVisible.ToString(), info.vendorId.ToString(), info.Id.ToString());


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, adList);
            return response;

        }


        [HttpPost]
        public HttpResponseMessage UpdateAd([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                info = controller.UpdateAd(info);


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, info);
            return response;

        }




        [HttpPost]
        public HttpResponseMessage GetAdById([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                info = controller.GetAdById(info.Id.ToString());


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, info);
            return response;

        }


        [HttpPost]
        public HttpResponseMessage UpdateImage([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                info = controller.UpdateAdImage(info.mapImage.ToString(), info.Id.ToString());


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, info);
            return response;

        }



        [HttpPost]
        public HttpResponseMessage UpdateAdVideo([FromBody]JObject data)
        {
            string results = "";

            AdInfo info = new AdInfo();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                info = controller.UpdateAdVideo(info.mapVideo.ToString(), info.Id.ToString());


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, info);
            return response;

        }


        [HttpPost]
        public void UploadFile()
        {
           // if (HttpContext.Current.Request.Files.AllKeys.Any())
         //   {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

           //     if (httpPostedFile != null)
           //     {
                    // Validate the uploaded image(optional)

                    // Get the complete file path
                    string objId = HttpContext.Current.Request.Params.GetValues("adId")[0];
                    string videoUrl = HttpContext.Current.Request.Params.GetValues("videoUrl")[0];
                    string isVideo = HttpContext.Current.Request.Params.GetValues("isVideo")[0];
                    string isImg = HttpContext.Current.Request.Params.GetValues("isImg")[0];

                    if (bool.Parse(isVideo))
                    {
                        controller.UpdateAdVideo(videoUrl, objId);
                    }

                    if (bool.Parse(isImg))
                    {
                      string fileName = objId + "" + httpPostedFile.FileName;
                      var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles/AdImages"), fileName);
                        controller.UpdateAdImage(fileName, objId);
                        httpPostedFile.SaveAs(fileSavePath);
                    }
            //    }
          //  }
        }

        [HttpPost]
        public HttpResponseMessage GetAllAdTypes([FromBody]JObject data)
        {
            DataTable obj = new DataTable();
            obj = controller.GetAllAdTypes();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;

        }


        [HttpPost]
        public HttpResponseMessage GetAllBidTypes([FromBody]JObject data)
        {
            DataTable obj = new DataTable();
            obj = controller.GetAllBidTypes();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;

        }

        [HttpPost]
        public HttpResponseMessage PayNow([FromBody]JObject data)
        {
            string results = "";

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                var temp = new JavaScriptSerializer().Deserialize<AdInfo>(results);
                controller.PayDuePayment(temp.vendorId);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;

        }


    }

    
}