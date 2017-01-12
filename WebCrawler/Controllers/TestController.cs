



using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Google;
using PushSharp.Windows;
using System;
using System.Collections.Generic;
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



namespace WebCrawler.Controllers
{

    public class AppPushBrokers
    {
        public ApnsServiceBroker Apns { get; set; }
        public GcmServiceBroker Gcm { get; set; }
        public WnsServiceBroker wsb { get; set; }

    }
    public class TestController : ApiController
    {
        PushNotificationClass pushClass = new PushNotificationClass();

        public string push(JObject data)
        {
            pushClass.SendNotificationToIOSNewFormat1();
            return "Ok";
        }
    }

    public class PushNotificationClass
    {

        #region  Constants



        //public const string ANDROID_SENDER_AUTH_TOKEN = "AIzaSyCIplhvmqJhrGMuM-seJlMi5i_7K0auyEA";

        public const string ANDROID_SENDER_AUTH_TOKEN = "AIzaSyCdiACV4Q6WHSQqO-J7_JI0TB_kisTIrus";

        //public const string WINDOWS_PACKAGE_NAME = "yyyy";

        //public const string WINDOWS_PACKAGE_SECURITY_IDENTIFIER = "zzzz";

        //public const string WINDOWS_CLIENT_SECRET = "hhhh";

        public const string APPLE_APP_NAME = "com.ios.qlumidemo";

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

        public PushNotificationClass()

        {

            string ProductionCertificate = WebConfigurationManager.AppSettings["ProductionCertificate"];

            // Initialize

            useProductionCertificate = false;// Convert.ToBoolean(ProductionCertificate); // you can set it dynamically from config



            appleCertificateType = useProductionCertificate == true ? "q.pem" : "qlumi.p12";   // this is working for production

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

            catDetailDictionary.Add("Type", catType);



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




        public string SendNotificationToIOSNewFormat1()
        {

            int id = 1;
            string type = "test";
            string msg = "{ \"registration_ids\": [ \"28c3bc1ce8917eafaabc47c2adc3239f3c52c8fd279cf9387821612deb2da830\" ],\"data\": {\"notification_data\": [{\"alert\":\"hello this actual message\",\"Title\":\"irfan\",\"rId\":\"131\",\"sId\":\"130\",\"createdAt\":\"12/12/2016\"}]}}";
            string deviceId = "28c3bc1ce8917eafaabc47c2adc3239f3c52c8fd279cf9387821612deb2da830";
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

            alertDictionary.Add("alert", notificationMsg);

            alertDictionary.Add("sound", "default");
            alertDictionary.Add("sId","130");
            alertDictionary.Add("rId", "131");
            alertDictionary.Add("timestamp", "13/12/2016");



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

    }

}