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
    
    public partial class ChatHistory
    {
        public int Id { get; set; }
        public Nullable<int> sId { get; set; }
        public Nullable<int> rId { get; set; }
        public string sName { get; set; }
        public string rName { get; set; }
        public Nullable<int> messageType { get; set; }
        public string message { get; set; }
        public string messageDate { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
        public virtual UserInfo UserInfo1 { get; set; }
    }
}
