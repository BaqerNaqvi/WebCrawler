
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataAccess;


namespace WebCrawler.DataCore.Managers
{
    public class JobManager
    {
 

        public void GenerateBill()
        {
            var cq = new CustomQuery();
            var cq2 = new CustomQuery();
            var adminManager = new AdminManager();
            var vendors = new List<Vendor>();
            string query = "select * from vendor where role=1";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var obj = adminManager.mapDataToVendor(row);
                    vendors.Add(obj);
                    query = "Select * from adinfo where vendorid=" + obj.Id;
                    DataTable adddt;
                    adddt = cq.ExecuteSQLQuery(query);
                    if (adddt.Rows.Count > 0)
                    {
                        foreach (DataRow d1 in adddt.Rows)
                        {
                            obj.AdInfoes.Add(adminManager.mapAdInfo(d1));
                        }
                    }
                }
            }
            foreach (var ven in vendors)
            {
                if (ven.Id != 44)
                {
                    continue;
                }
                var vendorSum = 0;
                int days = 0;
                if (ven.AdInfoes != null && ven.AdInfoes.Any())
                {
                    var innerqt = new CustomQuery();
                    DataTable dasdsad;

                    foreach (var adinfo in ven.AdInfoes)
                    {
                        query =
                       "select sum(cost) from adprogress where vendorid=" + ven.Id + " and adid=" + adinfo.Id + " and createdat >= CURDATE() AND createdat < CURDATE() + INTERVAL 1 DAY";
                        dasdsad = innerqt.ExecuteSQLQuery(query);
                        if (dasdsad.Rows.Count > 0)
                        {
                            var localsum = dasdsad.Rows[0].ItemArray[0];
                            vendorSum = vendorSum + Convert.ToInt32(localsum);
                        }
                    }
                }

                query = "select * from billinginfo where vendorid=" + ven.Id;
                DataTable adddt;
                adddt = cq.ExecuteSQLQuery(query);
                if (adddt.Rows.Count > 0)
                {
                    var duePyment = int.Parse(adddt.Rows[0]["duepayment"].ToString());
                    var duePaymentDate = DateTime.Parse(adddt.Rows[0]["nextpaymentdate"].ToString());


                    query = "update billinginfo set duepayment=" + (duePyment + vendorSum) + " where vendorid=" + ven.Id;
                    DataTable dt31 = cq2.ExecuteSQLQuery(query);
                    if (dt31.Rows.Count > 0)
                    {

                    }
                    days = Convert.ToInt32((duePaymentDate - DateTime.Now).TotalDays);

                    if (days <= 0)
                    {
                        // show button 
                        foreach (var adinfo in ven.AdInfoes)
                        {
                            query = "update adinfo set isvisible=" + 3 + " where id=" + adinfo.Id;
                            DataTable asas = cq2.ExecuteSQLQuery(query);
                        }
                    }
                    else if (days <= 5)
                    {
                        // show button 
                        foreach (var adinfo in ven.AdInfoes)
                        {
                            query = "update adinfo set isvisible=" + 2 + " where id=" + adinfo.Id;
                            DataTable asas = cq2.ExecuteSQLQuery(query);
                        }
                    }
                    //query="u"
                }

            }

        }

        public void ManagePayment(int? vendorId)
        {
            var cq = new CustomQuery();
            var cq2 = new CustomQuery();
            var adminManager = new AdminManager();
            var vendors = new List<Vendor>();
            string query = "select * from vendor where role=1";
            DataTable dt = cq.ExecuteSQLQuery(query);
            //if (dt.Rows.Count > 0)
        }
    }
}
