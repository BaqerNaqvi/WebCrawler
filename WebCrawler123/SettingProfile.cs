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
    
    public partial class SettingProfile
    {
        public SettingProfile()
        {
            this.UserInterests = new HashSet<UserInterest>();
        }
    
        public int Id { get; set; }
        public int userId { get; set; }
        public string profileName { get; set; }
        public string color { get; set; }
        public Nullable<int> colorId { get; set; }
        public Nullable<int> isActive { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<UserInterest> UserInterests { get; set; }
    }
}
