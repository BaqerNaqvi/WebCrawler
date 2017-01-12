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
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Utilities;
using WebCrawler.DataCore;
using WebCrawler.DataCore.Managers;

using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace WebCrawler.Controllers
{
    public class locationController : ApiController
    {
        private static BusinessCoreController controller = new BusinessCoreController();
        public static string apiKey = "AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8";
       
        //Meeting notes
        [HttpPost]
        public HttpResponseMessage AddMeetingNote([FromBody]JObject data)
        {
            string results = "";
            meetingNote info = new meetingNote();
            DataTable meetingNote = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<meetingNote>(results);
                meetingNote = controller.AddMeetingNote(info);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }



            




            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (meetingNote.Rows.Count > 0)
            {

                //Send push notification 


                DataTable userDeviceAndOrigin = controller.GetGcmTokenByUserId(meetingNote.Rows[0].ItemArray[10].ToString());
                string deviceId = "asdfasdlj";
                string requestOrigin = "iPhone";
                if(userDeviceAndOrigin.Rows.Count > 0)
                {
                    deviceId = userDeviceAndOrigin.Rows[0].ItemArray[1].ToString();
                    requestOrigin = userDeviceAndOrigin.Rows[0].ItemArray[0].ToString();
                }
                string notification_data = "Demo Notification";

                GCMOutput mp = new GCMOutput();
                mp.T = 2;
                mp.meetingID = int.Parse(meetingNote.Rows[0].ItemArray[0].ToString());
                mp.alert = meetingNote.Rows[0].ItemArray[1].ToString();
                mp.Title = "Meeting Request";
                mp.RId = int.Parse(meetingNote.Rows[0].ItemArray[10].ToString());
                mp.SId = int.Parse(meetingNote.Rows[0].ItemArray[8].ToString());
                mp.sName = meetingNote.Rows[0].ItemArray[9].ToString();

                HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.OK, mp);
                notification_data = new JavaScriptSerializer().Serialize(mp);

                string postData =
                "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
                  "\"data\": {\"notification_data\": [{\"T\":\"2\",\"alert\":\""+mp.alert+"\",\"Title\":\"Meeting Request\",\"meetingID\":\""+mp.meetingID+"\",\"RId\":\""+mp.RId+"\",\"SId\":\""+mp.SId+"\",\"sName\":\""+mp.sName+"\"}]}}";

                if (deviceId.Length > 64)
                {
                    string response1 = SendGCMNotification(apiKey, postData);
                }
                else{
                    controller.SendIPhoneMeetingPushNotfication(info.sender_userId.ToString(), info.receiver_userId.ToString(), "Meeting Request", meetingNote.Rows[0].ItemArray[0].ToString(), info.sender_name, deviceId, info.lati, info.longi, info.purpose, info.address, info.meetingDate, info.meetingTime, info.allowReminder.ToString(), info.receiver_name);
                }
                
                //End push notification here

                jsObj.status = "Ok";
                jsObj.result = meetingNote;
                jsObj.error_message = "Successfully Added !";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.error_message = "Failed. Something went wrong";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage UpdateMeetingNote([FromBody]JObject data)
        {
            string results = "";
            meetingNote info = new meetingNote();
            DataTable meetingNote = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<meetingNote>(results);
                meetingNote = controller.UpdateMeetingNote(info);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }


            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (meetingNote.Rows.Count > 0)
            {

                jsObj.status = "Ok";
                jsObj.result = meetingNote;
                jsObj.error_message = "Successfully Updated !";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.error_message = "Failed. Something went wrong";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }
        [HttpPost]
        public HttpResponseMessage FetchMeetingDetail([FromBody]JObject data)
        {
            string results = "";
            DataTable meetingNote = new DataTable();
            JsonMeetingNoteInput info = new JsonMeetingNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonMeetingNoteInput>(results);
                meetingNote = controller.GetMeetingNotes(info.meetingId,int.Parse(info.userId.ToString()));
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (meetingNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = meetingNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage FetchUserMeetings([FromBody]JObject data)
        {
            string results = "";
            DataTable meetingNote = new DataTable();
            JsonMeetingNoteInput info = new JsonMeetingNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonMeetingNoteInput>(results);
                meetingNote = controller.GetAllMeetingNotes(info.meetingId, int.Parse(info.userId.ToString()));
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (meetingNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = meetingNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage FetchUserMeetingAsReceiver([FromBody]JObject data)
        {
            string results = "";
            DataTable meetingNote = new DataTable();
            JsonMeetingNoteInput info = new JsonMeetingNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonMeetingNoteInput>(results);
                meetingNote = controller.GetAllMeetingNotesAsReceiver(info.meetingId, int.Parse(info.userId.ToString()));
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (meetingNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = meetingNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        //Delete meeting 

        [HttpPost]
        public HttpResponseMessage DeleteMeetingNote([FromBody]JObject data)
        {
            string results = "";
            DataTable meetingNote = new DataTable();
            JsonMeetingNoteInput info = new JsonMeetingNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonMeetingNoteInput>(results);
                meetingNote = controller.DeleteMeetingNote(info.meetingId, int.Parse(info.userId.ToString()));
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (meetingNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = meetingNote;
                jsObj.error_message = "Deleted Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Deleted Successfully!";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        //GPS locations

        [HttpPost]
        public HttpResponseMessage AddGpsNote([FromBody]JObject data)
        {
            string results = "";
            gpsNote info = new gpsNote();
            DataTable gpsNotes = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<gpsNote>(results);
                gpsNotes = controller.AddGpsNote(info);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNotes.Rows.Count > 0)
            {

                //Send push notification 


                DataTable userDeviceAndOrigin = controller.GetGcmTokenByUserId(gpsNotes.Rows[0].ItemArray[9].ToString());
                string deviceId = "asdfasdlj";
                string requestOrigin = "iPhone";
                if (userDeviceAndOrigin.Rows.Count > 0)
                {
                    deviceId = userDeviceAndOrigin.Rows[0].ItemArray[1].ToString();
                    requestOrigin = userDeviceAndOrigin.Rows[0].ItemArray[0].ToString();
                }
                string notification_data = "Demo Notification";

                GPSOutput mp = new GPSOutput();
                mp.T = 1;
                mp.alert = gpsNotes.Rows[0].ItemArray[1].ToString();
                mp.noteId = gpsNotes.Rows[0].ItemArray[0].ToString();
                mp.Title = "Qlumi Location Note";
                mp.RId = int.Parse(gpsNotes.Rows[0].ItemArray[9].ToString());
                mp.SId = int.Parse(gpsNotes.Rows[0].ItemArray[7].ToString());
                mp.sName = gpsNotes.Rows[0].ItemArray[8].ToString();

                HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.OK, mp);
                notification_data = new JavaScriptSerializer().Serialize(mp);

                string postData =
                "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
                  "\"data\": {\"notification_data\": [{\"T\":\"1\",\"alert\":\"" + mp.alert + "\",\"noteId\":\"" + mp.noteId + "\",\"Title\":\"" + mp.Title + "\",\"RId\":\"" + mp.RId + "\",\"SId\":\"" + mp.SId + "\",\"sName\":\"" + mp.sName + "\"}]}}";


                string res = "";
                if (deviceId.Length > 64)
                {
                    res = SendGCMNotification(apiKey, postData);
                }
                else
                {
                    res = controller.SendIPhoneNotePushNotfication(info.sender_userId.ToString(), info.receiver_userId.ToString(), info.title, info.description, gpsNotes.Rows[0].ItemArray[0].ToString(), info.sender_name, deviceId, info.lati, info.longi, info.address, info.color);
                }
                

                
                //End push notification here

                jsObj.status = "Ok";
                jsObj.result = gpsNotes;
                jsObj.error_message = res;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.result = new DataTable();
                jsObj.error_message = "Failed. GPS note not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage AddUserGpsNote([FromBody]JObject data)
        {
            string results = "";
            UserGpsNote info = new UserGpsNote();
            DataTable gpsNotes = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserGpsNote>(results);
                gpsNotes = controller.AddUserGpsNote(info);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNotes.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNotes;
                jsObj.error_message = "Success!";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.result = new DataTable();
                jsObj.error_message = "Failed. GPS note not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage UpdateUserGpsNote([FromBody]JObject data)
        {
            string results = "";
            UserGpsNote info = new UserGpsNote();
            DataTable gpsNotes = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserGpsNote>(results);
                gpsNotes = controller.UpdateUserNote(info);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNotes.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNotes;
                jsObj.error_message = "Updated!";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.result = new DataTable();
                jsObj.error_message = "Failed. GPS note not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage BookmarkedGpsNotesByUserId([FromBody]JObject data)
        {
            string results = "";
            BookmarkInput info = new BookmarkInput();
            DataTable gpsNotes = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<BookmarkInput>(results);
                gpsNotes = controller.BookmarkedGpsNotesByUserId(info.userId, info.noteId, info.isBookmark);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNotes.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNotes;
                jsObj.error_message = "Bookmarked!";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.result = new DataTable();
                jsObj.error_message = "Failed. GPS note not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage FetchBookmarkedGpsNotesByUserId([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            JsonGpsNoteInput info = new JsonGpsNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonGpsNoteInput>(results);
                gpsNote = controller.GetUserBookmarkedGpsNotesByUserId(info.userId);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage FetchGpsDetail([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            JsonGpsNoteInput info = new JsonGpsNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonGpsNoteInput>(results);
                gpsNote = controller.GetGpsNotes(info.noteId, info.userId);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage FetchGpsDetailByUserId([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            JsonGpsNoteInput info = new JsonGpsNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonGpsNoteInput>(results);
                gpsNote = controller.GetUserGpsNotesByUserId(info.userId);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage DeleteGpsNoteByUserId([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            JsonGpsNoteInput info = new JsonGpsNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonGpsNoteInput>(results);
                gpsNote = controller.DeleteUserGpsNotesByUserId(info.userId);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            jsObj.status = "Ok";
            jsObj.result = gpsNote;
            jsObj.error_message = "Successfully!";

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
            return response;

        }

        [HttpPost]
        public HttpResponseMessage DeleteGpsNoteByNoteId([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            JsonGpsNoteInput info = new JsonGpsNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonGpsNoteInput>(results);
                gpsNote = controller.DeleteUserGpsNotesByNoteId(info.noteId, info.userId);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            jsObj.status = "Ok";
            jsObj.result = gpsNote;
            jsObj.error_message = "Successfully!";

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
            return response;

        }



        [HttpPost]
        public HttpResponseMessage UpdateUserLocation([FromBody]JObject data)
        {
            string results = "";
            JsonLocationInput info = new JsonLocationInput();
            DataTable location = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonLocationInput>(results);
                location = controller.UpdateUserLocation(info.userId, info.lati, info.longi);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonLocationOutput jsObj = new JsonLocationOutput();
            if (location.Rows.Count > 0)
            {
                jsObj.status = "Ok";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        //Send Push notification
        private string SendGCMNotification(string apiKey, string postData, string postDataContentType = "application/json")
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //  
            //  MESSAGE CONTENT  
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //  
            //  CREATE REQUEST  
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            //  Request.KeepAlive = false;  

            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            Request.ContentLength = byteArray.Length;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //  
            //  SEND MESSAGE  
            try
            {
                WebResponse Response = Request.GetResponse();

                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    var text = "Unauthorized - need new token";
                }
                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    var text = "Response from web service isn't OK";
                }

                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Reader.Close();

                return responseLine;
            }
            catch (Exception e)
            {
            }
            return "error";
        }

        #region vendorNotes
        [HttpPost]
        public HttpResponseMessage AddVendorGpsNote([FromBody]JObject data)
        {
            string results = "";
            VendorNote info = new VendorNote();
            DataTable gpsNotes = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<VendorNote>(results);
                gpsNotes = controller.AddVendorGpsNote(info);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNotes.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNotes;
                jsObj.error_message = "Success!";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Failed";
                jsObj.result = new DataTable();
                jsObj.error_message = "Failed. GPS note not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage FetchVendorNotesById([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            JsonGpsNoteInput info = new JsonGpsNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonGpsNoteInput>(results);
                gpsNote = controller.GetVendorGpsNotesById(info.userId);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage SendAllVendorNotes([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            VendorNoteInput info = new VendorNoteInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<VendorNoteInput>(results);
                gpsNote = controller.SendAllVendorNotes(info.vendorId, info.unlock_code);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNote;
                jsObj.error_message = "Successfully!";

                //Send push notification 


                DataTable userDeviceAndOrigin = controller.GetAllUsersByUnlockCode(info.unlock_code.ToString());
                string deviceId = "asdfasdlj";
                string requestOrigin = "iPhone";
                if (userDeviceAndOrigin.Rows.Count > 0)
                {
                    string notification_data = "Demo Notification";

                    GPSOutput mp = new GPSOutput();
                    mp.T = 4;
                    mp.alert = "";// gpsNotes.Rows[0].ItemArray[1].ToString();
                    mp.noteId = "";//gpsNotes.Rows[0].ItemArray[0].ToString();
                    mp.Title = "Qlumi Location Note";
                    mp.RId = 0;//int.Parse(gpsNotes.Rows[0].ItemArray[9].ToString());
                    mp.SId = 0;//int.Parse(gpsNotes.Rows[0].ItemArray[7].ToString());
                    mp.sName = "";//gpsNotes.Rows[0].ItemArray[8].ToString();

                    HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.OK, mp);
                    notification_data = new JavaScriptSerializer().Serialize(mp);

                    string postData =
                    "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
                      "\"data\": {\"notification_data\": [{\"T\":\"4\",\"alert\":\"" + mp.alert + "\",\"noteId\":\"" + mp.noteId + "\",\"Title\":\"" + mp.Title + "\",\"RId\":\"" + mp.RId + "\",\"SId\":\"" + mp.SId + "\",\"sName\":\"" + mp.sName + "\"}]}}";




                    foreach(DataRow row in userDeviceAndOrigin.Rows)
                    {
                        deviceId = row[0].ToString();
                        requestOrigin = row[1].ToString();
                        string res = "";
                        if (deviceId.Length > 64)
                        {
                            res = SendGCMNotification(apiKey, postData);
                        }
                        else
                        {
                            res = controller.SendIPhoneNotePushNotfication("", "", "", "", "", "", deviceId, "", "", "", "");
                        }
                    }
                    
                }
                

               



                //End push notification here

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage FetchVendorNotesByCoord([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            VendorInput info = new VendorInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<VendorInput>(results);
                gpsNote = controller.GetVendorGpsNotesByCoord(info.vendorId, info.lati, info.longi);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage DeleteVendorNotesByCoord([FromBody]JObject data)
        {
            string results = "";
            DataTable gpsNote = new DataTable();
            VendorInput info = new VendorInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<VendorInput>(results);
                gpsNote = controller.DeleteVendorGpsNotesByCoord(info.vendorId, info.lati, info.longi);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "Invalid Request");
                return error;
                //Handle it
            }
            JsonMeetingNoteResponse jsObj = new JsonMeetingNoteResponse();
            if (gpsNote.Rows.Count > 0)
            {
                jsObj.status = "Ok";
                jsObj.result = gpsNote;
                jsObj.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }
            else
            {
                jsObj.status = "Ok";
                jsObj.result = new DataTable();
                jsObj.error_message = "Notes not found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, jsObj);
                return response;
            }

        }

        #endregion 

        public static bool ValidateServerCertificate(
                                                     object sender,
                                                     X509Certificate certificate,
                                                     X509Chain chain,
                                                     SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }

    public class JsonMeetingNoteResponse
    {
        public string status { get; set; }
        public DataTable result { get; set; }
        public string error_message { get; set; }


        
    }
    public class JsonMeetingNoteInput
    {
        public string meetingId { get; set; }
        public string userId { get; set; }

    }

    public class JsonGpsNoteInput
    {
        public string noteId { get; set; }
        public string userId { get; set; }

    }

    public class BookmarkInput
    {
        public string noteId { get; set; }
        public string userId { get; set; }
        public string isBookmark { get; set; }

    }

    public class JsonLocationInput
    {
        public string userId { get; set; }
        public string lati { get; set; }
        public string longi { get; set; }

    }
    public class VendorInput
    {
        public string vendorId { get; set; }
        public string lati { get; set; }
        public string longi { get; set; }

    }

    public class VendorNoteInput
    {
        public string vendorId { get; set; }
        public string unlock_code { get; set; }

    }
    public class JsonLocationOutput
    {
        public string status { get; set; }

    }


    public class GCMOutput
    {
        public int T { get; set; }
        public string alert { get; set; }
        public string Title { get; set; }
        public int meetingID { get; set; }
        public int RId { get; set; }
        public int SId { get; set; }
        public string sName { get; set; }
    }

    public class GPSOutput
    {
        public int T { get; set; }
        public string noteId { get; set; }
        public string alert { get; set; }
        public string Title { get; set; }
        public int RId { get; set; }
        public int SId { get; set; }
        public string sName { get; set; }
    }


    public class ChatMessageOutput
    {
        public int T { get; set; }
        public string alert { get; set; }
        public string Title { get; set; }
        public int rId { get; set; }
        public int sId { get; set; }
        public string createAt { get; set; }
    }
    
}