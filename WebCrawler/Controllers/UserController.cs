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


namespace WebCrawler.Controllers
{
    public class UserController : ApiController
    {
        private static BusinessCoreController controller = new BusinessCoreController();
        public static string apiKey = "AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8";
        // GET api/<controller>
        public int Get()
        {


            return 0;
        }
        // POST api/<controller>
        public string Post([FromBody]admin admin)
        {

            EncryptPwd encryption = new EncryptPwd();
            admin ad = new admin();
            string status = admin.name.ToString().Trim();

            if (status == "login")
            {
                string username = admin.email.Trim();
                string password = admin.password.Trim();
                string encpassword = encryption.EncryptToString(password);

                if (username.Length > 0 && encpassword.Length > 0)
                {
                    BusinessCoreController controller = new BusinessCoreController();
                    ad = controller.AuthenticateUser(username, password);
                    if (ad.Id != 0)
                    {
                        controller.LoginUser(ad.Id);

                        var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                        return sr.Serialize(ad);
                    }
                    else
                    {
                        return "0";
                    }

                }
                else
                {
                    return "0";
                }

            }
            if (status == "logout")
            {
                string username = admin.email.Trim();
                string password = admin.password.Trim();
                string encpassword = encryption.EncryptToString(password);

                if (username.Length > 0 && encpassword.Length > 0)
                {
                    BusinessCoreController controller = new BusinessCoreController();
                    ad = controller.AuthenticateUser(username, password);
                    if (ad.Id != 0)
                    {
                        controller.LoginUser(ad.Id);

                        var sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                        return sr.Serialize(ad);
                    }
                    else
                    {
                        return "0";
                    }

                }
                else
                {
                    return "0";
                }

            }
            else if (status == "forget")
            {
                BusinessCoreController controller = new BusinessCoreController();
                string email = admin.email;
                string username = admin.username;
                int check = controller.CheckUserEmail(email,username);
                if (check > 0)
                {
                    string rand = GetRandomPasswordUsingGUID(2);
                    EncryptPwd encryptions = new EncryptPwd();
                    string encryptedpassword = encryptions.EncryptToString(rand);
                    controller.UpdatePasswordByEmail(email, encryptedpassword);


                    Sendmail(rand);
                    

                    return "1";


                }
                else
                {


                    return "0";
                }

            }
            else
            {
                BusinessCoreController controller = new BusinessCoreController();
                string email = admin.email.Trim();
                string name = admin.name;
                string username = admin.username;


                string rand = admin.password;
                EncryptPwd encryptions = new EncryptPwd();
                string encryptedpassword = encryptions.EncryptToString(rand);

                admin ads = new admin();
                ads.email = email;
                ads.name = name;
                ads.username = username;
                ads.password = encryptedpassword;
                int adsId = controller.RegisterAdmin(ads);


                //Sendmail(rand);


                return adsId.ToString();

            }




        }
        [HttpPost]
        public HttpResponseMessage RegisterUser([FromBody]JObject data)
        {
            string results = "";
            int uId = 0;
            string message = "Signup Failed";

            UserInfo info = new UserInfo();
            

            List<UserInfo> allUsers = new List<UserInfo>();

            if(data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserInfo>(results);
                uId = controller.RegisterAppUser(info);
                if(uId != 0)
                {
                    message = uId.ToString();
                    info = controller.GetUserByIdWithNoCode(uId);

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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
                //Handle it
            }
            JsonResponseSignup js = new JsonResponseSignup();
            if(uId == 2)
            {
                js.status = "Failed";
                js.userId = "0";
                js.qlumi_userId = "0";
                js.error_message = "User with this contact number already exists!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else if (uId == 3)
            {
                js.status = "Failed";
                js.userId = "0";
                js.qlumi_userId = "0";
                js.error_message = "User with this email address already exists!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else if (uId == 0)
            {
                js.status = "Failed";
                js.userId = "0";
                js.qlumi_userId = "0";
                js.error_message = "Registration Failed. Something went wrong!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else             
            {
                js.status = "Ok";
                js.userId = uId.ToString();
                js.qlumi_userId = info.qlumi_userId;
                js.error_message = "Successfully Registered !";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            
        }

        [HttpPost]
        public HttpResponseMessage UpdateUserInfo([FromBody]JObject data)
        {
            string results = "";

            UserInfo info = new UserInfo();


            List<UserInfo> allUsers = new List<UserInfo>();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserInfo>(results);
                controller.UpdateUserInfo(info);
                

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

            JsonResponseSignup js = new JsonResponseSignup();
            js.status = "";
            js.userId = "";
            js.qlumi_userId = "";
            js.error_message = "User Information udpated!";

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            return response;

        }

        [HttpPost]
        public HttpResponseMessage SearchUserInfo([FromBody]JObject data)
        {
            string results = "";

            UserInfo info = new UserInfo();


            DataTable allUsers = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserInfo>(results);
                allUsers = controller.GetSearchResult(info);


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, allUsers);
            return response;

        }



        [HttpPost]
        public HttpResponseMessage SearchVendorUserInfo([FromBody]JObject data)
        {
            string results = "";

            UserInfo info = new UserInfo();


            DataTable allUsers = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserInfo>(results);
                allUsers = controller.GetVendorSearchResult(info);


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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, allUsers);
            return response;

        }

        [HttpPost]
        public HttpResponseMessage SyncUserInfo([FromBody]JObject data)
        {
            string results = "";

            SyncRootObject info = new SyncRootObject();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<SyncRootObject>(results);

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle its
                    }
                }
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
                //Handle it
            }

            controller.SyncProfileInformation(info.Id.ToString(), results, info.updatedAt);

            if(info.isLogoutRequest)
            {
                controller.UpdateGcmToken(info.Id.ToString(), "");
            }
            
           
            JsonResponse js = new JsonResponse();
            js.status = "Ok";
            js.result = null;
            js.error_message = "Profile Successfully Updated!";
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage LogOut([FromBody]JObject data)
        {
            string results = "";

            RenewUnlockInput info = new RenewUnlockInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<RenewUnlockInput>(results);

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle its
                    }
                }
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
                //Handle it
            }

