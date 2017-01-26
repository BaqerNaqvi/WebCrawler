using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using WebCrawler;
using DataAccess;
using System.Data;
using System.Data.SqlClient;
using Utilities;
using System.Configuration;
using MySql.Data.MySqlClient;
using WebCrawler.Models;


namespace WebCrawler.DataCore.Managers
{
    public class AdminManager
    {

        private static BusinessCoreController controller = new BusinessCoreController();
        

        internal int AddHtmlData(crawler cr)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);


            //int medId = -1;
            try
            {
                connection.Open();
                string sql = "INSERT INTO crawler (placeId,placeWebsite,deviceId,deviceLati,deviceLongi,htmlData,createdAt,googleData) VALUES (@placeId,@placeWebsite,@deviceId,@deviceLati,@deviceLongi,@htmlData,current_timestamp,@googleData)  ; SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@placeId", cr.placeId);
                    cmd.Parameters.AddWithValue("@placeWebsite", cr.placeWebsite);
                    cmd.Parameters.AddWithValue("@deviceId", cr.deviceId);
                    cmd.Parameters.AddWithValue("@deviceLati", cr.deviceId);
                    cmd.Parameters.AddWithValue("@deviceLongi", cr.deviceLongi);
                    cmd.Parameters.AddWithValue("@htmlData", cr.htmlData);
                    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@googleData", cr.googleData);

