using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataAccess;
using Npgsql;
using WebCrawler.Models;

namespace WebCrawler.DataCore.Managers
{
    public class AddManager
    {
        #region Config
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;" + "Password=P@ssw0rd;Database=MyDb;");

        #endregion
        #region Subs
        public List<AdInfo> GetAdss(GetAddsForUser requestModel)
        {
            var addIds = new List<string>();
            var foundAdds = new List<AdInfo>();
            conn.Open();
            var query = "SELECT adid FROM adlocation WHERE" +
                            " ST_DWithin(location::geography,  ST_GeomFromText('POINT("+requestModel.location_lng+" "
                            +requestModel.location_lat+")', 4326)::geography,50000);";
            var cmd = new NpgsqlCommand(query, conn);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var addId = dr["adid"];
                addIds.Add(addId.ToString());
            }
            conn.Close();

            if (addIds.Count > 0)
            {
                var customParm = "(";
                for (int i=0; i<addIds.Count;i++)
                {
                    customParm = customParm + addIds[i];
                    if (i + 1 < addIds.Count)
                    {

                        customParm = customParm + ",";
                    }
                }
                customParm = customParm + ")";

                var adminManager = new AdminManager();
                query = "select * from adinfo where id in "+ customParm;
                CustomQuery cq = new CustomQuery();
                DataTable dt = cq.ExecuteSQLQuery(query);
                if (dt.Rows.Count > 0)
                {
                    foundAdds.AddRange(from DataRow row in dt.Rows select adminManager.mapAdInfo(row));
                }
            }
            return foundAdds;
        }

        /// <summary>
        /// Insert Add-info to table 
        /// </summary>
        public void InsertAdInfo(int adid, string lat, string lng, string palce)
        {
            conn.Open();
            string query = "INSERT INTO adlocation(adid,location,locname) VALUES(" + adid + ", ST_GeomFromText('SRID=4326;POINT(" + lng + "  " + lat + ")'), 'No Place Added'); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            var dt = cmd.ExecuteScalar();
            conn.Close();
        }
        #endregion
    }
}
