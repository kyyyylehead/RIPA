//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RIPASTOP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Submissions
    {
        //public Submissions()
        //{
            //this.statusMsgs = new HashSet<StatusMessage_JSON_vw>();
        //}
        public int ID { get; set; }
        public System.DateTime StartDate { get; set; }
        public string Status { get; set; }
        public string LogFile { get; set; }
        public Nullable<int> TotalProcessed { get; set; }
        public Nullable<int> TotalSuccess { get; set; }
        public Nullable<int> TotalRejected { get; set; }
        public Nullable<int> TotalWithErrors { get; set; }
        public Nullable<int> TotalHTTPErrors { get; set; }
        public Nullable<System.DateTime> DateSubmitted { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        //public virtual ICollection<StatusMessage_JSON_vw> statusMsgs { get; set; } //access StatusMessage_JSON_vw entity 
        //public virtual ICollection<Submissions> subList { get; set; }
    }
}
