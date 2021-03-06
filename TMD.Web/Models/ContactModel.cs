﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TMD.Web.Models
{
    public class ContactModel
    {
        public int    ContactID { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        public string FullName { get; set; }

        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Display(Name = "Primary Phone")]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Cell No.")]
        [Required(ErrorMessage = "Cell No. is required.")]
        public string CellNo { get; set; }

        [Display(Name = "Contact Type")]
        public Nullable<int> ContactType { get; set; }

        public Nullable<int> AddressId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdateDate { get; set; }

       



    }
}