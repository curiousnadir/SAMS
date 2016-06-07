﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMD.Common;
using TMD.Interfaces.IRepository;
using TMD.Interfaces.IServices;
using TMD.Models.DomainModels;
using TMD.Models.RequestModels;
using TMD.Models.ResponseModels;
using TMD.Repository.Repositories;

namespace TMD.Implementation.Services
{
    public class InquiryService : IInquiryService
    {
        private readonly IInquiryRepository inquiryRepository;
        private readonly IDocumentService documentService;
        private readonly IContactRepository contactRepository;
        private readonly IProductRepository productRepository;
        private readonly IInquiryDetailRepository inquiryDetailRepository;


        public InquiryService(IInquiryRepository inquiryRepository,IDocumentService documentService, IContactRepository contactRepository, IProductRepository productRepository, IInquiryDetailRepository inquiryDetailRepository)
        {
            this.inquiryRepository = inquiryRepository;
            this.documentService = documentService;
            this.contactRepository = contactRepository;
            this.productRepository = productRepository;
            this.inquiryDetailRepository = inquiryDetailRepository;

        }
        public int AddInquiry(Models.DomainModels.Inquiry inquiry)
        {

            inquiryRepository.Add(inquiry);
            inquiryRepository.SaveChanges();

            return inquiry.InquiryID; // If Exception occurs this line will not be executed

        }

        public int UpdateInquiry(Models.DomainModels.Inquiry inquiry)
        {
            inquiryRepository.Update(inquiry);
            inquiryRepository.SaveChanges();

            return inquiry.InquiryID;
        }

        public IEnumerable<Models.DomainModels.Inquiry> GetAllInquiries()
        {
            return inquiryRepository.GetAll().ToList();

        }

        public Inquiry GeInquiryById(int id)
        {
            return inquiryRepository.Find(id);
        }

        public InquiryResponse GetInquiryResponse(int? id)
        {
            InquiryResponse inquiryResponse = new InquiryResponse();
            if (id != null)
            {
                inquiryResponse.Inquiry = inquiryRepository.Find((int)id);
                inquiryResponse.InquiryDetails = inquiryDetailRepository.GetInquiryDailByByInquiryId((int)id).ToList();
                inquiryResponse.InquiryDocuments = documentService.GetAllDocumentByRefId((int) id).ToList();
            }
            inquiryResponse.Contacts = contactRepository.GetAll().ToList();
            inquiryResponse.Products = productRepository.GetAll().ToList();
            

            return inquiryResponse;
        }

        public Inquiry GetInquiryAndInquiryDetail(int InquiryId)
        {
            return inquiryRepository.GetInquiryAndInquiryDetail(InquiryId);
        }

        public bool SaveInquiry(InquiryResponse inquiryResp)
        {
            if (inquiryResp.Inquiry.InquiryID > 0)
            {
                inquiryRepository.Update(inquiryResp.Inquiry);
            }
            else
            {
                inquiryRepository.Add(inquiryResp.Inquiry);
            }
            inquiryRepository.SaveChanges();

            SaveInquiryandDetails(inquiryResp);
            documentService.AddDocuments(inquiryResp.InquiryDocuments, inquiryResp.Inquiry.InquiryID,
                DocumentType.Inquiry);
            return true;
        }

        private void SaveInquiryandDetails(InquiryResponse inquiryResp)
        {
            if (inquiryResp.Inquiry.InquiryID > 0)
            {
                var inquiryDetails = inquiryDetailRepository.GetInquiryDailByByInquiryId(inquiryResp.Inquiry.InquiryID).ToList();

                foreach (var inquirydetail in inquiryDetails)
                {
                    inquiryDetailRepository.Delete(inquirydetail);
                }
            }
            if (inquiryResp.InquiryDetails != null && inquiryResp.InquiryDetails.Any())
            {
                foreach (var inquiryDetail in inquiryResp.InquiryDetails)
                {
                    inquiryDetail.InquiryID = inquiryResp.Inquiry.InquiryID;

                    inquiryDetailRepository.Add(inquiryDetail);
                }
            }
            inquiryDetailRepository.SaveChanges();
        }
        public InquiryResponse GetAllInquiries(InquirySearchRequest searchRequest)
        {
            var inquiries = inquiryRepository.GetAllInquiries(searchRequest);
            return inquiries;
        }
    }
}
