//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TMD.Models.DomainModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inquiry
    {
        public int InquiryID { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public int ContactID { get; set; }
        public System.DateTime InquiryDate { get; set; }
        public int Priority { get; set; }
        public string ContactResponse { get; set; }
        public string UserComments { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdateDate { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        public virtual AspNetUser AspNetUser2 { get; set; }
        public virtual Contact Contact { get; set; }
        
        public virtual ICollection<InquiryDetail> InquiryDetails { get; set; }
    }
}
