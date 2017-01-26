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

        public AddManager ()
        {
            
        }
        #region Subs
        public List<AdInfoContainer> GetAdss(GetAddsAPiRequestModel requestModel, string baseUrl)
        {
            var addIds = new List<string>();
            DataTable dt = new DataTable();
            var foundAdds = new List<AdInfoContainer>();
            var query = "SELECT adid FROM adlocation WHERE" +
                            " ST_DWithin(location::geography,  ST_GeomFromText('POINT("+requestModel.location_lng+" "
                            +requestModel.location_lat+")', 4326)::geography,50000);";
            conn.Open();
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
                query = "select * from adinfo where id in "+ customParm+" and isvisible=1";
                CustomQuery cq = new CustomQuery();
                 dt = cq.ExecuteSQLQuery(query);
                if (dt.Rows.Count > 0)
                {
                    foundAdds.AddRange(from DataRow row in dt.Rows select adminManager.mapAdInfoForApi(row,baseUrl));
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
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                var dt = cmd.ExecuteScalarAsync();
            }
        }
        #endregion

        
    }
}
