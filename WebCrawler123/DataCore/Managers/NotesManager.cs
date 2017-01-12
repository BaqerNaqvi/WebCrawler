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

namespace WebCrawler.DataCore.Managers
{
    public class NotesManager
    {
        
        #region Meeting Notes

        internal DataTable GetMeetingNotes(string meetingId, int userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from meetingNotes where Id = "+meetingId+" and receiver_userId = '"+userId+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }
        internal DataTable GetMeetingNotesById(string meetingId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from meetingNotes where Id = " + meetingId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }
        internal DataTable GetAllMeetingNotes(string meetingId, int userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from meetingNotes where sender_userId = '" + userId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }

        internal DataTable GetAllMeetingNotesAsReceiver(string meetingId, int userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from meetingNotes where receiver_userId = '" + userId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }

        internal DataTable DeleteMeetingNote(string meetingId, int userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from meetingNotes where Id = '" + meetingId + "'";
            cq.ExecuteNonQuery(query);
            DataTable dt = GetAllMeetingNotes(meetingId, userId);
            return dt;

        }


        internal DataTable AddMeetingNote(meetingNote cr)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);
            long meetingId = 0;
            DataTable dt = new DataTable();
            //int medId = -1;
            try
            {
                connection.Open();
                string sql = "INSERT INTO meetingNotes (purpose,lati,longi,address,meetingDate,meetingTime,allowReminder,sender_userId,sender_name,receiver_userId,receiver_name) VALUES (@purpose,@lati,@longi,@address,@meetingDate,@meetingTime,@allowReminder,@sender_userId,@sender_name,@receiver_userId,@receiver_name); SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@purpose", cr.purpose);
                    cmd.Parameters.AddWithValue("@lati", cr.lati);
                    cmd.Parameters.AddWithValue("@longi", cr.longi);
                    cmd.Parameters.AddWithValue("@address", cr.address);
                    cmd.Parameters.AddWithValue("@meetingDate", cr.meetingDate);
                    cmd.Parameters.AddWithValue("@meetingTime", cr.meetingTime);
                    cmd.Parameters.AddWithValue("@allowReminder", cr.allowReminder);
                    cmd.Parameters.AddWithValue("@sender_userId", cr.sender_userId);
                    cmd.Parameters.AddWithValue("@sender_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@receiver_userId", cr.receiver_userId);
                    cmd.Parameters.AddWithValue("@receiver_name", cr.sender_name);


                    int mId = cmd.ExecuteNonQuery();
                    meetingId = cmd.LastInsertedId;
                    return GetMeetingNotes(meetingId.ToString(), int.Parse(cr.receiver_userId.ToString()));
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }
        }


        internal DataTable UpdateMeetingNote(meetingNote cr)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);
            //int medId = -1;
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                string sql = "update meetingNotes set purpose = @purpose, lati = @lati, longi = @longi, address = @address, meetingDate = @meetingDate, meetingTime = @meetingTime, allowReminder = @allowReminder, sender_userId = @sender_userId, sender_name = @sender_name, receiver_userId = @receiver_userId, receiver_name = @receiver_name where Id = @Id";
            
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@purpose", cr.purpose);
                    cmd.Parameters.AddWithValue("@lati", cr.lati);
                    cmd.Parameters.AddWithValue("@longi", cr.longi);
                    cmd.Parameters.AddWithValue("@address", cr.address);
                    cmd.Parameters.AddWithValue("@meetingDate", cr.meetingDate);
                    cmd.Parameters.AddWithValue("@meetingTime", cr.meetingTime);
                    cmd.Parameters.AddWithValue("@allowReminder", cr.allowReminder);
                    cmd.Parameters.AddWithValue("@sender_userId", cr.sender_userId);
                    cmd.Parameters.AddWithValue("@sender_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@receiver_userId", cr.receiver_userId);
                    cmd.Parameters.AddWithValue("@receiver_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@Id", cr.Id);

