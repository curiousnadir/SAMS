﻿using System.ComponentModel.DataAnnotations;

namespace TMD.Web.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; }

        [Display(Name = "Short Description")]
        [Required(ErrorMessage = "Short Description is required.")]
        public string ShortDescription { get; set; }

        [Display(Name = "Detail Description")]
        [Required(ErrorMessage = "Detail Description is required.")]
        public string DetailDescription { get; set; }
        
        public int ProductCategoryID { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    }
}