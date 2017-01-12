using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Google;
using PushSharp.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebCrawler;
using WebCrawler.DataCore.Managers;
using WebCrawler.Models;

namespace WebCrawler.DataCore
{
    public class BusinessCoreController
    {
        #region Crawler

        public crawler GetHtmlDataById(int Id)
        {
            return new AdminManager().GetHtmlDataById(Id);
        }
        public int AddHtmlData(crawler cr)
        {
            return new AdminManager().AddHtmlData(cr);
        }
        public void UpdateCrawlDataByPlaceId(string data, string placeId)
        {
            new AdminManager().UpdateCrawlDataByPlaceId(data, placeId);
        }
        public crawler GetCrawlerByUserIdAndGps(string userId, string lati, string longi)
        {
            return new AdminManager().GetCrawlerByUserIdAndGps(userId, lati, longi);
        }
        public crawler GetCrawlerByPlaceId(string placeId)
        {
            return new AdminManager().GetCrawlerByPlaceId(placeId);
        }
        public List<crawler> GetCrawlerByUserId(int userId)
        {
            return new AdminManager().GetCrawlerByUserId(userId);
        }
        public List<crawler> GetAllCrawlData()
        {
            return new AdminManager().GetAllCrawlData();
        }
        public void DeleteData(int Id)
        {
            new AdminManager().DeleteData(Id);
        }



        public List<includedWord> GetIncludedWords()
        {
            return new AdminManager().GetIncludedWords();
        }
        public List<includedWord> IncludeWord(includedWord inc)
        {
            return new AdminManager().IncludeWord(inc);
        }
        public List<includedWord> RemoveIncludedWord(int Id)
        {
            return new AdminManager().RemoveIncludedWord(Id);
        }


        public List<excludedWord> GetExcludedWords()
        {
            return new AdminManager().GetExcludedWords();
        }
        public List<excludedWord> AddExcludeWord(excludedWord ex)
        {
            return new AdminManager().AddExcludeWord(ex);
        }
        public List<excludedWord> RemoveExcludedWord(int Id)
        {
           return new AdminManager().RemoveExcludedWord(Id);
        }




        public List<category> GetCategories()
        {
            return new AdminManager().GetCategories();
        }
        public List<category> AddCategory(category ex)
        {
            return new AdminManager().AddCategory(ex);
        }
        public List<category> RemoveCategory(int Id)
        {
            return new AdminManager().RemoveCategory(Id);
        }


        public List<AppCategory> GetAppCategories()
        {
            return new UsersManager().GetAllCategories();
        }
        //public List<SubCategory> GetAllSubCategories(int catId)
        //{
        //    return new UsersManager().GetAllSubCategories(catId);
        //}

        public List<VendorProfile> GetAppVendorProfileSettings(int vendorId)
        {
            return new UsersManager().GetAppVendorProfileSettings(vendorId);
        }


        public DataTable GetAllVendorProfileSettings()
        {
            return new UsersManager().GetAllVendorProfileSettings();
        }


        public DataTable GetVendorProfileSettings(string vendorId)
        {
            return new UsersManager().GetVendorProfileSettings(vendorId);
        }


        public List<VendorProfile> AddVendorProfile(VendorProfile vendor)
        {
            return new UsersManager().AddVendorProfile(vendor);
        }
        #endregion

        #region Admin

        public admin AuthenticateUser(string email, string password)
        {
            return new AdminManager().AuthenticateUser(email, password);
        }
        public Vendor AuthenticateVendor(string email, string password, string role)
        {
            return new AdminManager().AuthenticateVendor(email, password,role);
        }
        public int CheckUserEmail(string email, string username)
        {
            return new AdminManager().CheckUserEmail(email,username);
        }
        public int RegisterAdmin(admin admin)
        {
            return new AdminManager().RegisterAdmin(admin);
        }
        public void UpdatePasswordByEmail(string email, string password)
        {

            new AdminManager().UpdatePasswordByEmail(email, password);
        }

        public void LoginUser(int id)
        {

            new AdminManager().LoginUser(id);
        }
        public void LoginOutUser(int id)
        {

            new AdminManager().LoginOutUser(id);
        }
        #endregion 






        #region User

        public UserInfo AuthenticateAppUser(string email, string password, string token)
        {
            return new UsersManager().AuthenticateUser(email, password,token);
        }
        public void UpdateGcmToken(string userId, string gcmToken)
        {
            new UsersManager().UpdateGcmToken(userId,gcmToken);
        }
        public DataTable UpdateTokBoxConnectionId(string userId, string connectionId)
        {
            return new UsersManager().UpdateTokBoxConnectionId(userId, connectionId);
        }
        public DataTable FetchTokBoxConnectionId(string userId)
        {
            return new UsersManager().FetchTokBoxConnectionId(userId);
        }
        public void SyncProfileInformation(string userId, string info, string updatedAt)
        {
            new UsersManager().SyncProfileInformation(userId, info, updatedAt);
        }
        public UserInfo GetUserById(int Id)
        {
            return new UsersManager().GetUserById(Id);
        }

        public UserInfo GetUserByIdWithNoCode(int Id)
        {
            return new UsersManager().GetUserByIdWithNoCode(Id);
        }
        public int CheckAppUserEmail(string email, string username)
        {
            return new UsersManager().CheckUserEmail(email, username);
        }
        public int RegisterAppUser(UserInfo user)
        {
            return new UsersManager().RegisterUser(user);
        }
        public void UpdateAppUserPasswordByEmail(string email, string password)
        {

            new UsersManager().UpdatePasswordByEmail(email, password);
        }

        public List<Profiles> GetUserProfileSettings(int userId)
        {
            return new UsersManager().GetAppUserProfileSettings(userId);
        }
        public DataTable GetAllUsers()
        {
            return new UsersManager().GetAllUsers();
        }