                    cmd.ExecuteNonQuery();
                    return GetMeetingNotesById(cr.Id.ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }
        }
        #endregion


        #region gps Notes

        internal DataTable GetGpsNotes(string noteId, string userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from gpsNotes where Id = " + noteId + " and receiver_userId = '"+userId+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }

        internal DataTable GetGpsNotesByUserId(string userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from gpsNotes where receiver_userId = '" + userId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }


        internal DataTable GetUserGpsNotesByUserId(string userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from UserGpsNotes where sender_userId = '" + userId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }

        internal DataTable GetUserBookmarkedGpsNotesByUserId(string userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from UserGpsNotes where sender_userId = '" + userId + "' and isBookmark = 1";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }


        internal DataTable GetUserGpsNotesById(string Id)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from UserGpsNotes where Id = '" + Id + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }
        internal DataTable DeleteUserGpsNotesByUserId(string userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from UserGpsNotes where sender_userId = '" + userId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return GetUserGpsNotesByUserId(userId);

        }

        internal DataTable DeleteUserGpsNotesByNoteId(string noteId, string userId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from UserGpsNotes where Id = '" + noteId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return GetUserGpsNotesByUserId(userId);

        }

        internal DataTable AddGpsNote(gpsNote cr)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);
            long meetingId = 0;
            DataTable dt = new DataTable();
            //int medId = -1;
            try
            {
                connection.Open();
                string sql = "INSERT INTO gpsNotes (title,lati,longi,address,description,color,sender_userId, sender_name,receiver_userId,receiver_name) VALUES (@title,@lati,@longi,@address,@description,@color,@sender_userId,@sender_name,@receiver_userId,@receiver_name) ; SELECT LAST_INSERT_ID();";
            
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", cr.title);
                    cmd.Parameters.AddWithValue("@lati", cr.lati);
                    cmd.Parameters.AddWithValue("@longi", cr.longi);
                    cmd.Parameters.AddWithValue("@address", cr.address);
                    cmd.Parameters.AddWithValue("@description", cr.description);
                    cmd.Parameters.AddWithValue("@color", cr.color);
                    cmd.Parameters.AddWithValue("@sender_userId", cr.sender_userId);
                    cmd.Parameters.AddWithValue("@sender_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@receiver_userId", cr.receiver_userId);
                    cmd.Parameters.AddWithValue("@receiver_name", cr.receiver_name);

                    int mId = cmd.ExecuteNonQuery();
                    meetingId = cmd.LastInsertedId;
                    return GetGpsNotes(meetingId.ToString(), cr.receiver_userId.ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }
        }

        internal DataTable UpdateUserNote(UserGpsNote cr)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);
            //int medId = -1;
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                string sql = "update UserGpsNotes set title = @title, lati = @lati, longi = @longi, color = @color, address = @address, description = @description, sender_userId = @sender_userId, sender_name = @sender_name where Id = @Id";


                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", cr.title);
                    cmd.Parameters.AddWithValue("@lati", cr.lati);
                    cmd.Parameters.AddWithValue("@longi", cr.longi);
                    cmd.Parameters.AddWithValue("@color", cr.color);
                    cmd.Parameters.AddWithValue("@address", cr.address);
                    cmd.Parameters.AddWithValue("@sender_userId", cr.sender_userId);
                    cmd.Parameters.AddWithValue("@sender_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@description", cr.description);
                    cmd.Parameters.AddWithValue("@Id", cr.Id);

                    cmd.ExecuteNonQuery();
                    return GetUserGpsNotesById(cr.Id.ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }
        }


        internal DataTable BookmarkedGpsNotesByUserId(string userId, string noteId, string isBookmark)
        {

            CustomQuery cq = new CustomQuery();
            string query = "update UserGpsNotes set isBookmark ='" + isBookmark + "' where Id='" + noteId + "' and sender_userId = '" + userId + "' ";
            cq.ExecuteNonQuery(query);

            return GetUserGpsNotesByUserId(userId);

        }
        internal DataTable AddUserGpsNote(UserGpsNote cr)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);
            long meetingId = 0;
            DataTable dt = new DataTable();
            //int medId = -1;
            try
            {
                connection.Open();
                string sql = "INSERT INTO UserGpsNotes (title,lati,longi,address,description,color,sender_userId, sender_name,isBookmark) VALUES (@title,@lati,@longi,@address,@description,@color,@sender_userId,@sender_name,@isBookmark) ; SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", cr.title);
                    cmd.Parameters.AddWithValue("@lati", cr.lati);
                    cmd.Parameters.AddWithValue("@longi", cr.longi);
                    cmd.Parameters.AddWithValue("@address", cr.address);
                    cmd.Parameters.AddWithValue("@description", cr.description);
                    cmd.Parameters.AddWithValue("@color", cr.color);
                    cmd.Parameters.AddWithValue("@sender_userId", cr.sender_userId);
                    cmd.Parameters.AddWithValue("@sender_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@receiver_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@isBookmark", "0");

