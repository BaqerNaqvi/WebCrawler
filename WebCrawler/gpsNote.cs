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
    
    public partial class gpsNote
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string lati { get; set; }
        public string longi { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public Nullable<int> sender_userId { get; set; }
        public string sender_name { get; set; }
        public Nullable<int> receiver_userId { get; set; }
        public string receiver_name { get; set; }
    }
}
