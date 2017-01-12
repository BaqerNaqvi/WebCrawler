using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebCrawler.Views
{
    public partial class GCMNotifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_send_Click(object sender, EventArgs e)
        {
            //RegisterId you got from Android Developer.
            string deviceId = "cZcoF5vd8qE:APA91bG4mvOXOyjI6hV0n2TabsgI4gxnYESyhiYAyrx378aH1-9KMjcw2Yd05LSYcDsOeg2TfzKg7_KWdr0Ey4HknA98f68vKIzyg7LUzMORNEcxCdRWQP-OZFbr_iYmO16N-uk_i3mI";

            //string message = "Demo Notification";
            string tickerText = "Patient Registration";
            string contentTitle = "hello title";
            string postData =
            "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
              "\"data\": {\"tickerText\":\"" + tickerText + "\", " + "\"contentTitle\":\"" + contentTitle + "\", " + "\"notification_data\": \"{\"T\":\"2\",\"alert\":\"I want to meet you to discuss few points\",\"Title\":\"Meeting Request\",\"meetingID\":\"21\",\"RId\":\"130\",\"SId\":\"145\",\"sName\":\"irfan\"}\"}}";

            string response = SendGCMNotification("AIzaSyCazZ5up3Iv7vd199Sg2IHd4Ud0kGbo0C4", postData);


            res.Text = response;

        }

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


        public static bool ValidateServerCertificate(
                                                     object sender,
                                                     X509Certificate certificate,
                                                     X509Chain chain,
                                                     SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}