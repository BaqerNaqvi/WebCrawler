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
using OpenTokSDK;
using Newtonsoft;
using MySql.Data.MySqlClient;
using WebCrawler.Models;


namespace WebCrawler.DataCore.Managers
{
    public class UsersManager
    {
        #region User

        internal UserInfo AuthenticateUser(string email, string password, string gcmToken)
        {
            DataUser user = new DataUser();
            CustomQuery cq = new CustomQuery();
            int daysLeft = 0;
            string query = "Select * from UserInfo where email='" + email + "' and password='" + password + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                UpdateGcmTokenByEmail(email, gcmToken);
                CreateTokBoxSessionAndToken(dt.Rows[0].ItemArray[0].ToString());
                user = GetUserByIdWithNoVendor(int.Parse(dt.Rows[0].ItemArray[0].ToString()));
                
                daysLeft = ValidateUnlockCode(user.unlock_code.ToString(), DateTime.Parse(user.createdAt.ToString()));
                if (daysLeft > 0)
                {
                    user.unlock_code = user.unlock_code;
                }
                else
                {
                    user.unlock_code = "0";
                }

                user.codeValidUntil = daysLeft.ToString();
            }
            return user;
        }
        internal void CreateTokBoxSessionAndToken(string userId)
        {
            int ApiKey = 45620662; // YOUR API KEY
            string ApiSecret = "4c3f57ba82e30ffb75571b97be53400aacfce5f0";

            //Create session

            var OpenTok = new OpenTok(ApiKey, ApiSecret);
            string sId = OpenTok.CreateSession().Id;

            //Create Token
            string tempSessionId = "2_MX40NTYyMDY2Mn5-MTQ3MTM0MTMxOTY2Nn45Qk5UY2NnV1B4OXI5NklrSWpmN2ZQc2h-UH4";
            Role role = new Role();
            double inOneWeek = (DateTime.UtcNow.Add(TimeSpan.FromDays(30)).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string connectionMetadata = "{\"userId\":"+userId+"}";
            string token = OpenTok.GenerateToken(tempSessionId,Role.PUBLISHER, inOneWeek, connectionMetadata);


            //string tempToken = "T1==cGFydG5lcl9pZD00NTYyMDY2MiZzaWc9YTcyNTEyNzFmNTg5NDY2NzcyNjRiN2I5MmUzOTA2Y2ZjMDBkN2VkMzpzZXNzaW9uX2lkPTJfTVg0ME5UWXlNRFkyTW41LU1UUTNNVE0wTVRNeE9UWTJObjQ1UWs1VVkyTm5WMUI0T1hJNU5rbHJTV3BtTjJaUWMyaC1VSDQmY3JlYXRlX3RpbWU9MTQ3MTM0MTMyMCZub25jZT0yNzEwNjgmcm9sZT1QVUJMSVNIRVI=";
            
            //UpdateTokBoxInfo(userId, token, sId);
            UpdateTokBoxInfo(userId, token, tempSessionId);

        }
        internal void UpdateTokBoxInfo(string userId, string token, string sessionId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update UserInfo set sessionId= '" +sessionId + "', loginStatus = '"+token+"' where Id = '"+userId + "'";
            cq.ExecuteNonQuery(query);
        }

        internal DataTable UpdateTokBoxConnectionId(string userId, string connectionId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update UserInfo set connectionId = '"+connectionId+"' where Id = '" + userId + "'";
            
            cq.ExecuteNonQuery(query);

            DataTable dt = FetchTokBoxConnectionId(userId);
            return dt;

        }
        internal DataTable FetchTokBoxConnectionId(string userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select connectionId from UserInfo where Id = '" + userId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        internal int AddChatMessage(ChatHistory chat)
        {
            CustomQuery cq = new CustomQuery();
            int chatId = 0;
            string query = "insert into ChatHistory (sId, rId, sName,rName, messageType, message, messageDate, createdAt) values (" + chat.sId + "," + chat.rId + ",'" + chat.sName + "','" + chat.rName + "','" + chat.messageType + "','" + chat.message + "','" + chat.messageDate + "',CURRENT_TIMESTAMP) ; SELECT LAST_INSERT_ID();";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                UpdateUnreadCount(int.Parse(chat.rId.ToString()), 1);
                chatId = int.Parse(dt.Rows[0].ItemArray[0].ToString());

            }
            return chatId;
        }
        internal void UpdateUnreadCount(int userId, int countValue)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select unreadCount from messageCount where userId = " + userId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                int updatedCount = int.Parse(dt.Rows[0].ItemArray[0].ToString()) + countValue;
                string update = "update messageCount set unreadCount = " + updatedCount + " where userId = "+userId+"";
                cq.ExecuteNonQuery(update);
            }
            else
            {
                string insert = "insert into messageCount values ('" + userId + "','0');";
                cq.ExecuteSQLQuery(insert);
            }
        }
        internal DataTable GetChatHistory(string sId, string rId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from ChatHistory where sId = '" + sId + "' and rId = '" + rId + "' ORDER BY createdAt ASC";
            DataTable dt = cq.ExecuteSQLQuery(query);

            string query1 = "select * from ChatHistory where sId = '" + rId + "' and rId = '" + sId + "' ORDER BY createdAt ASC";
            DataTable dt1 = cq.ExecuteSQLQuery(query1);
            string update = "update messageCount set unreadCount = 0 where userId = "+rId+"";
            cq.ExecuteNonQuery(update);
            dt.Merge(dt1);

            DataView dv = dt.DefaultView;
            dv.Sort = "createdAt ASC";

            DataTable dt3 = dv.ToTable();

            return dt3;
        }
        internal int ValidateUnlockCode(string unlockCode, DateTime createdAt)
        {
            CustomQuery cq = new CustomQuery();
            int daysLeft = 0;
            string query = "select unlock_code, createdAt,unlockCodeAge from Vendor where unlock_code = '"+unlockCode+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                int vendorTokenAge = int.Parse(dt.Rows[0].ItemArray[2].ToString());
                double tokenAge = DateTime.Today.Subtract(createdAt).TotalDays;
                daysLeft = vendorTokenAge - Convert.ToInt32(tokenAge);
            }
            return daysLeft;
        }
        

        internal int CheckUserEmail(string email, string username)
        {

            CustomQuery cq = new CustomQuery();
            string query = "select * from UserInfo where email='" + email + "'";
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
            string query = "update UserInfo set password ='" + password + "' where email='" + email + "' ";
            cq.ExecuteNonQuery(query);
        }
        internal void StorePasswordResetCodeByEmail(string email, string resetCode)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update UserInfo set connectionId ='" + resetCode + "' where email='" + email + "' ";
            cq.ExecuteNonQuery(query);
        }
        internal int UpdatePasswordByResetCode(string password, string resetCode)
        {
            CustomQuery cq = new CustomQuery();
            string resetQuery = "select * from UserInfo where connectionId = '" + resetCode + "'";
            DataTable dt = cq.ExecuteSQLQuery(resetQuery);
            if (dt.Rows.Count > 0)
            {
                string query = "update UserInfo set password ='" + password + "' where connectionId='" + resetCode + "' ";
                cq.ExecuteNonQuery(query);
                return int.Parse(dt.Rows[0].ItemArray[0].ToString());
            }
            else 
            {
                return 0;
            }
            
        }
        internal void UpdateGcmTokenByEmail(string email, string gcmToken)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update UserInfo set gcm_token ='" + gcmToken + "' where email='" + email + "' ";
            cq.ExecuteNonQuery(query);
        }
        internal DataTable GetGcmTokenByUserId(string userId)
        {
            CustomQuery cq = new CustomQuery();
            string gcmToken = "";
            string query = "select requestOrigin,gcm_token from UserInfo where Id = '"+userId+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        internal DataTable GetVendorRemainingUnlockCodes(string email)
        {
            CustomQuery cq = new CustomQuery();
            string gcmToken = "";
            string query = "select * from vendor where email = '"+email+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                gcmToken = dt.Rows[0].ItemArray[0].ToString();

            }

            return dt;
        }
        internal void UpdateGcmToken(string userId, string gcmToken)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update UserInfo set gcm_token ='" + gcmToken + "' where Id='" + userId + "' ";
            cq.ExecuteNonQuery(query);
        }
        internal void SyncProfileInformation1(string userId, string info, string updatedAt)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update UserInfo set notesInfo ='" + info + "', createdAt = STR_TO_DATE('"+updatedAt+"','%d/%m/%Y %H:%i:%s') where Id="+userId + "";
            cq.ExecuteNonQuery(query);
        }

        internal void SyncProfileInformation(string userId, string notesInfo, string updatedAt)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);

            //int medId = -1;
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                string sql = "update UserInfo set notesInfo = @notesInfo where Id = "+userId+"";


                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@notesInfo", notesInfo);
                    //cmd.Parameters.AddWithValue("@createdAt", updatedAt);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        internal int RegisterUser(UserInfo user)
        {

            CustomQuery cq = new CustomQuery();
            int adminId;
            UserInfo row = new UserInfo();

            row = AuthenticateUserContactByPhoneNumber(user.contactNumber);
            if(row.Id != 0)
            {
                return 2;
            }
            row = AuthenticateUserByEmail(user.email);
            if(row.Id != 0)
            {
                return 3;
            }
            string query = "Insert into UserInfo (name, email, password,gcm_token,unlock_code,requestOrigin,app_version,profileImage,contactNumber,qlumi_userId,connectionId,sessionId,loginStatus,notesInfo,createdAt) values ('" + user.name + "','" + user.email + "','" + user.password + "','" + user.gcm_token + "','" + user.unlock_code + "','" + user.requestOrigin + "','" + user.app_version + "','" + user.profileImage + "','" + user.contactNumber + "','','','','0','', current_timestamp) ; SELECT LAST_INSERT_ID();";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                adminId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                CreateUserUniqueId(adminId);
                string insert = "INSERT INTO userLocation (userId,lati,longi,createdAt,updatedAt) VALUES ('" + adminId + "','0', '0', current_timestamp,current_timestamp) ; SELECT LAST_INSERT_ID();";
                cq.ExecuteSQLQuery(insert);
                return adminId;
            }
            else
            {
                return 0;
            }
        }
        internal void CreateUserUniqueId(int userId)
        {
            CustomQuery cq = new CustomQuery();
            DataUser user = new DataUser();
            user = GetUserByIdWithNoCode(userId);
            if(user.Id != 0)
            {
                string query = "update UserInfo set qlumi_userId = '" + user.name + "_" + user.Id + "' where Id = " + userId + "";
                cq.ExecuteNonQuery(query);
            }
            
            
        }

        internal int UpdatePasswordByModel(UpdatePassword data)
        {
            CustomQuery cq = new CustomQuery();
            
            string query = "select * from vendor where password='" + data.CurrentPass + "' and Id=" + data.UserId;
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                query = "update vendor set password ='" + data.NewPass + "' where Id=" + data.UserId;
                var result = cq.ExecuteNonQuery(query);
                return result;
            }
            return 0;
        }
        public DataUser GetUserById(int Id)
        {
            DataUser user = new DataUser();
            user.unlock_code = "0";
            CustomQuery cq = new CustomQuery();
            string query = "Select UserInfo.Id,UserInfo.name,UserInfo.email,UserInfo.password,UserInfo.contactNumber,UserInfo.profileImage,UserInfo.unlock_code,UserInfo.gcm_token,UserInfo.requestOrigin,UserInfo.app_version,UserInfo.createdAt,UserInfo.qlumi_userId,UserInfo.connectionId,UserInfo.sessionId,UserInfo.loginStatus,UserInfo.notesInfo, Vendor.logoImage as BrandLogo, userLocation.lati, userLocation.longi from UserInfo inner join Vendor on UserInfo.unlock_code = Vendor.unlock_code inner join userLocation on userLocation.userId = UserInfo.Id where UserInfo.Id = " + Id + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = mapDataToUser(dt.Rows[0]);
                user.Profiles = GetAppUserProfileSettings(user.Id);
                user.BrandLogo = "http://qlumi.com/UploadedFiles/"+dt.Rows[0].ItemArray[16].ToString();
                user.iAmVisibleToContact = 0;
                user.latitude = dt.Rows[0].ItemArray[17].ToString();
                user.longitude = dt.Rows[0].ItemArray[18].ToString();
                user.codeValidUntil = ValidateUnlockCode(user.unlock_code.ToString(), DateTime.Parse(user.createdAt.ToString())).ToString();
            }
            return user;
        }
        public DataUser GetUserByIdWithNoVendor(int Id)
        {
            DataUser user = new DataUser();
            user.unlock_code = "0";
            CustomQuery cq = new CustomQuery();
            string query = "select UserInfo.Id, UserInfo.name, UserInfo.email, UserInfo.password,UserInfo.contactNumber,UserInfo.profileImage,UserInfo.unlock_code,UserInfo.gcm_token,UserInfo.requestOrigin,UserInfo.app_version,UserInfo.createdAt,UserInfo.qlumi_userId,UserInfo.connectionId,UserInfo.sessionId,UserInfo.loginStatus,UserInfo.notesInfo , userLocation.lati, userLocation.longi from UserInfo  inner join userLocation on userLocation.userId = UserInfo.Id where UserInfo.Id = " + Id + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = mapDataToUser(dt.Rows[0]);
                user.Profiles = GetAppUserProfileSettings(user.Id);
                user.BrandLogo = "http://qlumi.com/UploadedFiles/default.png";
                user.iAmVisibleToContact = 0;
                user.unlock_code = dt.Rows[0].ItemArray[6].ToString();
                user.latitude = dt.Rows[0].ItemArray[16].ToString();
                user.longitude = dt.Rows[0].ItemArray[17].ToString();
                user.codeValidUntil = "";
            }
            return user;
        }
        public DataUser GetUserByIdWithNoCode(int Id)
        {
            DataUser user = new DataUser();
            user.unlock_code = "0";
            CustomQuery cq = new CustomQuery();
            string query = "Select * from UserInfo where Id = " + Id + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = mapDataToUser(dt.Rows[0]);
                user.Profiles = null;
                user.BrandLogo = null;
            }
            return user;
        }
        internal DataTable GetAllUsers()
        {
            DataTable user = new DataTable();
            CustomQuery cq = new CustomQuery();
            string query = "select * from UserInfo";

            user = cq.ExecuteSQLQuery(query);

            return user;
        }

        internal DataTable GetAllUsersByUnlockCode(string unlock)
        {
            DataTable user = new DataTable();
            CustomQuery cq = new CustomQuery();
            string query = "select gcm_token, requestOrigin from UserInfo where unlock_code = '"+unlock+"'";

            user = cq.ExecuteSQLQuery(query);

            return user;
        }
        internal DataTable GetVendorUsers(string unlock_code, string vendorId)
        {
            DataTable user = new DataTable();
            CustomQuery cq = new CustomQuery();
            string query = "select * from UserInfo where unlock_code = '"+unlock_code+"'";

            user = cq.ExecuteSQLQuery(query);

            return user;
        }
        internal void UpdateUserInfo(UserInfo info)
        {
            CustomQuery cq = new CustomQuery();
            string query = "update UserInfo set name = '"+info.name+"', email = '"+info.email+"',contactNumber = '"+info.contactNumber+"', password = '"+info.password+"' where Id = "+info.Id+" ";
            cq.ExecuteNonQuery(query);
        }


        public DataUser mapDataToUser(DataRow row)
        {
            DataUser info = new DataUser();

            info.Id = int.Parse(row["Id"].ToString());
            info.qlumi_userId = row["qlumi_userId"].ToString();
            info.loginStatus = row["loginStatus"].ToString();
            info.name = row["name"].ToString();
            info.email = row["email"].ToString();
            info.password = row["password"].ToString();
            info.gcm_token = row["gcm_token"].ToString();
            info.unlock_code = row["unlock_code"].ToString();
            info.requestOrigin = row["requestOrigin"].ToString();
            info.app_version = row["app_version"].ToString();
            info.profileImage = row["profileImage"].ToString();
            info.contactNumber = row["contactNumber"].ToString();
            info.connectionId = row["connectionId"].ToString();
            info.sessionId = row["sessionId"].ToString();
            info.notesInfo = row["notesInfo"].ToString();
            info.createdAt = DateTime.Parse(row["createdAt"].ToString());

            return info;

        }

        #endregion 




        #region Interests

        internal List<AppCategory> AddCategory(AppCategory cat)
        {
            CustomQuery cq = new CustomQuery();
            List<AppCategory> catList = new List<AppCategory>();
            string query = "Insert into AppCategory (categoryName,categoryType,iconName,iconMarkerName,color,createdAt) values ('" + cat.categoryName + "','" + cat.categoryType + "','" + cat.iconName + "','" + cat.iconMarkerName + "','" + cat.color+ "' current_timestamp) ; SELECT LAST_INSERT_ID();";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return catList = GetAllCategories();

        }
        public List<AppCategory> GetAllCategories()
        {
            CustomQuery cq = new CustomQuery();
            List<AppCategory> catList = new List<AppCategory>();
            string query = "select * from AppCategory";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    catList.Add(mapDataToAppCategory(row));
                }
                return catList;
            }
            else
            {
                return catList;
            }
        }
        internal List<AppCategory> DeleteCategory(int catId)
        {
            CustomQuery cq = new CustomQuery();
            List<AppCategory> catList = new List<AppCategory>();
            string query = "delete from AppCategory where Id = "+catId+"";
            cq.ExecuteNonQuery(query);
            return GetAllCategories();
        }

        public AppCategory mapDataToAppCategory(DataRow row)
        {
            AppCategory info = new AppCategory();

            info.Id = int.Parse(row["Id"].ToString());
            info.categoryName = row["categoryName"].ToString();
            info.categoryType = row["categoryType"].ToString();
            info.iconName = row["iconName"].ToString();
            info.iconMarkerName = row["iconMarkerName"].ToString();
            info.color = row["color"].ToString();
            info.createdAt = DateTime.Parse(row["createdAt"].ToString());

            return info;

        }


        public ProfileCategory mapDataToAppProfileCategory(DataRow row)
        {
            ProfileCategory info = new ProfileCategory();

            info.categoryName = row["categoryName"].ToString();
            info.categoryType = row["categoryType"].ToString();
            info.iconName = row["iconName"].ToString();
            info.iconMarkerName = row["iconMarkerName"].ToString();
            info.color = row["color"].ToString();
            info.categoryId = int.Parse(row["Id"].ToString());
            info.interests = GetAllSubCategoriesForApp(info.categoryId);

            return info;

        }

        public List<ProfileCategory> GetAllCategoriesForAppByProfileId(int profileId)
        {
            CustomQuery cq = new CustomQuery();
            List<ProfileCategory> catList = new List<ProfileCategory>();
            string query = "select DISTINCT  AppCategory.categoryName, AppCategory.categoryType, AppCategory.iconName,AppCategory.iconMarkerName, AppCategory.color,AppCategory.Id from AppCategory inner join userinterest on AppCategory.Id =userinterest.categoryId where userinterest.settingId = "+profileId+"";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    catList.Add(mapDataToAppProfileCategory(row));
                }
                return catList;
            }
            else
            {
                return catList;
            }
        }


        public int AddVendorInterest(VendorInterest vi)
        {
            DataTable dt = new DataTable();
            CustomQuery cq = new CustomQuery();
            int interestId = 0;
            string query = "insert into VendorInterest (settingId,categoryId,subCategoryId,radius,color,isActive) values (" + vi.settingId + "," + vi.categoryId + "," + vi.subCategoryId + "," + vi.radius + ",'" + vi.color + "'," + vi.isActive + ") ; SELECT LAST_INSERT_ID();";
            dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                interestId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
            }
            return interestId;
        }

        public void AddUserInterest(UserInterest vi)
        {
            DataTable dt = new DataTable();
            CustomQuery cq = new CustomQuery();
            string query = "insert into UserInterest (settingId, categoryId,subCategoryId,radius,color,isActive) values (" + vi.settingId + "," + vi.categoryId + "," + vi.subCategoryId + "," + vi.radius + ",'" + vi.color + "'," + vi.isActive + ") ; SELECT LAST_INSERT_ID();";
            dt = cq.ExecuteSQLQuery(query);
            
        }

        public DataTable GetVendorInterestsByVendorId(int profileId)
        {
            DataTable dt = new DataTable();
            CustomQuery cq = new CustomQuery();
            string query = "select DISTINCT  AppCategory.categoryName as categoryName, SubCategory.subCategoryName as subCategoryName, VendorInterest.radius as radius, VendorInterest.color as color,VendorInterest.isActive as isActive from AppCategory inner join VendorInterest on AppCategory.Id = VendorInterest.categoryId inner join Subcategory on SubCategory.Id = VendorInterest.subCategoryId where VendorInterest.settingId = "+profileId+"";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        public DataTable GetVendorInterests(int profileId)
        {
            DataTable dt = new DataTable();
            CustomQuery cq = new CustomQuery();
            string query = "select DISTINCT  AppCategory.categoryName as categoryName, SubCategory.subCategoryName as subCategoryName, VendorInterest.radius as radius, VendorInterest.color as color,VendorInterest.isActive as isActive from AppCategory inner join VendorInterest on AppCategory.Id = VendorInterest.categoryId inner join Subcategory on SubCategory.Id = VendorInterest.subCategoryId where VendorInterest.settingId = "+profileId+"";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        internal void SetUserInterestByUnlockCode(int userId, string unlock_code)
        {

        }
        public DataUser GetVendorById(int Id)
        {
            DataUser user = new DataUser();
            user.unlock_code = "0";
            CustomQuery cq = new CustomQuery();
            string query = "Select * from Vendor where Id = " + Id + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = mapDataToUser(dt.Rows[0]);
                user.Profiles = GetAppUserProfileSettings(user.Id);
                user.BrandLogo = "http://qlumi.com/UploadedFiles/" + dt.Rows[0].ItemArray[24].ToString();
            }
            return user;
        }

        //SubCategory Section 



        internal SubCategory AddSubCategory(SubCategory cat)
        {
            CustomQuery cq = new CustomQuery();
            SubCategory subCat = new SubCategory();
            string query = "Insert into SubCategory values ('" + cat.categoryId + "','" + cat.subCategoryName + "','" + cat.subCategoryType + "','" + cat.iconName + "','" + cat.iconMarkerName + "','" + cat.color + "','" + cat.isCustom + "', current_timestamp) ; SELECT LAST_INSERT_ID();";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                subCat = GetSubCategoryById(int.Parse(dt.Rows[0].ItemArray[0].ToString()));
            }
            return subCat;

        }

        internal SubCategory GetSubCategoryById(int catId)
        {
            CustomQuery cq = new CustomQuery();
            SubCategory cat = new SubCategory();
            string query = "select * from SubCategory where Id = "+catId+"";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                cat = mapDataToAppSubCategory(dt.Rows[0]);
            }

            return cat;

        }
        internal List<SubCategory> RemoveSubCategory(int catId, int subCatId)
        {
            CustomQuery cq = new CustomQuery();
            List<SubCategory> catList = new List<SubCategory>();
            string query = "delete from SubCategory where Id = "+subCatId+"";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return catList = GetAllSubCategories(catId);

        }
        internal List<SubCategory> GetAllSubCategories(int catId)
        {
            CustomQuery cq = new CustomQuery();
            List<SubCategory> catList = new List<SubCategory>();
            string query = "select * from SubCategory where categoryId = "+catId+"";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    catList.Add(mapDataToAppSubCategory(row));
                }
                return catList;
            }
            else
            {
                return catList;
            }
        }
        internal List<SubCategory> DeleteSubCategory(int catId)
        {
            CustomQuery cq = new CustomQuery();
            List<SubCategory> catList = new List<SubCategory>();
            string query = "delete from SubCategory where Id = " + catId + "";
            cq.ExecuteNonQuery(query);
            return GetAllSubCategories(catId);
        }

        public SubCategory mapDataToAppSubCategory(DataRow row)
        {
            SubCategory info = new SubCategory();

            info.Id = int.Parse(row["Id"].ToString());
            info.categoryId = int.Parse(row["categoryId"].ToString());
            info.isCustom = int.Parse(row["isCustom"].ToString());
            info.subCategoryName = row["subCategoryName"].ToString();
            info.subCategoryType = row["subCategoryType"].ToString();
            info.iconName = row["iconName"].ToString();
            info.iconMarkerName = row["iconMarkerName"].ToString();
            info.color = row["color"].ToString();
            info.createdAt = DateTime.Parse(row["createdAt"].ToString());

            return info;

        }


        // app methods

        internal List<SubCategoryApp> GetAllSubCategoriesForApp(int catId)
        {
            CustomQuery cq = new CustomQuery();
            List<SubCategoryApp> catList = new List<SubCategoryApp>();
            string query = "select DISTINCT Subcategory.subCategoryName,Subcategory.subCategoryType,Subcategory.color,Subcategory.iconName,Subcategory.iconMarkerName,Subcategory.isCustom,Subcategory.Id from subcategory inner join userinterest on subcategory.Id =userinterest.subcategoryId inner join AppCategory on subcategory.categoryId =  AppCategory.Id where AppCategory.Id = " + catId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    catList.Add(mapDataToAppSubCategoryVersion(row));
                }
                return catList;
            }
            else
            {
                return catList;
            }
        }
        public SubCategoryApp mapDataToAppSubCategoryVersion(DataRow row)
        {
            SubCategoryApp info = new SubCategoryApp();

            info.subCategoryId = int.Parse(row["Id"].ToString());
            info.isCustom = int.Parse(row["isCustom"].ToString());
            info.interest_name = row["subCategoryName"].ToString();
            info.interest_type = row["subCategoryType"].ToString();
            info.iconName = row["iconName"].ToString();
            info.iconMarkerName = row["iconMarkerName"].ToString();
            info.color = row["color"].ToString();
            info.radius = "125";
            info.isActive = 0;

            return info;

        }



        //Profile section

        public Profiles mapDataToAppSettingProfile(DataRow row)
        {
            Profiles info = new Profiles();
            info.ProfileId = int.Parse(row["Id"].ToString());
            info.porfile_name = row["profileName"].ToString();
            info.color = row["color"].ToString();
            info.profile_interest = GetAllCategoriesForAppByProfileId(info.ProfileId);
            

            return info;

        }


        public DataTable GetVendorProfile(int vendorId)
        {
            CustomQuery cq = new CustomQuery();
            List<Profiles> settingsList = new List<Profiles>();
            string query = "select * from VendorProfile where vendorId = " + vendorId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        public List<Profiles> GetAppUserProfileSettings(int userId)
        {
            CustomQuery cq = new CustomQuery();
            List<Profiles> settingsList = new List<Profiles>();
            string query = "select * from SettingProfile where userId = " + userId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    settingsList.Add(mapDataToAppSettingProfile(row));

                }
                return settingsList;
            }
            else
            {
                return settingsList;
            }
        }
       

        public List<UserInterest> GetUserInterests(int Id)
        {
            CustomQuery cq = new CustomQuery();
            List<UserInterest> userInterest = new List<UserInterest>();
            string query = "select * from UserInterest where settingId = " + Id + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    userInterest.Add(mapDataToAppUserInterest(row));
                }
                return userInterest;
            }
            else
            {
                return userInterest;
            }
        }

        public void DeleteUserInterest(int settingId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from userinterest where settingId = "+settingId+"";
            cq.ExecuteNonQuery(query);
        }
        public void DeleteUserProfileAndInterest(int userId)
        {
            SettingProfile sp = new SettingProfile();

            CustomQuery cq = new CustomQuery();

            List<Profiles> profile = GetAppUserProfileSettings(userId);
            if (profile.Count > 0)
            {
                

                foreach(Profiles pro in profile)
                {
                    string query1 = "delete from userinterest where settingId = "+pro.ProfileId+"";
                    cq.ExecuteNonQuery(query1);
                }

                string query = "delete from SettingProfile where userId = " + userId + "";
                cq.ExecuteNonQuery(query);
                
            }
            
        }
       
        public UserInterest mapDataToAppUserInterest(DataRow row)
        {
            UserInterest info = new UserInterest();

            info.Id = int.Parse(row["Id"].ToString());
            info.categoryId = int.Parse(row["categoryId"].ToString());
            info.settingId = int.Parse(row["settingId"].ToString());
            info.subCategoryId = int.Parse(row["subCategoryId"].ToString());
            info.radius = int.Parse(row["radius"].ToString());
            info.isActive = int.Parse(row["isActive"].ToString());
            info.color = row["color"].ToString();
            

            return info;

        }

        




        //Vendor Profile Settings

        //Profile section

        public VendorProfile mapDataToAppVendorProfile(DataRow row)
        {
            VendorProfile info = new VendorProfile();

            info.Id = int.Parse(row["Id"].ToString());
            info.vendorId = int.Parse(row["vendorId"].ToString());
            info.colorId = int.Parse(row["colorId"].ToString());
            info.profileName = row["profileName"].ToString();
            info.profileName = row["profileName"].ToString();
            info.isActive = int.Parse(row["isActive"].ToString());


            return info;

        }



        public List<VendorProfile> GetAppVendorProfileSettings(int vendorId)
        {
            CustomQuery cq = new CustomQuery();
            List<VendorProfile> settingsList = new List<VendorProfile>();
            string query = "select * from VendorProfile where Vendor = " + vendorId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    settingsList.Add(mapDataToAppVendorProfile(row));

                }
                return settingsList;
            }
            else
            {
                return settingsList;
            }
        }

        public DataTable GetAllVendorProfileSettings()
        {
            CustomQuery cq = new CustomQuery();
            List<VendorProfile> settingsList = new List<VendorProfile>();
            string query = "select VendorProfile.profileName,VendorProfile.color,VendorProfile.colorId,vendorprofile.isActive,vendor.name from VendorProfile inner join vendor on VendorProfile.vendorId = Vendor.Id";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;
        }

        public DataTable GetVendorProfileSettings(string vendorId)
        {
            CustomQuery cq = new CustomQuery();
            List<VendorProfile> settingsList = new List<VendorProfile>();
            string query = "select VendorProfile.profileName,VendorProfile.color,VendorProfile.colorId,vendorprofile.isActive,vendor.name from VendorProfile inner join vendor on VendorProfile.vendorId = Vendor.Id  where VendorProfile.vendorId = '"+vendorId+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;
        }

        public DataTable GetSearchResult(UserInfo info)
        {

            //SELECT * FROM UserInfo WHERE name LIKE '%s%' and email LIKE '%bri%' and unlock_code LIKE '%a9%'

            CustomQuery cq = new CustomQuery();
            List<VendorProfile> settingsList = new List<VendorProfile>();
            string query = "SELECT * FROM UserInfo";
            bool flag = false;
            if (info.name != "" || info.email != "" || info.unlock_code != "" || info.contactNumber != "")
            {
                query += " WHERE ";
            }

            if (info.name != "")
            {
                flag = true;
                query += "name LIKE '%" + info.name + "%'";
            }
            if (info.email != "")
            {
                if (flag)
                {
                    query += " and email LIKE '%" + info.email + "%'";
                }
                else
                {
                    query += "email LIKE '%" + info.email + "%'";
                    flag = true;
                }
                
                
            }
            if (info.unlock_code != "")
            {
                if (flag)
                {
                    query += " and unlock_code LIKE '%" + info.unlock_code + "%'";
                }
                else
                {
                    query += "unlock_code LIKE '%" + info.unlock_code + "%'";
                    flag = true;
                }
                
            }

            if (info.contactNumber != "")
            {
                if (flag)
                {
                    query += " and contactNumber LIKE '%" + info.contactNumber + "%'";
                }
                else
                {
                    query += "contactNumber LIKE '%" + info.contactNumber + "%'";
                    flag = true;
                }

            }

            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;
        }



        public DataTable GetVendorSearchResult(UserInfo info)
        {

            //SELECT * FROM UserInfo WHERE name LIKE '%s%' and email LIKE '%bri%' and unlock_code LIKE '%a9%'

            CustomQuery cq = new CustomQuery();
            List<VendorProfile> settingsList = new List<VendorProfile>();
            string query = "SELECT * FROM UserInfo where unlock_code = '"+info.unlock_code+"' and name LIKE '%" + info.name + "%'";
            

            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;
        }

        internal List<VendorProfile> AddVendorProfile(VendorProfile VendorProfile)
        {
            CustomQuery cq = new CustomQuery();
            List<VendorProfile> vpList = new List<VendorProfile>();
            string query = "Insert into VendorProfile (vendorId,profileName,color,colorId,isActive) values ('" + VendorProfile.vendorId + "','" + VendorProfile.profileName + "','" + VendorProfile.color + "','0000','" + VendorProfile.isActive+ "'); ; SELECT LAST_INSERT_ID();";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                vpList = GetAppVendorProfileSettings(int.Parse(dt.Rows[0].ItemArray[0].ToString()));
            }
            return vpList;

        }

        internal int AddUserProfile(SettingProfile profile)
        {
            CustomQuery cq = new CustomQuery();
            List<VendorProfile> vpList = new List<VendorProfile>();
            string query = "Insert into SettingProfile (userId,profileName,color,colorId,isActive) values ('" + profile.userId + "','" + profile.profileName + "','" + profile.color + "','0000','" + profile.isActive + "'); ; SELECT LAST_INSERT_ID();";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                return int.Parse(dt.Rows[0].ItemArray[0].ToString());
            }
            else
            {
                return 0;
            }

        }




        #endregion




        #region contact 

        internal DataUser AddContact(string userId, string qlumiId, string contactNumber)
        {
            CustomQuery cq = new CustomQuery();
            DataUser userInfo = new DataUser();
            UserInfo user = new UserInfo();
            if (qlumiId != "" && qlumiId != null)
            {
                user = AuthenticateUserContact(qlumiId);
            }
            else
            {
                user = AuthenticateUserContactByPhoneNumber(contactNumber);
            }
            
            if (user.Id != 0)
            {
                if (GetUserByQlumiId(userId, user.qlumi_userId))
                {
                    string query = "insert into UserContact (userId, qlumi_userId, isVisible) values (" + userId + ",'" + user.qlumi_userId + "',0) ; SELECT LAST_INSERT_ID();";
                    DataTable dt = cq.ExecuteSQLQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        userInfo = GetUserByQlumiId(user.qlumi_userId);

                    }
                }
                else
                {
                    userInfo.name = "User already added in your contact list";
                }
                
            }
            else
            {
                userInfo.name = "User does not exist";
            }

            return userInfo;
            

        }
        internal void RemoveContact(string sId, string rId)
        {
            CustomQuery cq = new CustomQuery();
            DataUser userInfo = new DataUser();
            UserInfo user = new UserInfo();
            string qlumiId = GetQlumiIdByUserId(rId);
            string query = "delete from UserContact where qlumi_userId = '"+qlumiId+"' and userId = "+sId+"";
            cq.ExecuteNonQuery(query);
            


        }

        public bool GetUserByQlumiId(string userId, string qlumiId)
        {
            DataUser user = new DataUser();
            user.unlock_code = "0";
            CustomQuery cq = new CustomQuery();
            string query = "Select * from UserContact where qlumi_userId = '" + qlumiId + "' and userId = '"+userId+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetQlumiIdByUserId(string userId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            string qlumiId = "";
            string query = "select qlumi_userId from UserInfo where Id = "+userId+"";
            dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                qlumiId = dt.Rows[0].ItemArray[0].ToString();
            }
            return qlumiId;
        }
        public DataUser GetUserByQlumiId(string Id)
        {
            DataUser user = new DataUser();
            user.unlock_code = "0";
            CustomQuery cq = new CustomQuery();
            string query = "Select * from UserInfo where qlumi_userId = '"+Id+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = mapDataToUser(dt.Rows[0]);
            }
            return user;
        }
        public DataTable GetUserByQlumiIdDt(string userId, string qlumi_userId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            string query = "select Distinct  UserInfo.Id as userId, UserInfo.name,UserInfo.email,UserInfo.contactNumber,UserContact.isVisible as iAmVisibleToContact,UserInfo.qlumi_userId,UserInfo.sessionId, UserInfo.connectionId from UserInfo inner join UserContact on UserInfo.qlumi_userId = UserContact.qlumi_userId where UserContact.userId = " + userId + "  and UserContact.qlumi_userId = '" + qlumi_userId + "'";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }

        public DataTable GetUserVisibilityStatus(string userId, string qlumi_Id)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            string query = "select isVisible from UserContact where userId = " + userId + "  and qlumi_userId = '" + qlumi_Id + "'";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        internal UserInfo AuthenticateUserContact(string userId)
        {
            UserInfo user = new UserInfo();
            user.unlock_code = "1";
            CustomQuery cq = new CustomQuery();

            string query = "Select * from UserInfo where qlumi_userId='" +userId+ "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = GetUserByIdWithNoCode(int.Parse(dt.Rows[0].ItemArray[0].ToString()));
            }
            return user;
        }
        internal UserInfo AuthenticateUserContactByPhoneNumber(string contact)
        {
            UserInfo user = new UserInfo();
            user.unlock_code = "1";
            CustomQuery cq = new CustomQuery();

            string query = "Select * from UserInfo where contactNumber='" + contact + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = GetUserByIdWithNoCode(int.Parse(dt.Rows[0].ItemArray[0].ToString()));
            }
            return user;
        }
        internal UserInfo AuthenticateUserByEmail(string email)
        {
            UserInfo user = new UserInfo();
            user.unlock_code = "1";
            CustomQuery cq = new CustomQuery();

            string query = "Select * from UserInfo where email='" + email + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                user = GetUserByIdWithNoCode(int.Parse(dt.Rows[0].ItemArray[0].ToString()));
            }
            return user;
        }


        internal int SetVisibleStatus(string userId, string qlumi_UserId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable user = new DataTable();
            user = GetUserVisibilityStatus(userId, qlumi_UserId);
            if (user.Rows.Count > 0)
            {
                int visible = int.Parse(user.Rows[0].ItemArray[0].ToString());
                string query = "";
                if (visible == 0)
                {
                    query = "update UserContact set isVisible = 1 where qlumi_userId = '" + qlumi_UserId + "' and userId = '"+userId+"'";
                    cq.ExecuteNonQuery(query);
                    return 1;
                }
                else
                {
                    query = "update UserContact set isVisible = 0 where qlumi_userId = '" + qlumi_UserId + "' and userId = '" + userId + "'";
                    cq.ExecuteNonQuery(query);
                    return 0;
                }


            }
            else
            {
                return 2;
            }

            
        }

        internal DataTable GetUserContacts(string userId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            string query = "select Distinct  UserInfo.Id as userId, UserInfo.name,UserInfo.email,UserInfo.contactNumber,UserContact.isVisible as iAmVisibleToContact,UserInfo.qlumi_userId,UserInfo.sessionId, UserInfo.connectionId, messageCount.unreadCount from UserInfo inner join UserContact on UserInfo.qlumi_userId = UserContact.qlumi_userId  left join messageCount on UserInfo.Id = messageCount.userId where UserContact.userId = " + userId + "";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }

        internal DataTable GetUserContactsWithMessage(string userId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            string query = "select Distinct  UserInfo.Id as userId, UserInfo.name,UserInfo.email,UserInfo.contactNumber,UserContact.isVisible as iAmVisibleToContact,UserInfo.qlumi_userId,UserInfo.sessionId, UserInfo.connectionId, chathistory.message as last_message from UserInfo inner join UserContact on UserInfo.qlumi_userId = UserContact.qlumi_userId inner join chathistory on chathistory.sId = UserInfo.Id where UserContact.userId = " + userId + " and chathistory.sId = "+userId+"";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        

        internal DataTable GetUserDetail(string userId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            string query = "select UserInfo.Id as userId, UserInfo.name,UserInfo.email,UserInfo.contactNumber,UserInfo.qlumi_userId, UserInfo.sessionId, UserInfo.connectionId, UserInfo.password from UserInfo where Id = '" + userId + "'";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }

        internal DataTable GetUserVisibleContacts(string userId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            string query = "select Distinct  UserInfo.Id as userId, UserInfo.name,UserInfo.email,UserInfo.contactNumber,UserContact.isVisible as iAmVisibleToContact,UserInfo.qlumi_userId, userLocation.lati as latitude, userLocation.longi as longitude, UserInfo.sessionId, UserInfo.connectionId,userLocation.createdAt as UpdatedAt, userLocation.updatedAt as lastUpdatedAt from UserInfo inner join UserContact on UserInfo.Id = UserContact.userId inner join userLocation on userLocation.userId = UserInfo.Id where UserContact.qlumi_userId = '" + userId + "' and UserContact.isVisible = 1";
            dt = cq.ExecuteSQLQuery(query);
            return dt;
        }
        #endregion 

        #region Application stats
            internal DataTable GetApplicationStats()
            {
                CustomQuery cq = new CustomQuery();
                DataTable stats = new DataTable();
                int TotalIPhoneUsers = 0;
                int TotalAndroidUsers = 0;
                int TotalQlumiUsers = 0;
                int TotalVendors = 0;
                double AverageIPhonePerVendor = 0;
                double AverageAndroidPerVendor = 0;
                double AverageTotalPerVendor = 0;

                stats.Columns.Add("TotalIPhoneUsers", typeof(int));
                stats.Columns.Add("TotalAndroidUsers", typeof(int));
                stats.Columns.Add("TotalQlumiUsers", typeof(int));
                stats.Columns.Add("TotalQlumiVendors", typeof(int));
                stats.Columns.Add("AverageIPhonePerVendor", typeof(double));
                stats.Columns.Add("AverageAndroidPerVendor", typeof(double));
                stats.Columns.Add("AverageTotalPerVendor",typeof(double));
                
                string tip = "select * from UserInfo where requestOrigin = 'iOS'";
                DataTable tipDt = cq.ExecuteSQLQuery(tip);
                TotalIPhoneUsers = tipDt.Rows.Count;


                string tau = "select * from UserInfo where requestOrigin = 'android'";
                DataTable tauDt = cq.ExecuteSQLQuery(tau);
                TotalAndroidUsers = tauDt.Rows.Count;



                string tu = "select * from UserInfo";
                DataTable tuDt = cq.ExecuteSQLQuery(tu);
                TotalQlumiUsers = tuDt.Rows.Count;


                string tv = "select * from Vendor";
                DataTable tvDt = cq.ExecuteSQLQuery(tv);
                TotalVendors = tvDt.Rows.Count;

                AverageIPhonePerVendor = TotalIPhoneUsers / TotalVendors;
                AverageAndroidPerVendor = TotalAndroidUsers / TotalVendors;
                AverageTotalPerVendor = TotalQlumiUsers / TotalVendors;
                stats.Rows.Add(TotalIPhoneUsers, TotalAndroidUsers, TotalQlumiUsers, TotalVendors,AverageIPhonePerVendor, AverageAndroidPerVendor, AverageTotalPerVendor);

                return stats;
            }
            internal DataTable GetVendorRanking()
            {
                CustomQuery cq = new CustomQuery();
                DataTable stats = new DataTable();


                stats.Columns.Add("vendorId", typeof(int));
                stats.Columns.Add("vendorName", typeof(string));
                stats.Columns.Add("vendorCode", typeof(string));
                stats.Columns.Add("count", typeof(int));

                string query = "select * from Vendor";
                string countQuery = "";
                DataTable dt = cq.ExecuteSQLQuery(query);
                if(dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        countQuery = "select * from UserInfo where unlock_code = '"+row[6].ToString()+"'";
                        DataTable dt2 = cq.ExecuteSQLQuery(countQuery);
                        stats.Rows.Add(row[0], row[1], row[6], dt2.Rows.Count);
                    }
                }

                DataView dv = stats.DefaultView;
                dv.Sort = "count DESC";

                DataTable dt3 = dv.ToTable();

                return dt3;
            }
        #endregion 
    }



    //Reponse classes
    #region Classes
    public class DataUser : UserInfo
    {
        public ICollection<Profiles> Profiles { get; set; }
        public string BrandLogo { get; set; }
        public int iAmVisibleToContact { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string codeValidUntil { get; set; }

    }
    public class Profiles
    {
        public Profiles()
        {

        }
        public int ProfileId { get; set; }
        public string porfile_name { get; set; }
        public string color { get; set; }
        public Nullable<int> isActive { get; set; }
        public ICollection<ProfileCategory> profile_interest { get; set; }
    }

    public class ProfileCategory
    {
        public ProfileCategory()
        {

        }

        public string categoryName { get; set; }
        public string categoryType { get; set; }
        public string iconName { get; set; }
        public string iconMarkerName { get; set; }
        public string color { get; set; }
        public int categoryId { get; set; }
        public ICollection<SubCategoryApp> interests { get; set; }
    }
    public class SubCategoryApp
    {
        public SubCategoryApp()
        {

        }
        public string interest_name { get; set; }
        public string interest_type { get; set; }
        public string radius { get; set; }
        public int isActive { get; set; }
        public string color { get; set; }
        public string iconName { get; set; }
        public string iconMarkerName { get; set; }
        public Nullable<int> isCustom { get; set; }
        public int subCategoryId { get; set; }

    }


 
    #endregion
    
}