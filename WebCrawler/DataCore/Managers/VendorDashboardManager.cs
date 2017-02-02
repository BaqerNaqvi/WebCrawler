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

namespace WebCrawler.DataCore.Managers
{
    public class VendorDashboardManager
    {
        public int CreateVendorAd(AdInfo info)
        {
            CustomQuery cq = new CustomQuery();
            DataTable dt = new DataTable();
            int adId = 0;
            info.isVisible = 1;
            info.sponsorFacts = info.sponsorFacts.Replace("'", "");
            string query = "insert into AdInfo (vendorId,adTypeId,bidTypeId,categoryId," +
                           " interestId,isCustom,isVisible,customInterest,dailyBudget,couponUrl," +
                           "mapVideo,mapImage,sponsorFacts,sponsorWebsite,sponsorPhone,sponorLogo,adTitle, " +
                           "lati, longi, locationName, costPerAction, costPerConversion, createdAt)" +
                           " values ('" + info.vendorId + "','" + info.adTypeId + "','" + info.bidTypeId + "','" +
                           info.categoryId + "','" + info.interestId + "','" + info.isCustom + "','" +
                           info.isVisible + "','" + info.customInterest + "','" + info.dailyBudget + "','" + 
                           info.couponUrl + "','default.mp4','default.png','" + info.sponsorFacts + "','" +
                           info.sponsorWebsite + "','" + info.sponsorPhone + "','defaultLogo.png','" + 
                           info.adTitle + "', '" + info.lati + "', '" + info.longi + "', '" +
                           info.locationName + "', '" + info.costPerAction + "', '" + 
                           info.costPerConversion + "' ,current_timestamp); SELECT LAST_INSERT_ID();";
            
            dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                adId = int.Parse(dt.Rows[0].ItemArray[0].ToString());
            }

            return adId;
        }