                    int mId = cmd.ExecuteNonQuery();
                    meetingId = cmd.LastInsertedId;
                    return GetUserGpsNotesByUserId(cr.sender_userId.ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }
        }


        internal DataTable AddVendorGpsNote(VendorNote cr)
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(entityConnectionString);
            long meetingId = 0;
            DataTable dt = new DataTable();
            //int medId = -1;
            try
            {
                connection.Open();
                string sql = "INSERT INTO VendorNotes (title,lati,longi,address,description,sender_userId, sender_name,deliveryStatus,createdAt,updatedAt) VALUES (@title,@lati,@longi,@address,@description,@sender_userId,@sender_name,@deliveryStatus,@createdAt,@updatedAt) ; SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", cr.title);
                    cmd.Parameters.AddWithValue("@lati", cr.lati);
                    cmd.Parameters.AddWithValue("@longi", cr.longi);
                    cmd.Parameters.AddWithValue("@address", cr.address);
                    cmd.Parameters.AddWithValue("@description", cr.description);
                    cmd.Parameters.AddWithValue("@sender_userId", cr.sender_userId);
                    cmd.Parameters.AddWithValue("@sender_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@receiver_name", cr.sender_name);
                    cmd.Parameters.AddWithValue("@deliveryStatus", cr.deliveryStatus);
                    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                    int mId = cmd.ExecuteNonQuery();
                    meetingId = cmd.LastInsertedId;
                    return GetVendorGpsNotesById(cr.sender_userId.ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return dt;
            }


        }

        internal DataTable GetVendorGpsNotesByCoord(string vendorId, string lat, string lng)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from VendorNotes where lati = '" + lat + "' and longi = '"+lng+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }

        internal DataTable GetVendorGpsNotesById(string vendorId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "select * from VendorNotes where sender_userId = '" + vendorId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            return dt;

        }

        internal DataTable SendAllVendorNotes(string vendorId, string unlockCode)
        {
            CustomQuery cq = new CustomQuery();
            DataTable userNotes = new DataTable();
            string query = "select * from VendorNotes where sender_userId = '" + vendorId + "' and deliveryStatus = 0";
            DataTable vendorNotes = cq.ExecuteSQLQuery(query);

            string query2 = "select * from UserInfo where unlock_code = '" + unlockCode + "'";
            DataTable users = cq.ExecuteSQLQuery(query2);

            if (vendorNotes.Rows.Count > 0)
            {
                foreach (DataRow row in vendorNotes.Rows)
                {
                    //notes.title = row[0]
                    if(users.Rows.Count > 0)
                    {
                        foreach (DataRow userRow in users.Rows)
                        {

                            UserGpsNote notes = new UserGpsNote();
                            notes.title = row[1].ToString();
                            notes.lati = row[4].ToString();
                            notes.longi = row[5].ToString();
                            notes.address = row[3].ToString();
                            notes.color = "#09A7FF";
                            notes.description = row[2].ToString();
                            notes.sender_name = row[7].ToString();
                            notes.sender_userId = int.Parse(userRow[0].ToString());
                            //notes.receiver_name = userRow[1].ToString();
                            //notes.receiver_userId = int.Parse(userRow[0].ToString());



                            AddUserGpsNote(notes);
                        }


                        string update = "update VendorNotes set deliveryStatus = 1 where sender_userId = " +vendorId + "";
                        cq.ExecuteNonQuery(update);


                    }
                    

                    
                }
                
            }



            return vendorNotes;

        }


        internal DataTable SendAllVendorNotesToNewUser(string vendorId, string unlockCode, string userId)
        {
            CustomQuery cq = new CustomQuery();
            DataTable userNotes = new DataTable();
            string query = "select * from VendorNotes where sender_userId = '" + vendorId + "'";
            DataTable vendorNotes = cq.ExecuteSQLQuery(query);

            string query2 = "select * from UserInfo where Id = '" + userId + "'";
            DataTable users = cq.ExecuteSQLQuery(query2);

            if (vendorNotes.Rows.Count > 0)
            {
                foreach (DataRow row in vendorNotes.Rows)
                {
                    //notes.title = row[0]
                    if (users.Rows.Count > 0)
                    {
                        UserGpsNote notes = new UserGpsNote();
                        notes.title = row[1].ToString();
                        notes.lati =  row[4].ToString();
                        notes.longi = row[5].ToString();
                        notes.address = row[3].ToString();
                        notes.color = "#09A7FF";
                        notes.description = row[2].ToString();
                        notes.sender_name = row[7].ToString();
                        notes.sender_userId = int.Parse(userId);
                        notes.isBookmark = 0;
                        //notes.receiver_name = userRow[1].ToString();
                        //notes.receiver_userId = int.Parse(userRow[0].ToString());



                        AddUserGpsNote(notes);


                        string update = "update VendorNotes set deliveryStatus = 1 where sender_userId = " + vendorId + "";
                        cq.ExecuteNonQuery(update);


                    }



                }

            }



            return vendorNotes;

        }

        internal DataTable DeleteVendorGpsNotesByCoord(string Id, string lat, string lng)
        {
            CustomQuery cq = new CustomQuery();
            string query = "delete from VendorNotes where Id = '" + Id + "' and lati = '"+lat+"' and longi = '"+lng+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);

            return GetVendorGpsNotesById(Id);

        }
        #endregion


        #region location
        internal DataTable GetUserLocation(string userId)
        {
            CustomQuery cq = new CustomQuery();
            string locationId = "0";
            string query = "select * from userLocation where userId = '"+userId+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                locationId = dt.Rows[0].ItemArray[0].ToString();
            }

            return dt;
        }

        internal DataTable UpdateUserLocation(string userId, string lati, string longi)
        {
            CustomQuery cq = new CustomQuery();
            DataTable currentDt = GetUserLocation(userId);
            DataTable updatedDt = new DataTable();
            if (currentDt.Rows.Count > 0)
            {
                string update = "update userLocation set lati = '" + lati + "', longi = '" + longi + "', createdAt = current_timestamp, updatedAt = STR_TO_DATE('" + currentDt.Rows[0].ItemArray[4] + "','%m/%d/%Y %h:%i:%s %p') where userId = '" + userId + "'";
                cq.ExecuteNonQuery(update);
            }
            else
            {
                string insert = "insert into userLocation (userId, lati, longi, createdAt) values ('" + userId + "','" + lati + "', '" + longi + "', current_timestamp) ; SELECT LAST_INSERT_ID();";
                updatedDt = cq.ExecuteSQLQuery(insert);
                

            }

            updatedDt = GetUserLocation(userId);
            return updatedDt;
        }
        #endregion
    }
}