            controller.UpdateGcmToken(info.userId.ToString(), "");


            JsonResponse js = new JsonResponse();
            js.status = "Ok";
            js.result = null;
            js.error_message = "Logged Out!";
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            return response;
        }


        [HttpPost]
        public HttpResponseMessage VerifyUnlockCode([FromBody]JObject data)
        {


            string results = "";

            UserInfo info = new UserInfo();
            Vendor vendor = new Vendor();
            vendor.description = "Pending..";

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserInfo>(results);

                vendor = controller.GetVendorByUnlockCode(info.unlock_code, info.Id);


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

            JsonResponse js = new JsonResponse();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            if(vendor.description == "1")
            {
                UserInfo us = controller.GetUserById(info.Id);
                
                if (info.Id != 0)
                {
                    js.status = "Ok";
                    js.isUsingValideUnlockCode = 1;
                    js.result = us;
                    js.error_message = "Success";
                    return response;
                }
                else
                {
                    js.status = "Failed";
                    js.isUsingValideUnlockCode = 1;
                    js.result = us;
                    js.error_message = "Invalid User Credentials";
                    return response;
                }


            }
            else if (vendor.description == "2")
            {

                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.result = null;
                js.error_message = "Code limit reached";
                return response;
                    

            }
            else if (vendor.description == "0")
            {
                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.result = null;
                js.error_message = "Invalid Unlock Code";
                return response;
            }
            else
            {
                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.result = null;
                js.error_message = "Invalid Request";
                return response;
            }
            //success
            //limit reach
            //invalid

            
            
        }
        [HttpPost]
        public HttpResponseMessage UpdatePassword([FromBody]JObject data)
        {


            string results = "";
            int userId = 0;
            ResetPasswordInput info = new ResetPasswordInput();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<ResetPasswordInput>(results);
                userId = controller.UpdatePasswordByResetCode(info.newPassword, info.resetCode);


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

            JsonResponse js = new JsonResponse();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            if (userId != 0)
            {
                UserInfo us = controller.GetUserById(userId);
                js.status = "Ok";
                js.isUsingValideUnlockCode = 1;
                js.result = us;
                js.error_message = "Password updated successfully !";
                return response;

            }
            else
            {
                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.result = null;
                js.error_message = "Invalid Reset Code";
                return response;
            }
            //success
            //limit reach
            //invalid



        }
        [HttpPost]
        public HttpResponseMessage RenewUnlockCode([FromBody]JObject data)
        {


            string results = "";

            RenewUnlockInput info = new RenewUnlockInput();
            Vendor vendor = new Vendor();
            vendor.description = "Pending..";
            int uId = 0;

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<RenewUnlockInput>(results);
                uId = int.Parse(info.userId);
                vendor = controller.RenewUnlockCode(info.unlock_code, uId);


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

            JsonResponse js = new JsonResponse();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            if (vendor.description == "1")
            {
                UserInfo us = controller.GetUserById(uId);

                if (uId != 0)
                {
                    js.status = "Ok";
                    js.isUsingValideUnlockCode = 1;
                    js.result = us;
                    js.error_message = "Unlock code updated successfully !";
                    return response;
                }
                else
                {
                    js.status = "Failed";
                    js.isUsingValideUnlockCode = 1;
                    js.result = us;
                    js.error_message = "Invalid User Credentials";
                    return response;
                }


            }
            else if (vendor.description == "2")
            {

                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.result = null;
                js.error_message = "Code limit reached";
                return response;


            }
            else if (vendor.description == "0")
            {
                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.result = null;
                js.error_message = "Invalid Unlock Code";
                return response;
            }
            else
            {
                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.result = null;
                js.error_message = "Invalid Request";
                return response;
            }
            //success
            //limit reach
            //invalid



        }
        [HttpPost]
        public HttpResponseMessage LoginUser([FromBody]JObject data)
        {


            string results = "";

            UserInfo info = new UserInfo();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserInfo>(results);

                info = controller.AuthenticateAppUser(info.email, info.password,info.gcm_token);
                

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
                
            }
            JsonResponse js = new JsonResponse();
            if (info.Id != 0)
            {

                js.status = "Ok";
                js.result = info;
                if (info.unlock_code != "0")
                {
                    js.isUsingValideUnlockCode = 1;
                }
                else
                {
                    js.isUsingValideUnlockCode = 0;
                }
                js.error_message = "Success";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Failed";
                js.isUsingValideUnlockCode = 0;
                js.error_message = "Invalid User Credentials";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
                
            }

            
        }
        [HttpPost]
        public HttpResponseMessage CheckUserEmail([FromBody]JObject data)
        {


            string results = "";
            int emailResult = 0;
            UserEmailInput info = new UserEmailInput();
            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserEmailInput>(results);
                emailResult = controller.CheckAppUserEmail(info.email, "");


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

            UserEmailOutput js = new UserEmailOutput();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            if (emailResult == 1)
            {
                SendNewPasswordToUser(info.email);
                js.status = "Ok";
                js.error_message = "Password reset code sent to your email!";
                return response;

            }
            else
            {
                js.status = "Failed";
                js.error_message = "Invalid email";
                return response;
            }

        }
        [HttpPost]
        public HttpResponseMessage AddContact([FromBody]JObject data)
        {
            string results = "";

            UserContactWraper info = new UserContactWraper();


            DataUser user = new DataUser();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserContactWraper>(results);
                user = controller.AddContact(info.userId, info.qlumi_userId, info.contact_number);


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
            JsonResponseAddContact js = new JsonResponseAddContact();
            if (user.Id != 0)
            {
                user.password = "";
                js.status = "Ok";
                js.result = user;
                js.iAmVisibleToContact = "0";
                js.error_message = "Successfully!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Failed";
                user.password = "";
                js.iAmVisibleToContact = "0";
                js.error_message = user.name;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage RemoveContact([FromBody]JObject data)
        {
            string results = "";

            ChatInput info = new ChatInput();


            DataUser user = new DataUser();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<ChatInput>(results);
                controller.RemoveContact(info.sId, info.rId);


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
            UserEmailOutput js = new UserEmailOutput();
            js.status = "Ok";
            js.error_message = "User Deleted Successfully!";

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            return response;

        }
        [HttpPost]
        public HttpResponseMessage AllContact([FromBody]JObject data)
        {
            string results = "";

            
            UserContactWraper info = new UserContactWraper();


            DataTable users = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                dynamic dataInfo = new JavaScriptSerializer().Deserialize<UserContactWraper>(results);
                users = controller.GetUserContacts(dataInfo.userId);


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
            UserContactResponseWraper js = new UserContactResponseWraper();
            if (users.Rows.Count > 0)
            {
                js.status = "Ok";
                js.contactList = users;
                js.error_message = "Success";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Ok";
                js.contactList = new DataTable();
                js.error_message = "No user found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage AllUsers([FromBody]JObject data)
        {
            string results = "";


            UserInfo info = new UserInfo();


            DataTable users = new DataTable();

            try
            {
                info = new JavaScriptSerializer().Deserialize<UserInfo>(results);
                users = controller.GetAllUsers();


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
            UserContactResponseWraper js = new UserContactResponseWraper();
            if (users.Rows.Count > 0)
            {
                js.status = "Ok";
                js.contactList = users;
                js.error_message = "Success";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Ok";
                js.contactList = new DataTable();
                js.error_message = "No user found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }



        [HttpPost]
        public HttpResponseMessage VendorUsers([FromBody]JObject data)
        {
            string results = "";


            RenewUnlockInput info = new RenewUnlockInput();


            DataTable users = new DataTable();

            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<RenewUnlockInput>(results);
                users = controller.GetVendorUsers(info.unlock_code, info.userId);


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
            UserContactResponseWraper js = new UserContactResponseWraper();
            if (users.Rows.Count > 0)
            {
                js.status = "Ok";
                js.contactList = users;
                js.error_message = "Success";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Ok";
                js.contactList = new DataTable();
                js.error_message = "No user found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage AllContactWithMessage([FromBody]JObject data)
        {
            string results = "";


            UserContactWraper info = new UserContactWraper();


            DataTable users = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                dynamic dataInfo = new JavaScriptSerializer().Deserialize<UserContactWraper>(results);
                users = controller.GetUserContactsWithMessage(dataInfo.userId);


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
            UserContactResponseWraper js = new UserContactResponseWraper();
            if (users.Rows.Count > 0)
            {
                js.status = "Ok";
                js.contactList = users;
                js.error_message = "Success";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Ok";
                js.contactList = new DataTable();
                js.error_message = "No user found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }
        [HttpPost]
        public HttpResponseMessage ApplicationStats([FromBody]JObject data)
        {
            DataTable stats = new DataTable();
            stats = controller.GetApplicationStats();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, stats);
            return response;

        }
        [HttpPost]
        public HttpResponseMessage VendorRanking([FromBody]JObject data)
        {
            DataTable stats = new DataTable();
            stats = controller.GetVendorRanking();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, stats);
            return response;

        }
        [HttpPost]
        public HttpResponseMessage UserDetail([FromBody]JObject data)
        {
            string results = "";


            UserContactWraper info = new UserContactWraper();


            DataTable users = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                dynamic dataInfo = new JavaScriptSerializer().Deserialize<UserContactWraper>(results);
                users = controller.GetUserDetail(dataInfo.userId);


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
            UserContactResponseWraper js = new UserContactResponseWraper();
            if (users.Rows.Count > 0)
            {
                js.status = "Ok";
                js.contactList = users;
                js.error_message = "Success";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Failed";
                js.contactList = new DataTable();
                js.error_message = "No user found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }
        [HttpPost]
        public HttpResponseMessage AllVisibleContact([FromBody]JObject data)
        {
            string results = "";


            JsonLocationInput1 info = new JsonLocationInput1();


            DataTable users = new DataTable();
            DataTable location = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<JsonLocationInput1>(results);
                string uId = controller.GetQlumiIdByUserId(info.userId);
                users = controller.GetUserVisibleContacts(uId);
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
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
                //Handle it
            }
            UserContactResponseWraper js = new UserContactResponseWraper();

            

            if (users.Rows.Count > 0)
            {

                js.status = "Ok";
                js.contactList = CreateUserList(users, location);
                js.error_message = "Success";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Ok";
                js.contactList = new DataTable();
                js.error_message = "No user found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }

        public DataTable CreateUserList(DataTable dt, DataTable locationDt)
        {
            DataTable stats = new DataTable();
            stats.Columns.Add("userId", typeof(int));
            stats.Columns.Add("name", typeof(string));
            stats.Columns.Add("email", typeof(string));
            stats.Columns.Add("contactNumber", typeof(string));
            stats.Columns.Add("iAmVisibleToContact", typeof(string));
            stats.Columns.Add("qlumi_userId", typeof(string));
            stats.Columns.Add("latitude", typeof(string));
            stats.Columns.Add("longitude", typeof(string));
            stats.Columns.Add("sessionId", typeof(string));
            stats.Columns.Add("connectionId", typeof(string));
            stats.Columns.Add("UpdateAt", typeof(string));
            stats.Columns.Add("lastUpdatedAt", typeof(string));


            if (dt.Rows.Count > 0 && locationDt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    TimeZone zone = TimeZone.CurrentTimeZone;
                    DateTime startTime = DateTime.Parse(row[10].ToString());
                    DateTime endTime = DateTime.Parse(locationDt.Rows[0].ItemArray[4].ToString());

                    TimeSpan span = endTime.Subtract(startTime);

                    string min = span.Minutes.ToString();

                    if (int.Parse(span.Minutes.ToString()) < 6)
                    {
                        stats.Rows.Add(row.ItemArray);
                    }
                    //stats.Rows.Add(row.ItemArray);
                }
                
            }
            return stats;
        }

        [HttpPost]
        public HttpResponseMessage SetVisibleStatus([FromBody]JObject data)
        {
            string results = "";


            UserContactWraper info = new UserContactWraper();


            int visibleResult = 0;

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                dynamic dataInfo = new JavaScriptSerializer().Deserialize<UserContactWraper>(results);
                visibleResult = controller.SetVisibleStatus(dataInfo.userId, dataInfo.qlumi_userId);


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
            UserVisibleResponseWraper js = new UserVisibleResponseWraper();
            if (visibleResult == 1)
            {
                js.status = "Ok";
                js.iAmVisibleToContact = "1";
                js.message = "User is now Visible";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            if (visibleResult == 0)
            {
                js.status = "Ok";
                js.iAmVisibleToContact = "0";
                js.message = "User is now InVisible";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Failed";
                js.iAmVisibleToContact = "0";
                js.message = "User does not exists";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }

        public HttpResponseMessage RegisterVendor([FromBody]Vendor data)
        {
            List<Vendor> v = new List<Vendor>();
            try
            {
                data.unlock_code = GetRandomPasswordUsingGUID(6);
                v = controller.RegisterVendor(data);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }



        public HttpResponseMessage allVendors([FromBody]Vendor data)
        {
            List<Vendor> v = new List<Vendor>();
            try
            {
                v = controller.GetAllVendors();
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage CheckMySqlConnection([FromBody]JObject data)
        {
            string constr = "Data Source=u19209965.onlinehome-server.com;port=3306;Initial Catalog=test;User Id=irfan;password=admin@123";//ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
            return error;
        }
        [HttpPost]
        public HttpResponseMessage RemainingUnlockCodes([FromBody]JObject data)
        {


            string results = "";
            DataTable emailResult = new DataTable();
            UserEmailInput info = new UserEmailInput();
            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserEmailInput>(results);
                emailResult = controller.GetVendorRemainingUnlockCodes(info.email);


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

            VendorOutput js = new VendorOutput();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            if (emailResult.Rows.Count > 0)
            {
                js.status = "Ok";
                js.vendorInfo = emailResult;
                js.error_message = "Success!";
                return response;

            }
            else
            {
                js.status = "Failed";
                js.error_message = "Invalid email";
                return response;
            }

        }
        public HttpResponseMessage RemoveVendor([FromBody]Vendor data)
        {
            try
            {
                controller.DeleteVendor(data.Id);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "");
            return response;
        }
        //Chat

        [HttpPost]
        public HttpResponseMessage test1([FromBody]JObject data)
        {
            //SendNewPasswordToUser("imirfansaeefd@gmail.com");
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Empty respons");
            return response;

        }

        [HttpPost]
        public HttpResponseMessage GetBaseUrl([FromBody]JObject data)
        {

            ServerBaseUrl sb = new ServerBaseUrl();

            sb. status = "Ok";
            sb.serverURL = "http://qlumi.com/";
            sb.error_message = "Success!";

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sb);
            return response;

        }

        [HttpPost]
        public HttpResponseMessage AddMessage([FromBody]JObject data)
        {
            string results = "";

            ChatHistory info = new ChatHistory();
            int chatResponse = 0;

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<ChatHistory>(results);
                chatResponse = controller.AddChatMessage(info);
                //string qlumiId = controller.ge


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
            ChatOutput js = new ChatOutput();
            if (chatResponse != 0)
            {

                //Send push notification 


                DataTable userDeviceAndOrigin = controller.GetGcmTokenByUserId(info.rId.ToString());
                string deviceId = "asdfasdlj";
                string requestOrigin = "iPhone";
                if (userDeviceAndOrigin.Rows.Count > 0)
                {
                    deviceId = userDeviceAndOrigin.Rows[0].ItemArray[1].ToString();
                    requestOrigin = userDeviceAndOrigin.Rows[0].ItemArray[0].ToString();
                }
                string notification_data = "Demo Notification";

                ChatMessageOutput mp = new ChatMessageOutput();
                mp.T = 3;
                mp.alert = info.message.ToString();
                mp.Title = "Message1";
                mp.rId = int.Parse(info.rId.ToString());
                mp.sId = int.Parse(info.sId.ToString());
                mp.createAt = DateTime.Now.ToString();

                HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.OK, mp);
                notification_data = new JavaScriptSerializer().Serialize(mp);

                string postData =
                "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
                  "\"data\": {\"notification_data\": [{\"T\":\"3\",\"alert\":\"" + mp.alert + "\",\"Title\":\"" + mp.Title + "\",\"rId\":\"" + mp.rId + "\",\"sId\":\"" + mp.sId + "\",\"createdAt\":\"" + mp.createAt + "\"}]}}";

                if (deviceId != "")
                {
                    if (deviceId.Length > 64)
                    {
                        string response1 = SendGCMNotification(apiKey, postData);
                    }
                    else
                    {
                        string resp = controller.SendIPhonePushNotfication(info.sId.ToString(), info.rId.ToString(), info.rName, info.sName, DateTime.Now.ToString(), deviceId, info.message.ToString());
                    }
                }
                

                //End push notification here
                js.status = "Ok";
                js.error_message = "Message Saved!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Failed";
                js.error_message = "Message not saved";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }


        [HttpPost]
        public HttpResponseMessage AddMessageWithPush([FromBody]JObject data)
        {
            string results = "";

            ChatHistory info = new ChatHistory();
            int chatResponse = 0;

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<ChatHistory>(results);
                chatResponse = controller.AddChatMessage(info);
                //string qlumiId = controller.ge


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
            ChatOutput js = new ChatOutput();
            if (chatResponse != 0)
            {

                //Send push notification 


                DataTable userDeviceAndOrigin = controller.GetGcmTokenByUserId(info.rId.ToString());
                string deviceId = "asdfasdlj";
                string requestOrigin = "iPhone";
                if (userDeviceAndOrigin.Rows.Count > 0)
                {
                    deviceId = userDeviceAndOrigin.Rows[0].ItemArray[1].ToString();
                    requestOrigin = userDeviceAndOrigin.Rows[0].ItemArray[0].ToString();
                }
                string notification_data = "Demo Notification";

                ChatMessageOutput mp = new ChatMessageOutput();
                mp.T = 3;
                mp.alert = info.message.ToString();
                mp.Title = "Message2";
                mp.rId = int.Parse(info.rId.ToString());
                mp.sId = int.Parse(info.sId.ToString());
                mp.createAt = DateTime.Now.ToString();

                HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.OK, mp);
                notification_data = new JavaScriptSerializer().Serialize(mp);

                string postData =
                "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
                  "\"data\": {\"notification_data\": [{\"T\":\"3\",\"alert\":\"" + mp.alert + "\",\"Title\":\"" + mp.Title + "\",\"rId\":\"" + mp.rId + "\",\"sId\":\"" + mp.sId + "\",\"createdAt\":\"" + mp.createAt + "\"}]}}";

                if (deviceId != "")
                {
                    if (deviceId.Length > 64)
                    {
                        string response1 = SendGCMNotification(apiKey, postData);
                    }
                    else
                    {
                        string resp = controller.SendIPhonePushNotfication(info.sId.ToString(), info.rId.ToString(), info.rName, info.sName, DateTime.Now.ToString(), deviceId, info.message.ToString());
                    }
                    
                }


                //End push notification here
                js.status = "Ok";
                js.error_message = "Message Saved!";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Failed";
                js.error_message = "Message not saved";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }



        [HttpPost]
        public HttpResponseMessage ChatHistory([FromBody]JObject data)
        {
            string results = "";


            ChatInput info = new ChatInput();


            DataTable chatHistory = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<ChatInput>(results);
                chatHistory = controller.GetChatHistory(info.sId, info.rId);




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
            ChatHistoryOutput js = new ChatHistoryOutput();
            if (chatHistory.Rows.Count > 0)
            {
                js.status = "Ok";
                js.chat_history = chatHistory;
                js.error_message = "Success";

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }
            else
            {
                js.status = "Ok";
                js.chat_history = new DataTable();
                js.error_message = "No chat history found";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
                return response;
            }

        }

        [HttpPost]
        public HttpResponseMessage SetUnreadCount([FromBody]JObject data)
        {
            string results = "";


            UserContactWraper info = new UserContactWraper();


            DataTable chatHistory = new DataTable();

            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                results = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<UserContactWraper>(results);
                controller.UpdateUnreadCount(int.Parse(info.userId.ToString()), 0);




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

            UserEmailOutput js = new UserEmailOutput();
            js.status = "Ok";
            js.error_message = "Count is set to 0";

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, js);
            return response;

        }


        [HttpPost]
        public void UploadFile()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

                if (httpPostedFile != null)
                {
                    // Validate the uploaded image(optional)

                    // Get the complete file path
                    string vendorID = HttpContext.Current.Request.Params.GetValues("vendorId")[0];
                    string imageName = vendorID + "" + httpPostedFile.FileName;
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), imageName);

                    // Save the uploaded file to "UploadedFiles" folder
                    
                    controller.UpdateVendorImageName(int.Parse(vendorID), imageName);
                    httpPostedFile.SaveAs(fileSavePath);


                }
            }
        }
        protected void Sendmail(string pass)
        {

            string email = "imirfansaeed@gmail.com";
            BusinessCoreController controller = new BusinessCoreController();
            string updatedPass = pass;// GetRandomPasswordUsingGUID(5);
            string message = "New Password";
            string HostAdd = ConfigurationManager.AppSettings["Host"].ToString();
            string frommail = ConfigurationManager.AppSettings["FromMail"].ToString();
            string password = ConfigurationManager.AppSettings["Password"].ToString();
            System.Net.Mail.MailMessage eMail = new System.Net.Mail.MailMessage();
            eMail.IsBodyHtml = true;
            eMail.IsBodyHtml = true;
            eMail.Body = message;
            eMail.From = new System.Net.Mail.MailAddress("info.dentalvalet@gmail.com");
            eMail.To.Add(email);
            eMail.Subject = "Qlumi";
            System.Net.Mail.SmtpClient SMTP = new System.Net.Mail.SmtpClient();
            //SMTP.UseDefaultCredentials = true;
            SMTP.Credentials = new System.Net.NetworkCredential("info.dentalvalet@gmail.com", "searay123");
            //SMTP.Host = "localhost";
            //For Gmail Start
            SMTP.Port = 587;
            SMTP.Host = "smtp.gmail.com";
            SMTP.EnableSsl = true;
            
            SMTP.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //For Gmail End
            //controller.UpdatePasswordByEmail(email, updatedPass);
            SMTP.Send(eMail);




            // New email setup




        }
        protected void SendNewPasswordToUser(string userEmail)
        {

            string email = userEmail;
            BusinessCoreController controller = new BusinessCoreController();
            string updatedPass = GetRandomPasswordUsingGUID(5);
            string message = "<div style=\"width:100%; margin:0 auto;\"><div style=\"width:100%; float:left; text-align:center; margin:5px 0px; font-family:arial; font-size:20px; color:#28d;\"><img src=\"http://qlumi.com/Images/logo.png\" width=\"163\" /> </div><div style=\"width:100%; float:left; text-align:center; margin:15px 0px; font-family:arial; font-size:18px; color:#999;\">Email address confirmed! Here is your code to reset password. Please do not reply to this email. Replies to this email are not read. If you need to communicate with Qlumi staff please use the help support system at our website at <a href=\"http://www.qlumi.com\">http://www.qlumi.com</a> <br/>Thank you</div><div style=\"width:100%; float:left; text-align:center; margin:30px 0px; font-family:Arial; font-size:25px; color:#fff; padding:10px 0px; background-color:#024ebe;\">" + updatedPass + "</div></div>";
            

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("qlumi.com");

            mail.From = new MailAddress("no-reply@qlumi.com");
            mail.To.Add(email);
            mail.Subject = "Qlumi Password Recovery";
            mail.IsBodyHtml = true;
            mail.IsBodyHtml = true;
            mail.Body = message;
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential("no-reply@qlumi.com", "GhostBusters1");
            SmtpServer.EnableSsl = false;
            controller.StorePasswordResetCodeByEmail(email, updatedPass);
            SmtpServer.Send(mail);



        }
        public string GetRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);

            // Make sure length is valid
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes
            return guidResult.Substring(0, length);
        }
        public int CheckUnlockCodeExpiry()
        {
            return 0;
        }
        //Categories
        #region categories


        public HttpResponseMessage AllCategories([FromBody]AppCategory data)
        {
            List<AppCategory> v = new List<AppCategory>();
            try
            {
                v = controller.GetAppCategories();
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }
        #endregion
        //End Categories
        #region vendorProfile


        public HttpResponseMessage LoginVendor([FromBody]Vendor vendor)
        {



            EncryptPwd encryption = new EncryptPwd();
            Vendor ad = new Vendor();
            string status = vendor.name.ToString().Trim();

            string username = vendor.email.Trim(); //"imirfansaeed@gmail.com";// 
            string password = vendor.password.Trim(); //"searay";//
            string encpassword = encryption.EncryptToString(password);

            if (username.Length > 0 && encpassword.Length > 0)
            {
                BusinessCoreController controller = new BusinessCoreController();
                ad = controller.AuthenticateVendor(username, password, vendor.role.ToString());
                if (ad.Id != 0)
                {
                    //controller.LoginUser(ad.Id);

                    HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.OK, ad);
                    return res;
                }
                else
                {
                    HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.OK, "0");
                    return res; ;
                }

            }
            else
            {
                HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.OK, "0");
                return res;
            }

        }


        public HttpResponseMessage AddVendorProfile([FromBody]VendorProfile data)
        {
            List<VendorProfile> v = new List<VendorProfile>();
            try
            {
                v = controller.AddVendorProfile(data);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }

        public HttpResponseMessage AllProfiles([FromBody]VendorProfile data)
        {
            DataTable v = new DataTable();
            try
            {
                v = controller.GetAllVendorProfileSettings();
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AllVendorProfiles([FromBody]JObject data)
        {
            string res = "";

            RenewUnlockInput info = new RenewUnlockInput();
            DataTable v = new DataTable();
            if (data == null)
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.OK, "");
                return error;
            }
            try
            {
                res = data.ToString(Formatting.None);
                info = new JavaScriptSerializer().Deserialize<RenewUnlockInput>(res);
                v = controller.GetVendorProfileSettings(info.userId);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }

        public HttpResponseMessage AllAppCategory([FromBody]AppCategory data)
        {
            List<AppCategory> v = new List<AppCategory>();
            try
            {
                v = controller.GetAppCategories();
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }
        

        public HttpResponseMessage AllAppSubCategory([FromBody]SubCategory data)
        {
            List<SubCategory> v = new List<SubCategory>();
            try
            {
                v = controller.GetAllSubCategories(data.Id);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }


        public HttpResponseMessage VendorProfile([FromBody]VendorProfile data)
        {
            DataTable v = new DataTable();
            try
            {
                v = controller.GetVendorProfile(data.vendorId);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        
        }




        public HttpResponseMessage AddVendorInterest([FromBody]VendorInterest data)
        {
            DataTable v = new DataTable();
            int interstId = 0;
            try
            {
                interstId = controller.AddVendorInterest(data);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, interstId);
            return response;

        }


        public HttpResponseMessage AllVendorInterest([FromBody]VendorProfile data)
        {
            DataTable v = new DataTable();
            try
            {
                v = controller.GetVendorInterests(data.Id);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;

        }
        #endregion
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
        public static bool ValidateServerCertificate(
                                                     object sender,
                                                     X509Certificate certificate,
                                                     X509Chain chain,
                                                     SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }



        [HttpPost]
        public HttpResponseMessage SendPush([FromBody]JObject data)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "");
            var appleCert = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/final.p12"));


            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, appleCert, "");
            config.ValidateServerCertificate = false;

            var apnsBroker = new ApnsServiceBroker(config);

            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        response = Request.CreateResponse(HttpStatusCode.OK, "Notification Failed!");

                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException           
                        response = Request.CreateResponse(HttpStatusCode.OK, "Notification Failed because of unknown reason!");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                response = Request.CreateResponse(HttpStatusCode.OK, "Notification Sent!");

            };

            apnsBroker.Start();

            apnsBroker.QueueNotification(new ApnsNotification
            {
                DeviceToken = "060da08dd3d3c4306fdc8ade2b0685d490ff78b98097acc2fb17800ddae6f5ec", 

                //{ \"registration_ids\": [ \"" + deviceId + "\" ], " + "\"data\": {\"notification_data\": [{\"T\":\"3\",\"alert\":\"" + mp.alert + "\",\"Title\":\"" + mp.Title + "\",\"rId\":\"" + mp.rId + "\",\"sId\":\"" + mp.sId + "\",\"createdAt\":\"" + mp.createAt + "\"}]}}
                Payload = JObject.Parse("{\"aps\":{\"badge\":\"1\"}}")
            });
            apnsBroker.Stop();

            return response;
        }

        public HttpResponseMessage RegisterAdvertiser([FromBody]Vendor data)
        {
            List<Vendor> v = new List<Vendor>();
            try
            {
                data.unlock_code = GetRandomPasswordUsingGUID(6);
                v = controller.RegisterVendor(data);
                //UploadFile(1);

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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, v);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpDatePasswordByModel(UpdatePassword data)
        {
            if (data == null || (data.NewPass!=data.RepeatNewPass))
            {
                return Request.CreateResponse(HttpStatusCode.OK, 0);
            }
            var obj =  controller.UpdateUserPassword(data);
            var response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }
    }
    public class JsonLocationInput1
    {
        public string userId { get; set; }
        public string lati { get; set; }
        public string longi { get; set; }

    }
    public class JsonResponse
    {
        public JsonResponse()
        {
            isUsingValideUnlockCode = 1;
        }
        public string status{get;set;}
        public int isUsingValideUnlockCode { get; set; }
        public UserInfo result { get; set; }
        public string error_message { get; set; }

        
    }
    public class JsonResponseSignup
    {
        public string status { get; set; }
        public string userId { get; set; }
        public string qlumi_userId { get; set; }
        public string error_message { get; set; }

    }
    public class JsonResponseAddContact
    {
        public string status { get; set; }
        public string iAmVisibleToContact { get; set; }
        public UserInfo result { get; set; }
        public string error_message { get; set; }

    }
    public class UserContactWraper
    {
        public string userId { get; set; }
        public string qlumi_userId { get; set; }
        public string contact_number { get; set; }

    }
    public class UserEmailInput
    {
        public string email { get; set; }

    }
    public class UserEmailOutput
    {
        public string status { get; set; }
        public string error_message { get; set; }

    }

    public class ServerBaseUrl
    {
        public string status { get; set; }
        public string serverURL { get; set; }
        public string error_message { get; set; }

    }
    public class VendorOutput
    {
        public string status { get; set; }
        public DataTable vendorInfo { get; set; }
        public string error_message { get; set; }

    }
    public class RenewUnlockInput
    {
        public string userId { get; set; }
        public string unlock_code { get; set; }

    }
    public class ResetPasswordInput
    {
        public string resetCode { get; set; }
        public string newPassword { get; set; }

    }
    public class UserContactResponseWraper
    {
        public string status { get; set; }
        public DataTable contactList { get; set; }

        public string error_message { get; set; }
    }
    public class UserVisibleResponseWraper
    {
        public string status { get; set; }
        public string iAmVisibleToContact { get; set; }
        public string message { get; set; }

    }
    public class Chat_History
    {
        public string sId { get; set; }
        public string rId { get; set; }
        public string sName { get; set; }
        public string rName { get; set; }
        public string messageType { get; set; }
        public string message { get; set; }
        public string messageDate { get; set; }

    }
    public class ChatOutput
    {
        public string status { get; set; }
        public string error_message { get; set; }

    }
    public class ChatInput
    {
        public string sId { get; set; }
        public string rId { get; set; }
        public string local { get; set; }

    }
    public class ChatHistoryOutput
    {
        public string status { get; set; }
        public DataTable chat_history { get; set; }

        public string error_message { get; set; }
    }
    

    //SyncInformation



    public class Profile
    {
        public string porfile_name { get; set; }
        public string color { get; set; }
        public int isActive { get; set; }
        public List<object> profile_interest { get; set; }
    }

    public class MarkerDetails
    {
        public string formatted_address { get; set; }
        public string formatted_phone { get; set; }
        public string icon { get; set; }
        public string international_phone_number { get; set; }
        public bool isOpen { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string name { get; set; }
        public string place_id { get; set; }
        public string rating { get; set; }
        public string url { get; set; }
        public int user_ratings_total { get; set; }
        public string vicinity { get; set; }
        public string website { get; set; }
    }

    public class Place
    {
        public string icon { get; set; }
        public string id { get; set; }
        public bool isOpen { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string name { get; set; }
        public string place_id { get; set; }
        public string vicinity { get; set; }
    }

    public class PlaceDetails
    {
        public string crawlerInfo { get; set; }
        public MarkerDetails markerDetails { get; set; }
        public Place place { get; set; }
    }

    public class Bookmark
    {
        public string placeId { get; set; }
        public PlaceDetails placeDetails { get; set; }
    }

    public class SyncRootObject
    {
        public List<Profile> Profiles { get; set; }
        public List<Bookmark> Bookmarks { get; set; }
        public List<object> Notes { get; set; }
        public List<object> Meetings { get; set; }
        public string Id { get; set; }
        public string requestOrigin { get; set; }
        public string app_version { get; set; }
        public bool isLogoutRequest { get; set; }
        public string updatedAt { get; set; }
    }

}