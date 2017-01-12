using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace WebCrawler.DataCore.Managers
{
    public class AdvertiserManager
    {
        internal Vendor GetAdvertiserProfile(int adverterId)
        {
            CustomQuery cq = new CustomQuery();
            string query = "Select * from vendor where id="+ adverterId;
            var dt = cq.ExecuteSQLQuery(query);
            return dt.Rows.Count > 0 ? MapAdvertiser(dt.Rows[0]) : null;
        }

        public Vendor MapAdvertiser(DataRow row)
        {
            var cr = new Vendor
            {
                 Id   = int.Parse(row["Id"].ToString()),
                 name = row["name"].ToString(),
                 email = row["email"].ToString(),
                 contactNumber = row["contactNumber"].ToString(),
                 description = row["description"].ToString(),
                 website = row["website"].ToString(),

            };
            return cr;
        }

        internal Vendor UpdateAdvertiserProfile(Vendor obj)
        {
            var previousObj= GetAdvertiserProfile(obj.Id);
            if (previousObj != null)
            {
                CustomQuery cq = new CustomQuery();
                string query = "Update vendor set name='"+ obj.name+"',email='"+ obj.email+ "',contactNumber='"+ obj.contactNumber+ "',description='"+ obj.description+"',website='"+ obj.website+"' where id="+previousObj.Id;
                var dt = cq.ExecuteSQLQuery(query);
                return dt.Rows.Count > 0 ? MapAdvertiser(dt.Rows[0]) : null;
            }
            return null;
        }
    }
}
