﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TMD.Interfaces.IServices;
using TMD.Models.DomainModels;
using TMD.Web.ViewModels.Common;
using TMD.Web.ViewModels.Quote;
using TMD.Web.ModelMappers;
using TMD.Models.RequestModels;
using TMD.WebBase.Mvc;

namespace TMD.Web.Controllers
{
    [Authorize]
    public class QuoteController : BaseController
    {
        private readonly IQuoteService quoteService;
        private readonly IEmployeeService employeeService;

        public QuoteController(IQuoteService quoteService, IEmployeeService employeeService)
        {
            this.quoteService = quoteService;
            this.employeeService = employeeService;
        }

        // GET: /Quote/
        [SiteAuthorize(PermissionKey = "QuotesList")]
        public ActionResult Index()
        {
            ViewBag.MessageVM = TempData["message"] as MessageViewModel;
            var vm = new QuoteViewModel();
            vm.Employees = employeeService.GetAllEmployees().Select(x => x.MapEmployeeDdlFromServerToClient()).ToList();
            return View(vm);
        }

        [HttpPost]
        public JsonResult Index(QuoteSearchRequest searchRequest)
        {
            string[] userPermissionsSet = (string[])Session["UserPermissionSet"];
            searchRequest.HasPermissionToViewAll = userPermissionsSet.Contains("ViewAllInquiries");
            searchRequest.CurrentUserId = User.Identity.GetUserId();

            var contactResponse = quoteService.GetAllQuotes(searchRequest);
            var quoteList = contactResponse.Quotes.ToList().Select(x => x.MapServerToClientSearch()).ToList();
            var model = new QuoteViewModel()
            {
                data = quoteList,
                recordsFiltered = contactResponse.FilteredCount,
                recordsTotal = contactResponse.TotalCount
            };

            return Json(model, JsonRequestBehavior.AllowGet);
            //return null;
        }

        // GET: /Quote/Details/5
        [SiteAuthorize(PermissionKey = "QuotesList")]
        public ActionResult Print(int id)
        {
            var quote = quoteService.GetQuoteAndQuoteDetail(id);
            QuotePrintViewModel viewModel=new QuotePrintViewModel();
            if (quote != null)
            {
                viewModel.Quote = quote.MapServerToClient();
                viewModel.Quote.QuoteExclusions = quote.QuoteExclusions.Select(x => x.MapServerToClient()).ToList();
                viewModel.Contact = quote.Contact.MapForQuotePrint();
                viewModel.Quote.QuoteDetail = quote.QuoteDetails.FirstOrDefault().MapServerToClient();
                viewModel.ProductModel = quote.QuoteDetails.FirstOrDefault().ProductModel.MapServerToClient();
                viewModel.Product = quote.QuoteDetails.FirstOrDefault().ProductModel.Product.MapServerToClient();
                viewModel.ProductModelTechnicalSpec = quote.QuoteDetails.FirstOrDefault().ProductModel.ProductTechSpecs.Select(x => x.MapServerToClient()).ToList();
            }
            return View(viewModel);
        }

        // GET: /Quote/Create
        [SiteAuthorize(PermissionKey = "CreateUpdateQuote")]
        public ActionResult Create(int? id, int? inq, int? con)
        {
            QuoteViewModel viewModel = new QuoteViewModel();

            var quoteResponse = quoteService.GetQuoteResponse(id);
            int QuoteCount = quoteResponse.QuoteCount;
            if (quoteResponse.Quote != null)
            {
                viewModel.Quote = quoteResponse.Quote.MapServerToClient();
                if (quoteResponse.Quote.QuoteDetails.FirstOrDefault() != null)
                    viewModel.Quote.QuoteDetail = quoteResponse.Quote.QuoteDetails.FirstOrDefault().MapServerToClient();
                if (quoteResponse.Quote.QuoteExclusions.FirstOrDefault() != null)
                    viewModel.QuoteExclusions = quoteResponse.Quote.QuoteExclusions.Select(x=>x.MapServerToClient()).ToList();
            }
            else
            {
                viewModel.Quote = new Models.QuoteModel
                {
                    CreatedDate = DateTime.UtcNow,
                    Subject = "Quotation for supply of ",
                    PaymentTerms = "70% on Product Delivery \n30% After installation",
                    DeliveryTerms = "1 - 2 Week(s)",
                    InstallationTerms = "1 Week(s)",
                    FreeServiceTerms = "6 Month(s)",
                    WarrantyTerms = "1 Year(s)",
                    ValidityTerms = "1 Week(s)"
                };
                string ModelNo = Convert.ToString(QuoteCount).PadLeft(3, '0');
                viewModel.Quote.QuoteReferenceNo = "ZAM-"+ ModelNo;
                viewModel.Quote.QuoteDetail.Make = "ZAMTAS";
                if (inq != null)
                {
                    viewModel.Quote.InquiryId = inq;
                    viewModel.Quote.ContactId = con;
                }
            }
            viewModel.Contacts = quoteResponse.Contacts.Select(x => x.CreateDDL()).ToList();
            viewModel.Products = quoteResponse.Products.Select(x => x.CreateDDL()).ToList();
            viewModel.ProductModels = quoteResponse.ProductModels.Select(x => x.CreateDDL()).ToList();
            viewModel.Exclusions = quoteResponse.Exclusions.Select(x => x.MapServerToClient()).ToList();

            ViewBag.MessageVM = TempData["message"] as MessageViewModel;
            return View(viewModel);
        }

        // POST: /Quote/Create
        [HttpPost]
        [SiteAuthorize(PermissionKey = "CreateUpdateQuote")]
        public ActionResult Create(QuoteViewModel viewModel)
        {
            try
            {
                viewModel.Quote.UpdatedDate = DateTime.UtcNow;
                viewModel.Quote.UpdatedBy = User.Identity.GetUserId();
                if (viewModel.Quote.QuoteID == 0)
                {
                    //viewModel.Quote.CreatedDate = DateTime.UtcNow;
                    viewModel.Quote.CreatedBy = User.Identity.GetUserId();
                }


                Quote quote = new Quote();


                quote = viewModel.Quote.MapClientToServer();

                quote.QuoteDetails = new List<QuoteDetail>
                {
                    viewModel.Quote.QuoteDetail.MapClientToServer()
                };
                if(viewModel.QuoteExclusions.Any())
                    quote.QuoteExclusions= viewModel.QuoteExclusions.Select(x => x.MapClientToServer()).ToList();

                if (quoteService.SaveQuote(quote))
                {
                    TempData["message"] = new MessageViewModel
                    {
                        IsSaved = true,
                        Message = "Your data has been saved successfully!"
                    };
                }
                else
                {
                    TempData["message"] = new MessageViewModel
                    {
                        IsError = true,
                        Message = "There is some problem, please try again!"
                    };
                }
                return RedirectToAction("Create");
            }
            catch (Exception e)
            {
                ViewBag.MessageVM = new MessageViewModel
                {
                    IsError = true,
                    Message = e.Message+"\n"+e.InnerException
                };
                return View(viewModel);
            }
        }
    }
}
