﻿using TMD.Models.DomainModels;

namespace TMD.Web.ModelMappers
{
    public static class ProductMappers
    {
        public static Product MapClientToServer(this Models.Product source)
        {
            return new Product
            {
                ProductID = source.ProductID,
                ProductName = source.ProductName,
                ShortDescription = source.ShortDescription,
                DetailDescription = source.DetailDescription ?? "",
                ProductCategoryID = source.ProductCategoryID,
                CreatedBy = source.CreatedBy,
                CreatedDate = source.CreatedDate,
                UpdatedBy = source.UpdatedBy,
                UpdatedDate = source.UpdatedDate  

            };
        }
        public static Product MapClientToServerWithDocuments(this Models.Product source)
        {
            return new Product
            {
                ProductID = source.ProductID,
                ProductName = source.ProductName,
                ShortDescription = source.ShortDescription,
                DetailDescription = source.DetailDescription ?? "",
                ProductCategoryID = source.ProductCategoryID,
                CreatedBy = source.CreatedBy,
                CreatedDate = source.CreatedDate,
                UpdatedBy = source.UpdatedBy,
                UpdatedDate = source.UpdatedDate,
                Documents = source.Documents

            };
        }
        public static Models.Product MapServerToClient(this Product source)
        {
            return new Models.Product
            {

                ProductID = source.ProductID,
                ProductName = source.ProductName,
                ShortDescription = source.ShortDescription,
                DetailDescription = source.DetailDescription,
                ProductCategoryID = source.ProductCategoryID,
                CreatedBy = source.CreatedBy,
                CreatedDate = source.CreatedDate,
                UpdatedBy = source.UpdatedBy,
                UpdatedDate = source.UpdatedDate       

            };
        }
        public static Models.Product CreateDDL(this Product source)
        {
            return new Models.Product
            {

                ProductID = source.ProductID,
                ProductName = source.ProductName
            };
        }
    }
}