        public DataTable GetVendorUsers(string unlock_code, string vendorId)
        {
            return new UsersManager().GetVendorUsers(unlock_code, vendorId);
        }
        public DataTable GetSearchResult(UserInfo info)
        {
            return new UsersManager().GetSearchResult(info);
        }
        public DataTable GetVendorSearchResult(UserInfo info)
        {
            return new UsersManager().GetVendorSearchResult(info);
        }
        public void UpdateUserInfo(UserInfo info)
        {
            new UsersManager().UpdateUserInfo(info);
        }
        public void DeleteUserProfileAndInterest(int userId)
        {
            new UsersManager().DeleteUserProfileAndInterest(userId);
        }
        public void DeleteUserInterest(int settingId)
        {
            new UsersManager().DeleteUserInterest(settingId);
        }
        public int AddUserProfile(SettingProfile profile)
        {
            return new UsersManager().AddUserProfile(profile);
        }

        public void AddUserInterest(UserInterest vi)
        {
            new UsersManager().AddUserInterest(vi);
        }

        public Vendor RenewUnlockCode(string code, int userId)
        {
            return new AdminManager().RenewUnlockCode(code, userId);
        }

        public void StorePasswordResetCodeByEmail(string email, string resetCode)
        {
            new UsersManager().StorePasswordResetCodeByEmail(email, resetCode);
        }
        public int UpdatePasswordByResetCode(string password, string resetCode)
        {
            return new UsersManager().UpdatePasswordByResetCode(password, resetCode);
        }
        public DataTable GetUserVisibilityStatus(string userId, string qlumi_Id)
        {
            return new UsersManager().GetUserVisibilityStatus(userId, qlumi_Id);
        }
        public void UpdateTokBoxInfo(string userId, string token, string sessionId)
        {
            new UsersManager().UpdateTokBoxInfo(userId, token, sessionId);
        }

        public int AddChatMessage(ChatHistory chat)
        {
            return new UsersManager().AddChatMessage(chat);
        }

        public DataTable GetChatHistory(string sId, string rId)
        {
            return new UsersManager().GetChatHistory(sId, rId);
        }
        public DataTable GetAllUsersByUnlockCode(string unlock)
        {
            return new UsersManager().GetAllUsersByUnlockCode(unlock);
        }
       
        public void UpdateUnreadCount(int userId, int countValue)
        {
            new UsersManager().UpdateUnreadCount(userId, countValue);
        }

        public int UpdateUserPassword(UpdatePassword data)
        {
           return new UsersManager().UpdatePasswordByModel(data);
        }
        #endregion 



        #region Vendor
        public List<Vendor> RegisterVendor(Vendor vendor)
        {
            return new AdminManager().RegisterVendor(vendor);
        }
        public List<Vendor> GetAllVendors()
        {
            return new AdminManager().GetAllVendors();
        }

        public Vendor GetVendorById(int Id)
        {
            return new AdminManager().GetVendorById(Id);
        }
        public Vendor GetVendorByUnlockCode(string code, int userId)
        {
            return new AdminManager().GetVendorByUnlockCode(code,userId);
        }
        public void UpdateVendorImageName(int vendorId, string image)
        {
            new AdminManager().UpdateVendorImageName(vendorId, image);
        }
        public void DeleteVendor(int vendorId)
        {
            new AdminManager().DeleteVendor(vendorId);
        }

        public DataTable GetVendorProfile(int vendorId)
        {
            return new UsersManager().GetVendorProfile(vendorId);
        }

        public int AddVendorInterest(VendorInterest vi)
        {
            return new UsersManager().AddVendorInterest(vi);
        }

        public DataTable GetVendorInterests(int profileId)
        {
            return new UsersManager().GetVendorInterests(profileId);
        }

        public DataTable GetVendorInterestsByVendorId(int vendorId)
        {
            return new UsersManager().GetVendorInterestsByVendorId(vendorId);
        }
        public DataTable GetVendorRemainingUnlockCodes(string email)
        {
            return new UsersManager().GetVendorRemainingUnlockCodes(email);
        }
        #endregion 


        #region Categories

        public SubCategory AddSubCategory(SubCategory cat)
        {
            return new UsersManager().AddSubCategory(cat);
        }

        public List<SubCategory> GetAllSubCategories(int catId)
        {
            return new UsersManager().GetAllSubCategories(catId);
        }
        public List<SubCategory> RemoveSubCategory(int catId, int subCatId)
        {
            return new UsersManager().RemoveSubCategory(catId, subCatId);
        }

        #endregion

        #region Contact
        public DataUser AddContact(string userId, string qlumiId, string contactNumber)
        {
            return new UsersManager().AddContact(userId,qlumiId, contactNumber);
        }
        public void RemoveContact(string sId, string rId)
        {
            new UsersManager().RemoveContact(sId, rId);
        }
        public string GetQlumiIdByUserId(string userId)
        {
            return new UsersManager().GetQlumiIdByUserId(userId);
        }

        public DataTable GetUserContacts(string userId)
        {
            return new UsersManager().GetUserContacts(userId);
        }
        public DataTable GetUserContactsWithMessage(string userId)
        {
            return new UsersManager().GetUserContactsWithMessage(userId);
        }
        public DataTable GetUserDetail(string userId)
        {
            return new UsersManager().GetUserDetail(userId);
        }

        public DataTable GetUserVisibleContacts(string userId)
        {
            return new UsersManager().GetUserVisibleContacts(userId);
        }


        public int SetVisibleStatus(string userId, string qlumi_UserId)
        {
            return new UsersManager().SetVisibleStatus(userId, qlumi_UserId);
        }
        #endregion 


        #region locations