        public List<AdInfo> GetAdByVendorId(string vendorId)
        {
            CustomQuery cq = new CustomQuery();
            List<AdInfo> adList = new List<AdInfo>();
            string query = "select * from AdInfo where vendorId = '"+vendorId+"'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if(dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    adList.Add(MapDataToAdInfoObj(row));
                }
            }

            return adList;
            
        }


        public AdInfo GetAdById(string adId)
        {
            CustomQuery cq = new CustomQuery();
            AdInfo ad = new AdInfo();
            string query = "select * from AdInfo where Id = '" + adId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                ad = MapDataToAdInfoObj(dt.Rows[0]);
            }

            return ad;

        }



        public List<AdInfo> GetAllVendorAds(string vendorId)
        {
            CustomQuery cq = new CustomQuery();
            List<AdInfo> adList = new List<AdInfo>();
            string query = "select * from AdInfo where vendorId = '" + vendorId + "'";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    adList.Add(MapDataToAdInfoObj(row));
                }
            }

            return adList;

        }


        public List<AdInfo> GetAllAds()
        {
            CustomQuery cq = new CustomQuery();
            List<AdInfo> adList = new List<AdInfo>();
            string query = "select * from AdInfo";
            DataTable dt = cq.ExecuteSQLQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    adList.Add(MapDataToAdInfoObj(row));
                }
            }

            return adList;

        }



        public List<AdInfo> RemoveAd(string Id, string vendorId)
        {
            CustomQuery cq = new CustomQuery();
            AdInfo ad = new AdInfo();
            string query = "delete from adinfo where Id = " + Id + "";
            cq.ExecuteNonQuery(query);
            return GetAllVendorAds(vendorId);
        }

        public List<AdInfo> UpdateAdVisibility(string status, string vendorId, string adId)
        {
            CustomQuery cq = new CustomQuery();
            AdInfo ad = new AdInfo();
            string query = "update AdInfo set isVisible = '" + status + "' where Id = " + adId + "";
            cq.ExecuteNonQuery(query);
            return GetAllVendorAds(vendorId);
        }

        public AdInfo UpdateAd(AdInfo info)
        {
            CustomQuery cq = new CustomQuery();
            AdInfo ad = new AdInfo();
            string query = "update AdInfo set vendorId = '" + info.vendorId + "', adTypeId = '" + info.adTypeId + "', bidTypeId = '" + info.bidTypeId + "', categoryId = '" + info.categoryId + "', interestId = '" + info.interestId + "', isCustom = '" + info.isCustom + "', isVisible = '" + info.isVisible + "', customInterest = '" + info.customInterest + "', dailyBudget = '" + info.dailyBudget + "', couponUrl = '" + info.couponUrl + "', mapVideo = '" + info.mapVideo + "', mapImage = '" + info.mapImage + "', sponsorFacts = '" + info.sponsorFacts + "', sponsorWebsite = '" + info.sponsorWebsite + "', sponsorPhone = '" + info.sponsorPhone + "', sponorLogo = '" + info.sponorLogo + "', adTitle = '" + info.adTitle + "', lati = '" + info.lati + "', longi = '" + info.longi + "', locationName = '" + info.locationName + "', costPerAction = '" + info.costPerAction + "', costPerConversion = '" + info.costPerConversion + "' createdAt = current_timestamp where Id = " + info.Id + "";
            cq.ExecuteNonQuery(query);
            return GetAdById(info.Id.ToString());
        }

        public AdInfo UpdateAdImage(string image, string adId)
        {
            CustomQuery cq = new CustomQuery();
            AdInfo ad = new AdInfo();
            string query = "update AdInfo set mapImage = '" +image+ "' where Id = " +adId + "";
            cq.ExecuteNonQuery(query);
            return GetAdById(adId);
        }

        public AdInfo UpdateAdVideo(string videoUrl, string adId)
        {
            CustomQuery cq = new CustomQuery();
            AdInfo ad = new AdInfo();
            string query = "update AdInfo set mapVideo = '" + videoUrl + "' where Id = " + adId + "";
            cq.ExecuteNonQuery(query);
            return GetAdById(adId);
        }



        public BidType GetBidTypeById(int bidId)
        {
            CustomQuery cq = new CustomQuery();
            BidType ad = new BidType();
            string query = "select * from BidType where Id = " + bidId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);

            if(dt.Rows.Count > 0)
            {
                ad = MapDataToBidTypeObj(dt.Rows[0]);
            }
            return ad;
        }


        public AdType GetAdTypeById(int bidId)
        {
            CustomQuery cq = new CustomQuery();
            AdType ad = new AdType();
            string query = "select * from AdType where Id = " + bidId + "";
            DataTable dt = cq.ExecuteSQLQuery(query);

            if (dt.Rows.Count > 0)
            {
                ad = MapDataToAdTypeObj(dt.Rows[0]);
            }
            return ad;
        }




        #region interestANDBid
        public DataTable GetAllAdTypes()
        {
            CustomQuery cq = new CustomQuery();
            List<AdInfo> adList = new List<AdInfo>();
            string query = "select * from AdType";
            DataTable dt = cq.ExecuteSQLQuery(query);
            
            return dt;

        }
        public DataTable GetAllBidTypes()
        {
            CustomQuery cq = new CustomQuery();
            List<AdInfo> adList = new List<AdInfo>();
            string query = "select * from bidType";
            DataTable dt = cq.ExecuteSQLQuery(query);

            return dt;

        }
        #endregion






        public AdInfo MapDataToAdInfoObj(DataRow row)
        {
            AdInfo info = new AdInfo();

            info.Id = int.Parse(row["Id"].ToString());
            info.vendorId = int.Parse(row["vendorId"].ToString());
            info.adTypeId = int.Parse(row["adTypeId"].ToString());
            info.bidTypeId = int.Parse(row["bidTypeId"].ToString());
            info.interestId = int.Parse(row["interestId"].ToString());
            info.categoryId = int.Parse(row["categoryId"].ToString());
            info.isCustom = int.Parse(row["isCustom"].ToString());
            info.isVisible = int.Parse(row["isVisible"].ToString());
            info.customInterest = row["customInterest"].ToString();

            info.dailyBudget = row["dailyBudget"].ToString();
            info.couponUrl = row["sponsorWebsite"].ToString();
            info.mapVideo = row["mapVideo"].ToString();
            info.mapImage = row["mapImage"].ToString();
            info.sponsorFacts = row["sponsorFacts"].ToString();
            info.sponsorWebsite = row["couponUrl"].ToString();
            info.sponsorPhone = row["sponsorPhone"].ToString();
            info.sponorLogo = row["sponorLogo"].ToString();


            info.adTitle = row["adTitle"].ToString();
            info.longi = row["lati"].ToString();
            info.lati = row["longi"].ToString();
            info.locationName = row["locationName"].ToString();
            info.costPerAction = row["costPerAction"].ToString();
            info.costPerConversion = row["costPerConversion"].ToString();

            info.createdAt = DateTime.Parse(row["createdAt"].ToString());
            info.AdType = GetAdTypeById(int.Parse(row["adTypeId"].ToString()));
            info.BidType = GetBidTypeById(int.Parse(row["bidTypeId"].ToString()));


            return info;
        }

        public BidType MapDataToBidTypeObj(DataRow row)
        {
            BidType info = new BidType();

            info.Id = int.Parse(row["Id"].ToString());
            info.title = row["title"].ToString();
            info.bidCost = row["bidCost"].ToString();
            
            return info;
        }


        public AdType MapDataToAdTypeObj(DataRow row)
        {
            AdType info = new AdType();

            info.Id = int.Parse(row["Id"].ToString());
            info.title = row["title"].ToString();

            return info;
        }
    }
}