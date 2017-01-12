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
    public partial class PushNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string deivceId = "cZcoF5vd8qE:APA91bG4mvOXOyjI6hV0n2TabsgI4gxnYESyhiYAyrx378aH1-9KMjcw2Yd05LSYcDsOeg2TfzKg7_KWdr0Ey4HknA98f68vKIzyg7LUzMORNEcxCdRWQP-OZFbr_iYmO16N-uk_i3mI";
            string strResponse =
            SendNotification(deivceId,
            "Test Push Notification message ");
        }
        public string SendNotification(string deviceId, string message)
        {
            string GoogleAppID = "AIzaSyCazZ5up3Iv7vd199Sg2IHd4Ud0kGbo0C4";
            var SENDER_ID = "dDu1mXzLkL8";
            var value = message;
            WebRequest tRequest;
            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" +
            System.DateTime.Now.ToString() + "®istration_id=" + deviceId + "";
            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }




    }
}