        public DataTable GetGcmTokenByUserId(string userId)
        {
            return new UsersManager().GetGcmTokenByUserId(userId);
        }
        public DataTable GetMeetingNotes(string meetingId, int userId)
        {
            return new NotesManager().GetMeetingNotes(meetingId, userId);
        }
        public DataTable GetAllMeetingNotes(string meetingId, int userId)
        {
            return new NotesManager().GetAllMeetingNotes(meetingId, userId);
        }
        public DataTable GetAllMeetingNotesAsReceiver(string meetingId, int userId)
        {
            return new NotesManager().GetAllMeetingNotesAsReceiver(meetingId, userId);
        }
        public DataTable DeleteMeetingNote(string meetingId, int userId)
        {
            return new NotesManager().DeleteMeetingNote(meetingId, userId);
        }
        public DataTable AddMeetingNote(meetingNote meet)
        {
            return new NotesManager().AddMeetingNote(meet);

        }

        public DataTable UpdateMeetingNote(meetingNote meet)
        {
            return new NotesManager().UpdateMeetingNote(meet);

        }
        public DataTable GetGpsNotes(string noteId, string userId)
        {
            return new NotesManager().GetGpsNotes(noteId,userId);
        }
        public DataTable GetGpsNotesByUserId(string userId)
        {
            return new NotesManager().GetGpsNotesByUserId(userId);
        }
        public DataTable GetUserGpsNotesByUserId(string userId)
        {
            return new NotesManager().GetUserGpsNotesByUserId(userId);
        }

        public DataTable GetUserBookmarkedGpsNotesByUserId(string userId)
        {
            return new NotesManager().GetUserBookmarkedGpsNotesByUserId(userId);
        }

        public DataTable DeleteUserGpsNotesByUserId(string userId)
        {
            return new NotesManager().DeleteUserGpsNotesByUserId(userId);
        }
        public DataTable DeleteUserGpsNotesByNoteId(string noteId, string userId)
        {
            return new NotesManager().DeleteUserGpsNotesByNoteId(noteId, userId);
        }
        public DataTable AddGpsNote(gpsNote note)
        {
            return new NotesManager().AddGpsNote(note);
        }

        public DataTable AddUserGpsNote(UserGpsNote note)
        {
            return new NotesManager().AddUserGpsNote(note);
        }

        public DataTable UpdateUserNote(UserGpsNote cr)
        {
            return new NotesManager().UpdateUserNote(cr);
        }

        public DataTable BookmarkedGpsNotesByUserId(string userId, string noteId, string isBookmark)
        {
            return new NotesManager().BookmarkedGpsNotesByUserId(userId,noteId,isBookmark);
        }
        public DataTable GetUserLocation(string userId)
        {
            return new NotesManager().GetUserLocation(userId);
        }
        public DataTable UpdateUserLocation(string userId, string lati, string longi)
        {
            return new NotesManager().UpdateUserLocation(userId,lati,longi);
        }

        public DataTable GetApplicationStats()
        {
            return new UsersManager().GetApplicationStats();
        }
        public DataTable GetVendorRanking()
        {
            return new UsersManager().GetVendorRanking();
        }
        #endregion 


        #region VendorNote

        public DataTable AddVendorGpsNote(VendorNote cr)
        {
            return new NotesManager().AddVendorGpsNote(cr);
        }

        public DataTable GetVendorGpsNotesByCoord(string vendorId, string lat, string lng)
        {
            return new NotesManager().GetVendorGpsNotesByCoord(vendorId,lat,lng);
        }

        public DataTable GetVendorGpsNotesById(string vendorId)
        {
            return new NotesManager().GetVendorGpsNotesById(vendorId);
        }

        public DataTable SendAllVendorNotes(string vendorId, string unlock_code)
        {
            return new NotesManager().SendAllVendorNotes(vendorId, unlock_code);
        }

        public DataTable SendAllVendorNotesToNewUser(string vendorId, string unlock_code, string userId)
        {
            return new NotesManager().SendAllVendorNotesToNewUser(vendorId, unlock_code, userId);
        }
        public DataTable DeleteVendorGpsNotesByCoord(string vendorId, string lat, string lng)
        {
            return new NotesManager().DeleteVendorGpsNotesByCoord(vendorId, lat, lng);
        }
        #endregion



        #region Vendor Dashboard

        public int CreateVendorAd(AdInfo info)
        {
            return new VendorDashboardManager().CreateVendorAd(info);
        }
        public List<AdInfo> GetAdByVendorId(string vendorId)
        {
            return new VendorDashboardManager().GetAdByVendorId(vendorId);
        }

        public AdInfo GetAdById(string adId)
        {
            return new VendorDashboardManager().GetAdById(adId);
        }

        public List<AdInfo> GetAllVendorAds(string vendorId)
        {
            return new VendorDashboardManager().GetAllVendorAds(vendorId);
        }

        public List<AdInfo> GetAllAds()
        {
            return new VendorDashboardManager().GetAllAds();
        }

        public AdInfo UpdateAd(AdInfo info)
        {
            return new VendorDashboardManager().UpdateAd(info);
        }
        public AdInfo UpdateAdImage(string image, string adId)
        {
            return new VendorDashboardManager().UpdateAdImage(image, adId);
        }

        public AdInfo UpdateAdVideo(string image, string adId)
        {
            return new VendorDashboardManager().UpdateAdVideo(image, adId);
        }

        public DataTable GetAllAdTypes()
        {
            return new VendorDashboardManager().GetAllAdTypes();
        }

        public DataTable GetAllBidTypes()
        {
            return new VendorDashboardManager().GetAllBidTypes();
        }

        public List<AdInfo> RemoveAd(string Id, string vendorId)
        {
            return new VendorDashboardManager().RemoveAd(Id, vendorId);
        }

        public List<AdInfo> UpdateAdVisibility(string status, string vendorId, string adId)
        {
            return new VendorDashboardManager().UpdateAdVisibility(status, vendorId, adId);
        }
        #endregion

        #region Advertiser 

        public Vendor GetAdvertiserProfile(int vendorId)
        {
            return new AdvertiserManager().GetAdvertiserProfile(vendorId);
        }

        public Vendor UpdateAdvertiserProfile(Vendor vendor)
        {
            return new AdvertiserManager().UpdateAdvertiserProfile(vendor);
        }

