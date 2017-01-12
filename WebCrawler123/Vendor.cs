//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebCrawler
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vendor
    {
        public Vendor()
        {
            this.AdInfoes = new HashSet<AdInfo>();
            this.VendorProfiles = new HashSet<VendorProfile>();
            this.AdProgresses = new HashSet<AdProgress>();
            this.BillingInfoes = new HashSet<BillingInfo>();
            this.BillingHistories = new HashSet<BillingHistory>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string contactNumber { get; set; }
        public string logoImage { get; set; }
        public string unlock_code { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
        public string website { get; set; }
        public Nullable<int> tokenLimit { get; set; }
        public Nullable<int> currentTokenCount { get; set; }
        public Nullable<int> userId { get; set; }
        public string unlockCodeAge { get; set; }
        public int role { get; set; }
    
        public virtual ICollection<AdInfo> AdInfoes { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<VendorProfile> VendorProfiles { get; set; }
        public virtual ICollection<AdProgress> AdProgresses { get; set; }
        public virtual ICollection<BillingInfo> BillingInfoes { get; set; }
        public virtual ICollection<BillingHistory> BillingHistories { get; set; }
    }
}
