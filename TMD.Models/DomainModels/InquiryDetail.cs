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
    
    public partial class InquiryDetail
    {
        public int InquiryDetailID { get; set; }
        public int InquiryID { get; set; }
        public int ProductID { get; set; }

        public virtual Inquiry Inquiry { get; set; }
        public virtual Product Product { get; set; }
    }
}