        #endregion




        #region PushPart
        #region  Constants



        //public const string ANDROID_SENDER_AUTH_TOKEN = "AIzaSyCIplhvmqJhrGMuM-seJlMi5i_7K0auyEA";

        public const string ANDROID_SENDER_AUTH_TOKEN = "AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8";

        //public const string WINDOWS_PACKAGE_NAME = "yyyy";

        //public const string WINDOWS_PACKAGE_SECURITY_IDENTIFIER = "zzzz";

        //public const string WINDOWS_CLIENT_SECRET = "hhhh";

        public const string APPLE_APP_NAME = "com.ios.qlumi";

        public string APPLE_PUSH_CERT_PASS = "";

        #endregion



        #region Private members



        bool useProductionCertificate;

        string appleCertificateType;

        //  String appleCertName;

        String appleCertPath;

        byte[] appCertData;



        // logger

        //ILogger logger;



        // Config (1- Define Config for each platform)

        ApnsConfiguration apnsConfig;

        GcmConfiguration gcmConfig;

        WnsConfiguration wnsConfig;



        #endregion



        #region Constructor

        public BusinessCoreController()

        {

            string ProductionCertificate = WebConfigurationManager.AppSettings["ProductionCertificate"];

            // Initialize

            useProductionCertificate = true;// Convert.ToBoolean(ProductionCertificate); // you can set it dynamically from config



            appleCertificateType = useProductionCertificate ? "Dist.p12" : "Dev.p12";   // this is working for production

            // appleCertificateType = useProductionCertificate == true ? "ck_pizza.pem" : "Certificates.p12";     // this is incorrect certificate 
            //appleCertificateType = useProductionCertificate == true ? "DevbatchDist.p12" :"Certificates.p12";  // this is used for sand box
            ////  appleCertName = APPLE_APP_NAME + "-" + appleCertificateType;

            appleCertPath = HttpContext.Current.Server.MapPath("~/App_Data/" + appleCertificateType); // for web you should use HttpContext.Current.Server.MapPath(

            appCertData = File.ReadAllBytes(appleCertPath);

            var appleServerEnv = useProductionCertificate == true ? ApnsConfiguration.ApnsServerEnvironment.Production : ApnsConfiguration.ApnsServerEnvironment.Sandbox;

            // logger = LoggerHandler.CreateInstance();

            APPLE_PUSH_CERT_PASS = "";

            //APPLE_PUSH_CERT_PASS = useProductionCertificate == true ? "dist" : "1234";

            // 2- Initialize Config

            apnsConfig = new ApnsConfiguration(appleServerEnv, appCertData, APPLE_PUSH_CERT_PASS);

            //gcmConfig = new GcmConfiguration(ANDROID_SENDER_AUTH_TOKEN);

            //wnsConfig = new WnsConfiguration(WINDOWS_PACKAGE_NAME, WINDOWS_PACKAGE_SECURITY_IDENTIFIER, WINDOWS_CLIENT_SECRET);



        }



        #endregion



        #region Private Methods



        #endregion



        #region Public Methods



        public void SendNotificationToIOS(string msg, string deviceId)

