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
    
    public partial class UserInterest
    {
        public int Id { get; set; }
        public Nullable<int> settingId { get; set; }
        public Nullable<int> categoryId { get; set; }
        public Nullable<int> subCategoryId { get; set; }
        public Nullable<int> radius { get; set; }
        public string color { get; set; }
        public Nullable<int> isActive { get; set; }
    
        public virtual AppCategory AppCategory { get; set; }
        public virtual SettingProfile SettingProfile { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }
}