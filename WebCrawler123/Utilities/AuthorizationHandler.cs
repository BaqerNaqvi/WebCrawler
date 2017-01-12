using System;
using System.Collections.Generic;
using System.Text;
//using DataAccess;
using System.Data;
using System.Data.SqlClient;

namespace Utilities
{
    public class AuthorizationHandler
    {
        //private CustomQuery _CustomQuery = null;
        private string _ConnectionString = String.Empty;

        /// <summary>
        /// This method returns the rights on screens for a user in a location
        /// </summary>
        /// <param name="pUserId"></param> 
        /// <returns></returns>
        public void GetScreenRights(String pUserId)
        {
            // Logic for giving the Screen  Rights to Users Goes here.
        }
        
        /// <summary>
        /// Method to load rights for specific Control.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable checkControlSecurity(string pScreenPrefix, string pLocationId, string pUserId)
        {
            //Modify this function to reflect your DB.
            DataTable a = new DataTable();
            return a;
        }

        /// <summary>
        /// this method returns information of the logged in User.
        /// </summary>
        /// <param name="pUserId"></param>
        /// <returns></returns>
        public DataTable GetUserData(String pUserId)
        {
            //Modify this function to reflect your DB.
            DataTable a = new DataTable();
            return a;
        }
    }
}
