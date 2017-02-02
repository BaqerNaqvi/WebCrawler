using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using WebCrawler.DataCore.Managers;
using WebCrawler.Models;
using Newtonsoft.Json;

namespace WebCrawler.Controllers.api
{
    public class GetAdsController : ApiController
    {
       
        [HttpPost]
        public HttpResponseMessage GetAd([FromBody] JObject data)
        {
            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Bad Request!");
                return error;
            }
            try
            {
                var results = data.ToString(Formatting.None);
                GetAddsAPiRequestModel request = new JavaScriptSerializer().Deserialize<GetAddsAPiRequestModel>(results);

                var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);


                var addmanager = new AddManager();
                List<AdInfoContainer> ads = addmanager.GetAdss(new GetAddsAPiRequestModel
                { location_lat = request.location_lat, location_lng = request.location_lng, user_id = request.user_id }, baseUrl);



                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new GetAddsResponseModel
                {
                    ads = ads,
                    message = "success",
                    status = "ok"
                });
                return response;

            }
            catch (WebException ex)
            {
               
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.InternalServerError, "Something happened!");
                return error;
                //Handle it
            }
        }
    }

    #region utilities
    public class Review
    {
        public string text { get; set; }
        public double rating { get; set; }
    }

    public class Photo
    {
        public string photo_reference { get; set; }
    }

    public class PlaceInfo
    {
        public string name { get; set; }
        public int type { get; set; }
        public double location_lat { get; set; }
        public double location_lng { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public string icon { get; set; }
        public double rating { get; set; }
        public string place_id { get; set; }
        public List<Review> reviews { get; set; }
        public string url { get; set; }
        public string website { get; set; }
        public List<Photo> photos { get; set; }
    }

    public class AdInfo
    {
        public int type { get; set; }
        public string text { get; set; }
        public string coupon_url { get; set; }
        public string symbol_url { get; set; }
        public string advertiser_logo { get; set; }
        public string advertiser_tagline { get; set; }
        public string advertiser_url { get; set; }
        public string snapshot_url { get; set; }
        public int? landmark_type { get; set; }
        public string landmark_detail_url { get; set; }
    }

    public class Ad
    {
        public PlaceInfo place_info { get; set; }
        public AdInfo ad_info { get; set; }
    }

    public class RootObject
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Ad> ads { get; set; }
    }
    #endregion

}