        {

            // 3- Create a broker dictionary

            var apps = new Dictionary<string, AppPushBrokers> { {APPLE_APP_NAME,

                                                                 new AppPushBrokers {

                                                                    Apns = new ApnsServiceBroker (apnsConfig),

                                                                }}};



            #region Wire Up Events

            // 4- events to fires onNotification sent or failure for each platform



            #region Apple

            apps[APPLE_APP_NAME].Apns.OnNotificationFailed += (notification, aggregateEx) =>

            {



                aggregateEx.Handle(ex =>

                {



                    // See what kind of exception it was to further diagnose

                    if (ex is ApnsNotificationException)

                    {

                        var apnsEx = ex as ApnsNotificationException;



                        // Deal with the failed notification

                        var n = apnsEx.Notification;

                        //  logger.Error("Notification Failed: ID={n.Identifier}, Code={apnsEx.ErrorStatusCode}");



                    }

                    else if (ex is ApnsConnectionException)

                    {

                        // Something failed while connecting (maybe bad cert?)

                        //    logger.Error("Notification Failed (Bad APNS Connection)!");



                    }

                    else

                    {

                        // logger.Error("Notification Failed (Unknown Reason)!");



                    }



                    // Mark it as handled

                    return true;

                });

            };



            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>

            {

                Console.WriteLine("Notification Sent!");

            };



            #endregion



            #endregion



            #region Prepare Notification



            // 5- prepare the json msg for android and ios and any platform you want

            string notificationMsg = msg;

            string jsonMessage = @"{""message"":""" + notificationMsg +

                                        @""",""msgcnt"":1,""sound"":""custom.mp3""}";



            string appleJsonFormat = "{\"aps\": {\"alert\":" + '"' + notificationMsg + '"' + ",\"sound\": \"default\"}}";





            #endregion



            #region Start Send Notifications

            // 6- start sending

            apps["net.sdvision.sadeeq"].Apns.Start();



            #endregion



            #region Queue a notification to send

            // 7- Queue messages



            apps["net.sdvision.sadeeq"].Apns.QueueNotification(new ApnsNotification

            {

                DeviceToken = deviceId,

                Payload = JObject.Parse(appleJsonFormat)



            });



            #endregion



            #region Stop Sending Notifications

            //8- Stop the broker, wait for it to finish   

            // This isn't done after every message, but after you're

            // done with the broker

            apps["net.sdvision.sadeeq"].Apns.Stop();

            //apps["com.app.yourapp"].wsb.Stop();



            #endregion



        }



        public void SendNotificationToIOSNewFormat(int id, string type, string msg, string deviceId)

        {

            // 3- Create a broker dictionary

            var apps = new Dictionary<string, AppPushBrokers> { {APPLE_APP_NAME,

                                                                 new AppPushBrokers {

                                                                    Apns = new ApnsServiceBroker (apnsConfig),

                                                                }}};



            #region Wire Up Events

            // 4- events to fires onNotification sent or failure for each platform



            #region Apple

            apps[APPLE_APP_NAME].Apns.OnNotificationFailed += (notification, aggregateEx) =>

            {



                aggregateEx.Handle(ex =>

                {



                    // See what kind of exception it was to further diagnose

                    if (ex is ApnsNotificationException)

                    {

                        var apnsEx = ex as ApnsNotificationException;



                        // Deal with the failed notification

                        var n = apnsEx.Notification;

                        //  logger.Error("Notification Failed: ID={n.Identifier}, Code={apnsEx.ErrorStatusCode}");



                    }

                    else if (ex is ApnsConnectionException)

                    {

                        // Something failed while connecting (maybe bad cert?)

                        //    logger.Error("Notification Failed (Bad APNS Connection)!");



                    }

                    else

                    {

                        // logger.Error("Notification Failed (Unknown Reason)!");



                    }



                    // Mark it as handled

                    return true;

                });

            };



            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>

            {

                Console.WriteLine("Notification Sent!");

            };



            #endregion



            #endregion



            #region Prepare Notification



            // 5- prepare the json msg for android and ios and any platform you want

            string notificationMsg = msg;

            string jsonMessage = @"{""message"":""" + notificationMsg +

                                        @""",""msgcnt"":1,""sound"":""custom.mp3""}";



            //string appleJsonFormat = "{\"aps\": {\"alert\":" + '"' + notificationMsg + '"' + ",\"sound\": \"default\"}}";

            



            string catType = type;

            string catId = Convert.ToString(id);

           // string testsecondmesage = "{\"BranchID\":" + '"' + abc + '"' + ",\"sound\": \"default\"}";





            Dictionary<string, string> alertDictionary = new Dictionary<string, string>();

            alertDictionary.Add("alert", notificationMsg);

            alertDictionary.Add("sound", "default");



            Dictionary<string, string> catDetailDictionary = new Dictionary<string, string>();

            catDetailDictionary.Add("ID", catId);

            catDetailDictionary.Add("T", "1");



            Dictionary<string, Dictionary<string, string>> payloadDictionary = new Dictionary<string, Dictionary<string, string>>();

            payloadDictionary.Add("aps",alertDictionary);

            payloadDictionary.Add("Detail", catDetailDictionary);



            string json = JsonConvert.SerializeObject(payloadDictionary, new KeyValuePairConverter());

             

            #endregion



            #region Start Send Notifications

            // 6- start sending

            apps[APPLE_APP_NAME].Apns.Start();



            #endregion



            #region Queue a notification to send

            // 7- Queue messages



            apps[APPLE_APP_NAME].Apns.QueueNotification(new ApnsNotification

            {

                DeviceToken = "fUnFsLz_GDU:APA91bHAva8l908_ZSvqToxnHGOWz1QXTkJr7p7yLYjWKlPTBcoIFYDNVlSZx1OQE-9NGCkycpPLmsOcZmn1adPPNs4KoMjFdMcaCqyzzTMKd7HI7B0Ra8-c7Zc_GipZKq3wlVMmcIt1",

                //Payload = JObject.Parse(appleJsonFormat)

                Payload = JObject.Parse(json)

                

            });



            #endregion



            #region Stop Sending Notifications

            //8- Stop the broker, wait for it to finish   

            // This isn't done after every message, but after you're

            // done with the broker

            apps[APPLE_APP_NAME].Apns.Stop();

            //apps["com.app.yourapp"].wsb.Stop();



            #endregion



        }




        public string SendIPhonePushNotfication(string sId, string rId, string rName, string sName, string date, string token, string chatMsg)
        {

            int id = 1;
            string type = "test";
            string msg = "{ \"registration_ids\": [ \"28c3bc1ce8917eafaabc47c2adc3239f3c52c8fd279cf9387821612deb2da830\" ],\"data\": {\"notification_data\": [{\"alert\":\"hello this actual message\",\"Title\":\"irfan\",\"rId\":\"131\",\"sId\":\"130\",\"createdAt\":\"12/12/2016\"}]}}";
            string deviceId = token;// "28c3bc1ce8917eafaabc47c2adc3239f3c52c8fd279cf9387821612deb2da830";
            // 3- Create a broker dictionary

            var apps = new Dictionary<string, AppPushBrokers> { {APPLE_APP_NAME,

                                                                 new AppPushBrokers {

                                                                    Apns = new ApnsServiceBroker (apnsConfig),

                                                                }}};



            #region Wire Up Events

            // 4- events to fires onNotification sent or failure for each platform



            #region Apple

            apps[APPLE_APP_NAME].Apns.OnNotificationFailed += (notification, aggregateEx) =>
            {



                aggregateEx.Handle(ex =>
                {



                    // See what kind of exception it was to further diagnose

                    if (ex is ApnsNotificationException)
                    {

                        var apnsEx = ex as ApnsNotificationException;



                        // Deal with the failed notification

                        var n = apnsEx.Notification;

                        Console.Write("Notification Failed: ID={n.Identifier}, Code={apnsEx.ErrorStatusCode}");



                    }

                    else if (ex is ApnsConnectionException)
                    {

                        // Something failed while connecting (maybe bad cert?)

                        Console.Write("Notification Failed (Bad APNS Connection)!");



                    }

                    else
                    {

                        Console.Write("Notification Failed (Unknown Reason)!");



                    }



                    // Mark it as handled

                    return true;

                });

            };



            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>
            {

                Console.WriteLine("Notification Sent!");

            };



            #endregion



            #endregion



            #region Prepare Notification



            // 5- prepare the json msg for android and ios and any platform you want

            string notificationMsg = "hello this is actual message yar";

            string jsonMessage = @"{""message"":""" + notificationMsg +

                                        @""",""msgcnt"":1,""sound"":""custom.mp3"",""FullData"":"+msg+"}";



            //string appleJsonFormat = "{\"aps\": {\"alert\":" + '"' + notificationMsg + '"' + ",\"sound\": \"default\"}}";





            string catType = type;

            string catId = Convert.ToString(id);

            // string testsecondmesage = "{\"BranchID\":" + '"' + abc + '"' + ",\"sound\": \"default\"}";





            Dictionary<string, string> alertDictionary = new Dictionary<string, string>();

            alertDictionary.Add("T", "1");
            alertDictionary.Add("alert", sName+":"+chatMsg);

            alertDictionary.Add("sound", "default");
            alertDictionary.Add("sId",sId);
            alertDictionary.Add("rId", rId);
            alertDictionary.Add("rName", rName);
            alertDictionary.Add("sName", sName);
            alertDictionary.Add("timestamp", date);



            Dictionary<string, string> catDetailDictionary = new Dictionary<string, string>();

            catDetailDictionary.Add("ID", catId);

            catDetailDictionary.Add("Type", catType);



            Dictionary<string, Dictionary<string, string>> payloadDictionary = new Dictionary<string, Dictionary<string, string>>();

            payloadDictionary.Add("aps", alertDictionary);

            payloadDictionary.Add("Detail", catDetailDictionary);



            string json = JsonConvert.SerializeObject(payloadDictionary, new KeyValuePairConverter());



            #endregion



            #region Start Send Notifications

            // 6- start sending

            apps[APPLE_APP_NAME].Apns.Start();



            #endregion



            #region Queue a notification to send

            // 7- Queue messages

            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>
            {

                Console.WriteLine("Notification Sent!");

            };

            apps[APPLE_APP_NAME].Apns.QueueNotification(new ApnsNotification

            {

                DeviceToken = deviceId,

                //Payload = JObject.Parse(appleJsonFormat)

                Payload = JObject.Parse(json)



            });



            #endregion



            #region Stop Sending Notifications

            //8- Stop the broker, wait for it to finish   

            // This isn't done after every message, but after you're

            // done with the broker

            apps[APPLE_APP_NAME].Apns.Stop();

            //apps["com.app.yourapp"].wsb.Stop();



            #endregion
            return "ok";


        }



        public string SendIPhoneNotePushNotfication(string sId, string rId,string title, string description,string noteId,string sName, string token, string lati, string longi, string address, string color)
        {

            int id = 1;
            string type = "test";
            string resText = "";
            string msg = "{ \"registration_ids\": [ \"28c3bc1ce8917eafaabc47c2adc3239f3c52c8fd279cf9387821612deb2da830\" ],\"data\": {\"notification_data\": [{\"alert\":\"hello this actual message\",\"Title\":\"irfan\",\"rId\":\"131\",\"sId\":\"130\",\"createdAt\":\"12/12/2016\"}]}}";
            string deviceId = token;// "87a105b77b3764ce0f62d03f65a7413e9ff90016d9042c1e46c1174a7a7098a5";
            // 3- Create a broker dictionary

            var apps = new Dictionary<string, AppPushBrokers> { {APPLE_APP_NAME,

                                                                 new AppPushBrokers {

                                                                    Apns = new ApnsServiceBroker (apnsConfig),

                                                                }}};



            #region Wire Up Events

            // 4- events to fires onNotification sent or failure for each platform



            #region Apple

            apps[APPLE_APP_NAME].Apns.OnNotificationFailed += (notification, aggregateEx) =>
            {



                aggregateEx.Handle(ex =>
                {



                    // See what kind of exception it was to further diagnose

                    if (ex is ApnsNotificationException)
                    {

                        var apnsEx = ex as ApnsNotificationException;



                        // Deal with the failed notification

                        var n = apnsEx.Notification;

                        Console.Write("Notification Failed: ID={n.Identifier}, Code={apnsEx.ErrorStatusCode}");

                        resText = "Notification Failed!--" + n.Identifier + "---" + apnsEx.ErrorStatusCode;

                    }

                    else if (ex is ApnsConnectionException)
                    {

                        // Something failed while connecting (maybe bad cert?)

                        Console.Write("Notification Failed (Bad APNS Connection)!");

                        resText = "Notification Failed (Bad APNS Connection)!";

                    }

                    else
                    {

                        Console.Write("Notification Failed (Unknown Reason)!");

                        resText = "Notification Failed (Unknown Reason)!";

                    }



                    // Mark it as handled

                    return true;

                });

            };



            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>
            {

                Console.WriteLine("Notification Sent!");
                resText = "sent";

            };



            #endregion



            #endregion



            #region Prepare Notification



            // 5- prepare the json msg for android and ios and any platform you want

            string notificationMsg = "hello this is actual message yar";

            string jsonMessage = @"{""message"":""" + notificationMsg +

                                        @""",""msgcnt"":1,""sound"":""custom.mp3"",""FullData"":" + msg + "}";



            //string appleJsonFormat = "{\"aps\": {\"alert\":" + '"' + notificationMsg + '"' + ",\"sound\": \"default\"}}";





            string catType = type;

            string catId = Convert.ToString(id);

            // string testsecondmesage = "{\"BranchID\":" + '"' + abc + '"' + ",\"sound\": \"default\"}";





            Dictionary<string, string> alertDictionary = new Dictionary<string, string>();
            alertDictionary.Add("T", "2");
            alertDictionary.Add("alert", title);

            alertDictionary.Add("sound", "default");
            alertDictionary.Add("sId", sId);
            alertDictionary.Add("rId", rId);
            alertDictionary.Add("noteId", noteId);
            alertDictionary.Add("title", title);
            alertDictionary.Add("description", description);
            alertDictionary.Add("sName", sName);
            alertDictionary.Add("lati", lati);
            alertDictionary.Add("longi", longi);
            alertDictionary.Add("address", address);
            alertDictionary.Add("color", color);



            Dictionary<string, string> catDetailDictionary = new Dictionary<string, string>();

            catDetailDictionary.Add("ID", catId);

            catDetailDictionary.Add("Type", catType);



            Dictionary<string, Dictionary<string, string>> payloadDictionary = new Dictionary<string, Dictionary<string, string>>();

            payloadDictionary.Add("aps", alertDictionary);

            payloadDictionary.Add("Detail", catDetailDictionary);



            string json = JsonConvert.SerializeObject(payloadDictionary, new KeyValuePairConverter());



            #endregion



            #region Start Send Notifications

            // 6- start sending

            apps[APPLE_APP_NAME].Apns.Start();



            #endregion



            #region Queue a notification to send

            // 7- Queue messages

            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>
            {

                Console.WriteLine("Notification Sent!");
                resText = "Sent!";

            };

            apps[APPLE_APP_NAME].Apns.QueueNotification(new ApnsNotification

            {

                DeviceToken = deviceId,

                //Payload = JObject.Parse(appleJsonFormat)

                Payload = JObject.Parse(json)



            });



            #endregion



            #region Stop Sending Notifications

            //8- Stop the broker, wait for it to finish   

            // This isn't done after every message, but after you're

            // done with the broker

            apps[APPLE_APP_NAME].Apns.Stop();

            //apps["com.app.yourapp"].wsb.Stop();



            #endregion
            return resText;


        }


        public string SendIPhoneMeetingPushNotfication(string sId, string rId, string title,string meetingId, string sName, string token, string lati, string longi, string purpose, string address, string meetingDate, string meetingTime, string allowReminder, string receiver_name)
        {

            int id = 1;
            string type = "test";
            string msg = "{ \"registration_ids\": [ \"28c3bc1ce8917eafaabc47c2adc3239f3c52c8fd279cf9387821612deb2da830\" ],\"data\": {\"notification_data\": [{\"alert\":\"hello this actual message\",\"Title\":\"irfan\",\"rId\":\"131\",\"sId\":\"130\",\"createdAt\":\"12/12/2016\"}]}}";
            string deviceId = token;// "87a105b77b3764ce0f62d03f65a7413e9ff90016d9042c1e46c1174a7a7098a5";
            // 3- Create a broker dictionary

            var apps = new Dictionary<string, AppPushBrokers> { {APPLE_APP_NAME,

                                                                 new AppPushBrokers {

                                                                    Apns = new ApnsServiceBroker (apnsConfig),

                                                                }}};



            #region Wire Up Events

            // 4- events to fires onNotification sent or failure for each platform



            #region Apple

            apps[APPLE_APP_NAME].Apns.OnNotificationFailed += (notification, aggregateEx) =>
            {



                aggregateEx.Handle(ex =>
                {



                    // See what kind of exception it was to further diagnose

                    if (ex is ApnsNotificationException)
                    {

                        var apnsEx = ex as ApnsNotificationException;



                        // Deal with the failed notification

                        var n = apnsEx.Notification;

                        Console.Write("Notification Failed: ID={n.Identifier}, Code={apnsEx.ErrorStatusCode}");



                    }

                    else if (ex is ApnsConnectionException)
                    {

                        // Something failed while connecting (maybe bad cert?)

                        Console.Write("Notification Failed (Bad APNS Connection)!");



                    }

                    else
                    {

                        Console.Write("Notification Failed (Unknown Reason)!");



                    }



                    // Mark it as handled

                    return true;

                });

            };



            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>
            {

                Console.WriteLine("Notification Sent!");

            };



            #endregion



            #endregion



            #region Prepare Notification



            // 5- prepare the json msg for android and ios and any platform you want

            string notificationMsg = "hello this is actual message yar";

            string jsonMessage = @"{""message"":""" + notificationMsg +

                                        @""",""msgcnt"":1,""sound"":""custom.mp3"",""FullData"":" + msg + "}";



            //string appleJsonFormat = "{\"aps\": {\"alert\":" + '"' + notificationMsg + '"' + ",\"sound\": \"default\"}}";





            string catType = type;

            string catId = Convert.ToString(id);

            // string testsecondmesage = "{\"BranchID\":" + '"' + abc + '"' + ",\"sound\": \"default\"}";





            Dictionary<string, string> alertDictionary = new Dictionary<string, string>();
            alertDictionary.Add("T", "3");
            alertDictionary.Add("alert", title);

            alertDictionary.Add("sound", "default");
            alertDictionary.Add("sId", sId);
            alertDictionary.Add("rId", rId);
            alertDictionary.Add("meetingId", meetingId);
            alertDictionary.Add("title", title);
            alertDictionary.Add("sName", sName);
            alertDictionary.Add("rName", receiver_name);
            alertDictionary.Add("lati", lati);
            alertDictionary.Add("longi", longi);
            alertDictionary.Add("purpose", purpose);
            alertDictionary.Add("address", address);
            alertDictionary.Add("meetingDate", meetingDate);
            alertDictionary.Add("meetingTime", meetingTime);
            alertDictionary.Add("allowReminder", allowReminder);

            Dictionary<string, string> catDetailDictionary = new Dictionary<string, string>();

            catDetailDictionary.Add("ID", catId);

            catDetailDictionary.Add("Type", catType);



            Dictionary<string, Dictionary<string, string>> payloadDictionary = new Dictionary<string, Dictionary<string, string>>();

            payloadDictionary.Add("aps", alertDictionary);

            payloadDictionary.Add("Detail", catDetailDictionary);



            string json = JsonConvert.SerializeObject(payloadDictionary, new KeyValuePairConverter());



            #endregion



            #region Start Send Notifications

            // 6- start sending

            apps[APPLE_APP_NAME].Apns.Start();



            #endregion



            #region Queue a notification to send

            // 7- Queue messages

            apps[APPLE_APP_NAME].Apns.OnNotificationSucceeded += (notification) =>
            {

                Console.WriteLine("Notification Sent!");

            };

            apps[APPLE_APP_NAME].Apns.QueueNotification(new ApnsNotification

            {

                DeviceToken = deviceId,

                //Payload = JObject.Parse(appleJsonFormat)

                Payload = JObject.Parse(json)



            });



            #endregion



            #region Stop Sending Notifications

            //8- Stop the broker, wait for it to finish   

            // This isn't done after every message, but after you're

            // done with the broker

            apps[APPLE_APP_NAME].Apns.Stop();

            //apps["com.app.yourapp"].wsb.Stop();



            #endregion
            return "ok";


        }
        public void pushMessage(string deviceID)

        {

            int port = 2195;

            String hostname = "gateway.sandbox.push.apple.com";

            String certificatePath = HttpContext.Current.Server.MapPath("~/App_Data/final.p12");//System.Web.Hosting.HostingEnvironment.MapPath("ck_pizza.pem");

            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), "");

            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);

            SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);



            try

            {

                //sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, false);

                sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, false);

                MemoryStream memoryStream = new MemoryStream();

                BinaryWriter writer = new BinaryWriter(memoryStream);

                writer.Write((byte)0);

                writer.Write((byte)0);

                writer.Write((byte)32);



                writer.Write(HexStringToByteArray(deviceID.ToUpper()));

                String payload = "{\"aps\":{\"alert\":\"" + "Hi,, This Is a Sample Push Notification For IPhone.." + "\",\"badge\":1,\"sound\":\"default\"}}";

                writer.Write((byte)0);

                writer.Write((byte)payload.Length);

                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);

                writer.Write(b1);

                writer.Flush();

                byte[] array = memoryStream.ToArray();

                sslStream.Write(array);

                sslStream.Flush();

                client.Close();

            }

            catch (System.Security.Authentication.AuthenticationException ex)

            {

                client.Close();

            }

            catch (Exception e)

            {

                client.Close();

            }

        }





        public static byte[] HexStringToByteArray(string hex)

        {

            return Enumerable.Range(0, hex.Length)

                             .Where(x => x % 2 == 0)

                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))

                             .ToArray();

        }



        // The following method is invoked by the RemoteCertificateValidationDelegate.

        public static bool ValidateServerCertificate(

              object sender,

              X509Certificate certificate,

              X509Chain chain,

              SslPolicyErrors sslPolicyErrors)

        {

            if (sslPolicyErrors == SslPolicyErrors.None)

                return true;



            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);



            // Do not allow this client to communicate with unauthenticated servers.

            return false;

        }



        //--------------------------------------------------------------------------------------------------------------




        //----------------------------------------------------------------------------------------------------------------



        //----------------------------------------------------------------------------------------------------------------

        public bool ConnectToAPNS(string deviceId, string message)

        {

            X509Certificate2Collection certs = new X509Certificate2Collection();



            // Add the Apple cert to our collection

            certs.Add(getServerCert());



            // Apple development server address

            string apsHost;

            /*

            if (getServerCert().ToString().Contains("Production"))

                apsHost = "gateway.push.apple.com";

            else*/

            apsHost = "gateway.sandbox.push.apple.com";



            // Create a TCP socket connection to the Apple server on port 2195

            TcpClient tcpClient = new TcpClient(apsHost, 2195);



            // Create a new SSL stream over the connection

            SslStream sslStream1 = new SslStream(tcpClient.GetStream());



            // Authenticate using the Apple cert

            sslStream1.AuthenticateAsClient(apsHost, certs, SslProtocols.Default, false);

            try

            {

                //sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, false);

                //sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, false);

                MemoryStream memoryStream = new MemoryStream();

                BinaryWriter writer = new BinaryWriter(memoryStream);

                writer.Write((byte)0);

                writer.Write((byte)0);

                writer.Write((byte)32);



                writer.Write(HexStringToByteArray(deviceId.ToUpper()));

                String payload = "{\"aps\":{\"alert\":\"" + "Hi,, This Is a Sample Push Notification For IPhone.." + "\",\"badge\":1,\"sound\":\"default\"}}";

                writer.Write((byte)0);

                writer.Write((byte)payload.Length);

                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);

                writer.Write(b1);

                writer.Flush();

                byte[] array = memoryStream.ToArray();

                sslStream1.Write(array);

                sslStream1.Flush();

                tcpClient.Close();

            }

            catch (System.Security.Authentication.AuthenticationException ex)

            {

                tcpClient.Close();

            }

            catch (Exception e)

            {

                tcpClient.Close();

            }

            // pushMessage(deviceId);//, message, sslStream1);



            return true;

        }



        private static X509Certificate getServerCert()

        {

            X509Certificate test = new X509Certificate();



            //Open the cert store on local machine

            X509Store store = new X509Store(StoreLocation.CurrentUser);



            if (store != null)

            {

                // store exists, so open it and search through the certs for the Apple Cert

                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certs = store.Certificates;



                if (certs.Count > 0)

                {

                    int i;

                    for (i = 0; i < certs.Count; i++)

                    {

                        X509Certificate2 cert = certs[i];



                        if (cert.FriendlyName.Contains("Apple Development IOS Push Services"))

                        {

                            //Cert found, so return it.

                            Console.WriteLine("Found It!");

                            return certs[i];

                        }

                    }

                }

                return test;

            }

            return test;

        }



        private static byte[] HexToData(string hexString)

        {

            if (hexString == null)

                return null;



            if (hexString.Length % 2 == 1)

                hexString = '0' + hexString; // Up to you whether to pad the first or last byte



            byte[] data = new byte[hexString.Length / 2];



            for (int i = 0; i < data.Length; i++)

                data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);



            return data;

        }





        //---------------------------------------------------------------------------------------------------------------





        //--------------------------------------------------------------------------------------------------------






        //--------------------------------------------------------------------------------------------------------

        #endregion
        #endregion


    }

    public class AppPushBrokers
    {
        public ApnsServiceBroker Apns { get; set; }
        public GcmServiceBroker Gcm { get; set; }
        public WnsServiceBroker wsb { get; set; }

    }
}