                    cmd.ExecuteNonQuery();
                    return 1;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }


        internal void UpdateCrawlDataByPlaceId(string data, string id)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);
            //int medId = -1;
            try
            {
                connection.Open();
                string sql = "Update crawler set htmlData=@htmlData where placeId=@placeId";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@htmlData", data);
                    cmd.Parameters.AddWithValue("@placeId", id);

                    cmd.ExecuteNonQuery();
                    //return 1;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                // return medId;
            }
        }



        internal crawler GetHtmlDataById(int Id)
        {
            CustomQuery cq = new CustomQuery();
            crawler cr = new crawler();
            string query = "Select * from crawler where Id='" + Id + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                cr = mapDataToCrawler(dt.Rows[0]);
            }
            return cr;
        }
        internal List<crawler> GetCrawlerByUserId(int deviceId)
        {
            CustomQuery cq = new CustomQuery();
            List<crawler> re = new List<crawler>();
            string query = "select * from crawler where deviceId = " + deviceId + " order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    re.Add(mapDataToCrawler(row));
                }

            }
            return re;
        }
        internal List<crawler> GetAllCrawlData()
        {
            CustomQuery cq = new CustomQuery();
            List<crawler> re = new List<crawler>();
            string query = "select * from crawler order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    re.Add(mapDataToCrawler(row));
                }

            }
            return re;
        }
        internal crawler GetCrawlerByUserIdAndGps(string userId, string lati, string longi)
        {
            CustomQuery cq = new CustomQuery();
            crawler re = new crawler();
            string query = "select * from crawler where deviceId = '" + userId + "' and placeLati = '" + lati + "' and placeLongi = '" + longi + "' order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                re = mapDataToCrawler(dt.Rows[0]);

            }
            return re;
        }

        internal crawler GetCrawlerByPlaceId(string placeId)
        {
            CustomQuery cq = new CustomQuery();
            crawler re = new crawler();
            string query = "select * from crawler where placeId = '" + placeId + "'  order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                re = mapDataToCrawler(dt.Rows[0]);

            }
            return re;
        }

        internal void DeleteData(int Id)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from crawler where Id = " + Id + "";
            cq.ExecuteNonQuery(query);
        }

        private crawler mapDataToCrawler(DataRow row)
        {
            crawler cr = new crawler();

            cr.Id = int.Parse(row["Id"].ToString());
            cr.placeId = row["placeId"].ToString();

            cr.placeWebsite = row["placeWebsite"].ToString();

            cr.deviceId = row["deviceId"].ToString();
            cr.deviceLati = row["deviceLati"].ToString();
            cr.deviceLongi = row["deviceLongi"].ToString();
            cr.htmlData = row["htmlData"].ToString();
            cr.googleData = row["googleData"].ToString();
            cr.createdAt = DateTime.Parse(row["createdAt"].ToString());

            return cr;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }





        //manage words

        internal List<includedWord> IncludeWord(includedWord add)
        {
            CustomQuery cq = new CustomQuery();
            int wordId;
            string query = "Insert into includedWord (word,createdAt) values ('" + add.word + "',current_timestamp)  ; SELECT LAST_INSERT_ID();";

            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                wordId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                return GetIncludedWords();
            }
            else
            {
                return GetIncludedWords();
            }
        }
        internal List<includedWord> GetIncludedWords()
        {
            CustomQuery cq = new CustomQuery();
            List<includedWord> inc = new List<includedWord>();
            string query = "select * from includedWord order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    inc.Add(mapDataToIncludedWords(row));
                }

            }
            return inc;
        }
        internal List<includedWord> RemoveIncludedWord(int id)
        {
            CustomQuery cq = new CustomQuery();

            string query = "delete from includedWord where Id= '" + id + "'";
            cq.ExecuteNonQuery(query);
            return GetIncludedWords();
        }

        //Excluded Words




        internal List<excludedWord> AddExcludeWord(excludedWord add)
        {
            CustomQuery cq = new CustomQuery();
            int wordId;
            string query = "Insert into excludedWord (word,createdAt) values ('" + add.word + "',current_timestamp)  ; SELECT LAST_INSERT_ID();";

            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                wordId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                return GetExcludedWords();
            }
            else
            {
                return GetExcludedWords();
            }
        }
        internal List<excludedWord> GetExcludedWords()
        {
            CustomQuery cq = new CustomQuery();
            List<excludedWord> inc = new List<excludedWord>();
            string query = "select * from excludedWord order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    inc.Add(mapDataToExcludedWords(row));
                }

            }
            return inc;
        }
        internal List<excludedWord> RemoveExcludedWord(int id)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from excludedWord where Id= '" + id + "'";
            cq.ExecuteNonQuery(query);
            return GetExcludedWords();
        }







        //Categories

        internal List<category> AddCategory(category add)
        {
            CustomQuery cq = new CustomQuery();
            int wordId;
            string query = "Insert into categories (word,createdAt) values ('" + add.word+ "',current_timestamp)  ; SELECT LAST_INSERT_ID();";

            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                wordId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                return GetCategories();
            }
            else
            {
                return GetCategories();
            }
        }
        internal List<category> GetCategories()
        {
            CustomQuery cq = new CustomQuery();
            List<category> inc = new List<category>();
            string query = "select * from categories order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    inc.Add(mapDataToCategories(row));
                }

            }
            return inc;
        }

        internal List<category> RemoveCategory(int id)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from categories where Id= '" + id + "'";
            cq.ExecuteNonQuery(query);
            return GetCategories();
        }



        public includedWord mapDataToIncludedWords(DataRow row)
        {
            includedWord add = new includedWord();

            add.Id = int.Parse(row["Id"].ToString());
            add.word = row["word"].ToString();
            add.createdAt = DateTime.Parse(row["createdAt"].ToString());

            return add;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }

        public excludedWord mapDataToExcludedWords(DataRow row)
        {
            excludedWord ex = new excludedWord();

            ex.Id = int.Parse(row["Id"].ToString());
            ex.word = row["word"].ToString();
            ex.createdAt = DateTime.Parse(row["createdAt"].ToString());

            return ex;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }
        public category mapDataToCategories(DataRow row)
        {
            category ex = new category();

            ex.Id = int.Parse(row["Id"].ToString());
            ex.word = row["word"].ToString();
            ex.createdAt = DateTime.Parse(row["createdAt"].ToString());

            return ex;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }
        
        



        //admin part

        internal admin AuthenticateUser(string email, string password)
        {
            admin user = new admin();
            CustomQuery cq = new CustomQuery();
            string query = "Select * from admin where email='" + email + "' and password='" + password + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = mapDataToAdmin(dt.Rows[0]);
            }
            return user;
        }
        internal Vendor AuthenticateVendor(string email, string password, string role)
        {
         //   var tee = new JobManager();
           // tee.GenerateBill();

            Vendor user = new Vendor();
            CustomQuery cq = new CustomQuery();
            string query = "Select * from Vendor where email='" + email + "' and password='" + password + "'";//" and role='"+role+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = mapDataToVendor(dt.Rows[0]);

                query = "Select * from adinfo where vendorid=" + user.Id;
                dt = cq.ExecuteSQLQuery(query);
                if (dt.Rows.Count > 0)
                {
                    
                }
            }
            return user;
        }
        internal int CheckUserEmail(string email, string username)
        {

            CustomQuery cq = new CustomQuery();
            string query = "select * from admin where email='" + email +"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                return 1;
            }
            return 0;

        }

        internal void UpdatePasswordByEmail(string email, string password)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update admin set password ='" + password + "' where email='" + email + "' ";
            cq.ExecuteNonQuery(query);
        }

        internal void LoginUser(int id)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update admin set loginStatus ='1' where Id='" +id+"' ";
            cq.ExecuteNonQuery(query);
        }
        internal void LoginOutUser(int id)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update admin set loginStatus ='0' where Id='" + id + "' ";
            cq.ExecuteNonQuery(query);
        }

        internal int RegisterAdmin(admin admin)
        {

            CustomQuery cq = new CustomQuery();
            int adminId;
            string query = "Insert into admin (name, email, password,username,loginStatus) values ('" + admin.name + "','" + admin.email + "','" + admin.password + "','" + admin.username + "','0');  ; SELECT LAST_INSERT_ID();";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                adminId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                return adminId;
            }
            else
            {
                return 0;
            }
        }
        public admin mapDataToAdmin(DataRow row)
        {
            admin ex = new admin();

            ex.Id = int.Parse(row["Id"].ToString());
            ex.name = row["name"].ToString();
            ex.email = row["email"].ToString();
            ex.password = row["password"].ToString();
            ex.username = row["username"].ToString();

            return ex;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }




        internal List<Vendor> RegisterVendor(Vendor vendor)
        {
             // here
            CustomQuery cq = new CustomQuery();
            List<Vendor> ven = new List<Vendor>();
            if (AuthenticateVendorByEmail(vendor.email) != 1)
            {
                string query = "insert into Vendor (name, email, password,contactNumber,logoImage, unlock_code, description,website,tokenLimit, currentTokenCount,unlockCodeAge,role, createdAt) values ('" + vendor.name + "','" + vendor.email + "','" + vendor.password + "','" + vendor.contactNumber + "','default.png','" + vendor.unlock_code + "','" + vendor.description + "','" + vendor.website + "','" + vendor.tokenLimit + "','0','" + vendor.unlockCodeAge + "','"+vendor.role+"',current_timestamp)  ; SELECT LAST_INSERT_ID();";
                DataTable dt = cq.ExecuteSQLQuery(query);
                if (dt.Rows.Count > 0)
                {
                    query =
                        "insert into billinginfo (vendorid,lastmonthpayment,duepayment,lastpaymentdate,nextpaymentdate,totalpaid,createdat) " +
                        "values("+ dt.Rows[0].ItemArray[0]+ ",0,0,current_timestamp,DATE_ADD( current_timestamp, INTERVAL 1 month ),0,current_timestamp);";
                      dt = cq.ExecuteSQLQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        // do something 
                    }
                        ven = GetAllVendors();

                    return ven;
                }
                    return ven;
            }
            return ven;
        }

      

       

        internal int AuthenticateVendorByEmail(string email)
        {
            CustomQuery cq = new CustomQuery();
            int flag = 0;
            string query = "Select * from Vendor where email='" + email + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                flag = 1;
            }
            return flag;
        }
        internal void UpdateVendorImageName(int vendorId, string image)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update Vendor set logoImage = '"+image+"' where Id = "+vendorId+"";
            DataTable dt = cq.ExecuteSQLQuery(query);
        }
        internal void DeleteVendor(int vendorId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from Vendor where Id = " + vendorId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
        }
        internal Vendor GetVendorById(int Id)
        {
            CustomQuery cq = new CustomQuery();
            Vendor vendor = new Vendor();
            string query = "select * from Vendor where Id = "+Id+" order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                vendor = mapDataToVendor(dt.Rows[0]);
            }
            return vendor;
        }

        internal Vendor GetVendorByUnlockCode(string code, int userId)
        {
            CustomQuery cq = new CustomQuery();
            Vendor vendor = new Vendor();
            string query = "select * from Vendor where unlock_code = '"+code+"' order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {

                vendor = mapDataToVendor(dt.Rows[0]);
                controller.SendAllVendorNotesToNewUser(vendor.Id.ToString(), vendor.unlock_code.ToString(), userId.ToString());
                if (UpdateTokenCount(vendor,userId))
                {
                    vendor.description = "1";

                    //Setup user initial interests
                    if (!isUserInterestExist(userId))
                    {
                        AddUserProfile(vendor.Id, userId);
                    }
                    return vendor;
                }
                else
                {
                    vendor.description = "2";
                    return vendor;
                }
            }
            else
            {
                vendor.description = "0";
                return vendor;
            }
        }

        internal Vendor RenewUnlockCode(string code, int userId)
        {
            CustomQuery cq = new CustomQuery();
            Vendor vendor = new Vendor();
            string query = "select * from Vendor where unlock_code = '" + code + "' order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {

                vendor = mapDataToVendor(dt.Rows[0]);

                if (UpdateTokenCount(vendor, userId))
                {
                    UpdateUserToken(userId, vendor.unlock_code, vendor.unlockCodeAge);
                    vendor.description = "1";

                    return vendor;
                }
                else
                {
                    vendor.description = "2";
                    return vendor;
                }
            }
            else
            {
                vendor.description = "0";
                return vendor;
            }
        }

        internal bool isUserProfileExist(int userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from SettingProfile where userId = "+userId+"";

            DataTable dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                return true;
            }
            else{

                return false;
            }
        }

        internal bool isUserInterestExist(int userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from UserInterest where userId = " + userId + "";

            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        public void AddUserProfile(int vendorId, int userId)
        {

            CustomQuery cq = new CustomQuery();
            VendorProfile ui = new VendorProfile();
            string vendor = "select * from VendorProfile where vendorId = "+vendorId+"";
            DataTable vpDt = cq.ExecuteSQLQuery(vendor);
            int settingId = 0;
            if(vpDt.Rows.Count > 0)
            {
                foreach(DataRow row in vpDt.Rows)
                {
                    ui = mapDataToVendorProfile(row);

                    string query = "insert into SettingProfile (userId,profileName,color,colorId,isActive) values (" + userId + ",'" + ui.profileName + "','" + ui.color + "','" + ui.colorId + "'," + ui.isActive + ")  ; SELECT LAST_INSERT_ID();";
                    DataTable dt = cq.ExecuteSQLQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        settingId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                        AddUserInterest(settingId,ui.Id);
                    }
                }
                
                
            }

            
        }
        public void AddUserInterest(int settingId, int vendorSettingId)
        {
            CustomQuery cq = new CustomQuery();
            VendorInterest ui = new VendorInterest();
            string vendor = "select * from VendorInterest where settingId = " + vendorSettingId + "";
            DataTable vpDt = cq.ExecuteSQLQuery(vendor);
            if (vpDt.Rows.Count > 0)
            {
                foreach(DataRow row in vpDt.Rows)
                {
                    ui = mapDataToVendorInterest(row);
                    string query = "insert into UserInterest (settingId,categoryId,subCategoryId,radius,color,isActive) values (" + settingId + "," + ui.categoryId + "," + ui.subCategoryId + "," + ui.radius + ",'" + ui.color + "'," + ui.isActive + ")";
                    cq.ExecuteNonQuery(query);
                }
                
            }
            
        }

        internal bool UpdateTokenCount(Vendor vendor, int userId)
        {
            CustomQuery cq = new CustomQuery();

            if (int.Parse(vendor.tokenLimit.ToString()) > int.Parse(vendor.currentTokenCount.ToString()))
            {
                string query = "Update Vendor set currentTokenCount = '" + (int.Parse(vendor.currentTokenCount.ToString()) + 1) + "' where unlock_code = '" + vendor.unlock_code + "'";
                DataTable dt = cq.ExecuteSQLQuery(query);
                UpdateUserToken(userId, vendor.unlock_code,vendor.unlockCodeAge);
                
                
                return true;
            }
            else{
                return false;
            }
            
        }
        internal void UpdateUserToken(int userId, string code, string unlockCodeAge)
        {
            CustomQuery cq = new CustomQuery();
            string query = "Update UserInfo set unlock_code = '" + code+ "', createdAt = current_timestamp where Id = '" +userId+ "'";
            DataTable dt = cq.ExecuteSQLQuery(query);

        }

        internal List<Vendor> GetAllVendors()
        {
            CustomQuery cq = new CustomQuery();
            List<Vendor> vendor = new List<Vendor>();
            string query = "select * from Vendor order by createdAt DESC";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    vendor.Add(mapDataToVendor(row));
                }

            }
            return vendor;
        }


        public Vendor mapDataToVendor(DataRow row)
        {
            Vendor ex = new Vendor();

            ex.Id = int.Parse(row["Id"].ToString());
            ex.tokenLimit = int.Parse(row["tokenLimit"].ToString());
            ex.currentTokenCount = int.Parse(row["currentTokenCount"].ToString());
            ex.name = row["name"].ToString();
            ex.email = row["email"].ToString();
            ex.password = row["password"].ToString();
            ex.contactNumber = row["contactNumber"].ToString();
            ex.logoImage = row["logoImage"].ToString();
            ex.unlock_code = row["unlock_code"].ToString();
            ex.unlockCodeAge = row["unlockCodeAge"].ToString();
            ex.description = row["description"].ToString();
            ex.website = row["website"].ToString();
            ex.createdAt = DateTime.Parse(row["createdAt"].ToString());
            ex.role= int.Parse(row["role"].ToString());
            ex.AdInfoes= new List<AdInfo>();
            return ex;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }

        public VendorProfile mapDataToVendorProfile(DataRow row)
        {
            VendorProfile ex = new VendorProfile();

            ex.Id = int.Parse(row["Id"].ToString());
            ex.vendorId = int.Parse(row["vendorId"].ToString());
            ex.colorId = int.Parse(row["colorId"].ToString());
            ex.isActive = int.Parse(row["isActive"].ToString());
            ex.profileName = row["profileName"].ToString();
            ex.color = row["color"].ToString();


            return ex;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }
        public VendorInterest mapDataToVendorInterest(DataRow row)
        {
            VendorInterest ex = new VendorInterest();

            ex.Id = int.Parse(row["Id"].ToString());
            ex.settingId = int.Parse(row["settingId"].ToString());
            ex.categoryId = int.Parse(row["categoryId"].ToString());
            ex.subCategoryId = int.Parse(row["subCategoryId"].ToString());
            ex.radius = int.Parse(row["radius"].ToString());
            ex.isActive = int.Parse(row["isActive"].ToString());
            ex.color = row["color"].ToString();


            return ex;
            //placeId,placeName,placeAddress,placeLati,placeLongi,placeWebsite,placeType,google_browserKey,deviceId,deviceLati,deviceLongi,htmlData,createdAt
        }

        public AdInfo mapAdInfo(DataRow row)
        {
            var ex = new AdInfo
            {
                Id = int.Parse(row["Id"].ToString()),
                vendorId= int.Parse(row["vendorId"].ToString()),
                customInterest = row["customInterest"].ToString(),
                mapVideo = row["mapVideo"].ToString(),
                sponorLogo = row["sponorLogo"].ToString(),
                sponsorWebsite = row["sponsorWebsite"].ToString(),
                createdAt = Convert.ToDateTime(row["createdAt"].ToString()),
                adTitle = row["adTitle"].ToString(),
                lati = row["lati"].ToString(),
                longi = row["longi"].ToString(),
                locationName = row["locationName"].ToString(),
            };
            return ex;
        }


        public AdInfoContainer mapAdInfoForApi(DataRow row, string baseUrl)
        {
            var ex = new AdInfoForApi
            {
                text = row["adTitle"].ToString(),   
                type = int.Parse(row["adTypeId"].ToString()),
                couponUrl = row["couponUrl"].ToString(),
                symbol_url = row["couponUrl"].ToString(),
                advertiser_logo = row["sponorLogo"].ToString(),
                advertiser_url = row["sponsorWebsite"].ToString(),
                advertiser_tagline = "Best Option",
                phone_number = row["sponsorPhone"].ToString(),
                interestId= int.Parse(row["interestId"].ToString()),
            };
            var img = row["mapImage"].ToString();
            if (img != "default.png")
            {
                ex.landmarkType = 0;
                var mapImage = baseUrl+"/UploadedFiles/AdImages/" +img;
                ex.landmark_detail_url = mapImage;
            }
            var vid = row["mapVideo"].ToString();
            if (vid != "default.mp4")
            {
                ex.landmarkType = 1;
                ex.landmark_detail_url = vid;
            }


            var placeInfo = row["sponsorFacts"].ToString();
            return new AdInfoContainer {ad_info = ex, place_info = placeInfo };
        }
    }
}