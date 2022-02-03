using eInvoicing.DTO;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using eInvoicing.Service.Helper.Extension;
using System.Security.Claims;
using eInvoicing.Web.Filters;
using System.Net;
using System.Net.Http;
using System.Diagnostics;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IUserSession _userSession;
        private readonly IHttpClientHandler _httpClient;
        public DocumentController(IHttpClientHandler httpClient, IUserSession userSession)
        {
            _httpClient = httpClient;
            _userSession = userSession;
        }
        //[AllowAnonymous]
        [HttpGet]
        [ActionName("pending")]
        public ActionResult GetPendingDocuments()
        {
            return View();
        }
        [HttpPost]
        [ActionName("ajax_pending")]
        public ActionResult AjaxPendingDocuments()
        {
            try
            {
                var pageNumber = Request["pagination[page]"];
                var pageSize = Request["pagination[perpage]"];
                string fromDate = Request["fromDate"];
                string toDate = Request["toDate"];
                var sortDirection = Request["sort[sort]"];
                var sortColumnName = Request["sort[field]"];
                var searchValue = Request["query[generalSearch]"];
                var status = Request["query[status]"];
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var Today = DateTime.Now;
                DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture); //Convert.ToDateTime(fromDate);
                DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture); //Convert.ToDateTime(toDate);

                string url = "api/document/pending?pageNumber=" + Convert.ToInt32(pageNumber) + "&pageSize=" + Convert.ToInt32(pageSize) + "&fromdate=" + _fromDate + "&todate=" + _toDate + 
                    "&searchValue=" + searchValue + "&sortColumnName=" + sortColumnName + "&sortDirection=" + sortDirection + "&status=" + status;
                var response = _httpClient.GET(url);
                return Json(JsonConvert.DeserializeObject<DocumentResponse>(response.Info), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Calling Preparation error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("UploadFile")]
        public ActionResult ImportFromExcel()
        {
            try
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                    file.SaveAs(fname);
                    var response = _httpClient.GET("api/document/ImportFromExcel?fullPath=" + fname);
                    if (response.Info != null)
                    {
                        var ImportedSheet = JsonConvert.DeserializeObject<ImportedSheetDTO>(response.Info);
                        return Json(new
                        {
                            InsertedDocumentsCount = ImportedSheet.InsertedDocumentsCount,
                            InsertedInvoiceLinesCount = ImportedSheet.InsertedInvoiceLinesCount,
                            InsertedTaxableItemsCount = ImportedSheet.InsertedTaxableItemsCount,
                            UpdatedDocumentsCount = ImportedSheet.UpdatedDocumentsCount,
                            UpdatedInvoiceLinesCount = ImportedSheet.UpdatedInvoiceLinesCount,
                            UpdatedTaxableItemsCount = ImportedSheet.UpdatedTaxableItemsCount,
                            IsInserted = ImportedSheet.IsInserted,
                            IsUpdated = ImportedSheet.IsUpdated,
                            NonExistingDocumentIds= ImportedSheet.NonExistingDocumentIds,
                            UpdatesStatus = ImportedSheet.UpdatesStatus,
                            success = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Something went wrong Due to the following " + response.Message + " !!!" }, JsonRequestBehavior.AllowGet);
                }
                // Returns message that successfully uploaded  
                return Json(new { success = false, message = "File is empty!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Invalid Template Due to the following " + ex + " !!!"}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [ActionName("details")]
        public ActionResult GetDocumentById(string Id)
        {
            try
            {
                var document = JsonConvert.DeserializeObject<DocumentVM>(_httpClient.GET("api/document/GetByDocumentId?id=" + Id).Info);
                var docType = new List<Catalog>()
                            {
                             new Catalog(){ Value = "i", Text = "Invoice" },
                             new Catalog(){ Value = "c", Text = "Credit" },
                             new Catalog(){ Value = "d", Text = "Debit" }
                            };
                document.documentType = docType.FirstOrDefault(i => i.Value == document.documentType.ToLower())?.Text;
                ViewBag.document = document;
                return View(document);
            }
            catch (Exception ex)
            {
                // display error page
                return View(ex.Message.ToString());
            }
        }
        [ActionName("items")]
        public ActionResult GetItemsLineByDocId(string id)
        {
            try
            {
                var response = _httpClient.GET("api/document/GetByDocumentId?id=" + id);
                return Json(JsonConvert.DeserializeObject<DocumentVM>(response.Info).invoiceLines, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Calling Preparation error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }
        [ActionName("submitted_items")]
        public ActionResult GetItemsLineByuuid(string uuid)
        {
            try
            {
                var response = _httpClient.GET("api/document/raw?uuid=" + uuid);
                return Json(JsonConvert.DeserializeObject<GetDocumentResponse>(response.Info).invoiceLines, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Calling Preparation error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }
        //[AllowAnonymous]
        [HttpGet]
        [ActionName("submitted")]
        public ActionResult GetSubmittedDocuments()
        {
            return View();
        }
        [HttpGet]
        [ActionName("received")]
        public ActionResult GetReceivedDocuments()
        {
            return View();
        }
        [HttpPost]
        [ActionName("ajax_submitted")]
        public ActionResult AjaxSubmittedDocuments()
        {
            try
            {
                var pageNumber = Request["pagination[page]"];
                var pageSize = Request["pagination[perpage]"];
                string fromDate = Request["fromDate"];
                string toDate = Request["toDate"];
                var sortDirection = Request["sort[sort]"];
                var sortColumnName = Request["sort[field]"];
                var searchValue = Request["query[generalSearch]"];
                var status = Request["query[status]"];
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var Today = DateTime.Now;
                DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture); //Convert.ToDateTime(fromDate);
                DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);  //Convert.ToDateTime(toDate);

                string url = "api/document/submitted?pageNumber=" + Convert.ToInt32(pageNumber) + "&pageSize=" + Convert.ToInt32(pageSize) +"&fromdate=" + _fromDate + "&todate=" + _toDate +  
                    "&searchValue=" + searchValue + "&sortColumnName=" + sortColumnName + "&sortDirection=" + sortDirection + "&status=" + status;
                var response = _httpClient.GET(url);
                return Json(JsonConvert.DeserializeObject<DocumentResponse>(response.Info), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Calling Preparation error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("ajax_received")]
        public ActionResult AjaxReceivedDocuments()
        {
            try
            {
                var pageNumber = Request["pagination[page]"];
                var pageSize = Request["pagination[perpage]"];
                string fromDate = Request["fromDate"];
                string toDate = Request["toDate"];
                var sortDirection = Request["sort[sort]"];
                var sortColumnName = Request["sort[field]"];
                var searchValue = Request["query[generalSearch]"];
                var status = Request["query[status]"];
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var Today = DateTime.Now;
                DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture); //Convert.ToDateTime(fromDate);
                DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);  //Convert.ToDateTime(toDate);

                string url = "api/document/received?pageNumber=" + Convert.ToInt32(pageNumber) + "&pageSize=" + Convert.ToInt32(pageSize) + "&fromdate=" + _fromDate + "&todate=" + _toDate +
                    "&searchValue=" + searchValue + "&sortColumnName=" + sortColumnName + "&sortDirection=" + sortDirection + "&status=" + status;
                var response = _httpClient.GET(url);
                return Json(JsonConvert.DeserializeObject<DocumentResponse>(response.Info), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Calling Preparation error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [ActionName("raw")]
        public ActionResult GetDocumentbyuuid(string uuid)
        {
            try
            {
                GetDocumentResponse result = new GetDocumentResponse();
                var obj = _httpClient.GET("api/document/raw?uuid=" + uuid)?.Info;
                if (obj != null)
                {
                    result = JsonConvert.DeserializeObject<GetDocumentResponse>(obj);
                    if (result != null && result.StatusCode == HttpStatusCode.OK)
                    {
                        result.dateTimeIssued = result.dateTimeIssued; //DateTime.Parse(result.dateTimeIssued).ToString("dd-MMM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                        result.dateTimeRecevied = result.dateTimeRecevied; //DateTime.Parse(result.dateTimeRecevied).ToString("dd-MMM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                        var status = new List<Catalog>() {
                            new Catalog(){Value = "valid",    Text = "bg-success" },
                            new Catalog(){Value = "invalid",  Text = "bg-danger" },
                            new Catalog(){Value = "submitted",Text = "bg-primary" },
                            new Catalog(){Value = "cancelled",Text = "bg-dark" },
                            new Catalog(){Value = "rejected", Text = "bg-warning" } };
                        var docType = new List<Catalog>()
                            {
                             new Catalog(){ Value = "i", Text = "Invoice" },
                             new Catalog(){ Value = "c", Text = "Credit" },
                             new Catalog(){ Value = "d", Text = "Debit" }
                            };
                        var Types = new List<Catalog>()
                            {
                             new Catalog(){ Value = "0", Text = "B" },
                             new Catalog(){ Value = "1", Text = "P" },
                             new Catalog(){ Value = "2", Text = "F" }
                            };
                        result.documentType = docType.FirstOrDefault(i => i.Value == result.documentType?.ToLower())?.Text;
                        result.statusClass = status.FirstOrDefault(i => i.Value == result.status?.ToLower())?.Text;
                        result.issuer.type = Types.FirstOrDefault(i => i.Value == result.issuer.type?.ToLower())?.Text;
                        result.receiver.type = Types.FirstOrDefault(i => i.Value == result.receiver.type?.ToLower())?.Text;
                        ViewBag.IsExist = true;
                    }
                    else
                        ViewBag.IsExist = false;
                }
                else
                {
                    ViewBag.IsExist = false;
                }
                return View("raw", (object)result);
            }
            catch (Exception ex)
            {
                // display error page
                return View(ex.Message.ToString());
            }
        }

        [HttpGet]
        [ActionName("raw_received")]
        public ActionResult raw_received(string uuid)
        {
            try
            {
                GetDocumentResponse result = new GetDocumentResponse();
                var obj = _httpClient.GET("api/document/raw?uuid=" + uuid)?.Info;
                if (obj != null)
                {
                    result = JsonConvert.DeserializeObject<GetDocumentResponse>(obj);
                    if (result != null && result.StatusCode == HttpStatusCode.OK)
                    {
                        result.dateTimeIssued = result.dateTimeIssued; //DateTime.Parse(result.dateTimeIssued).ToString("dd-MMM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                        result.dateTimeRecevied = result.dateTimeRecevied; //DateTime.Parse(result.dateTimeRecevied).ToString("dd-MMM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                        var status = new List<Catalog>() {
                            new Catalog(){Value = "valid",    Text = "bg-success" },
                            new Catalog(){Value = "invalid",  Text = "bg-danger" },
                            new Catalog(){Value = "submitted",Text = "bg-primary" },
                            new Catalog(){Value = "cancelled",Text = "bg-dark" },
                            new Catalog(){Value = "rejected", Text = "bg-warning" } };
                        var docType = new List<Catalog>()
                            {
                             new Catalog(){ Value = "i", Text = "Invoice" },
                             new Catalog(){ Value = "c", Text = "Credit" },
                             new Catalog(){ Value = "d", Text = "Debit" }
                            };
                        var Types = new List<Catalog>()
                            {
                             new Catalog(){ Value = "0", Text = "B" },
                             new Catalog(){ Value = "1", Text = "P" },
                             new Catalog(){ Value = "2", Text = "F" }
                            };
                        result.documentType = docType.FirstOrDefault(i => i.Value == result.documentType?.ToLower())?.Text;
                        result.statusClass = status.FirstOrDefault(i => i.Value == result.status?.ToLower())?.Text;
                        result.issuer.type = Types.FirstOrDefault(i => i.Value == result.issuer.type?.ToLower())?.Text;
                        result.receiver.type = Types.FirstOrDefault(i => i.Value == result.receiver.type?.ToLower())?.Text;
                        ViewBag.IsExist = true;
                    }
                    else
                        ViewBag.IsExist = false;
                }
                else
                {
                    ViewBag.IsExist = false;
                }
                return View("raw_received", (object)result);
            }
            catch (Exception ex)
            {
                // display error page
                return View(ex.Message.ToString());
            }
        }

        [HttpGet]
        [ActionName("new_document")]
        public ActionResult CreateNewDocument()
        {
            try
            {
                var BusinessGroupId = ClaimsPrincipal.Current.FindFirst("BusinessGroupId").Value;
                var response = JsonConvert.DeserializeObject<NewDocumentVM>(_httpClient.GET("api/taxpayer/details?BusinessGroupId="+BusinessGroupId).Info);
                if (response != null)
                {
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "Quantities.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.Quans = JsonConvert.DeserializeObject<List<QuantityDTO>>(json);
                    }
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "Currencies.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.Currencies = JsonConvert.DeserializeObject<Dictionary<string, CurrencyDTO>>(json).GroupBy(item => item.Key)
                          .Select(group => new CurrencyDTO()
                          {
                              code = group.First().Value.code,
                              name = group.First().Value.name,
                              decimal_digits = group.First().Value.decimal_digits,
                              name_plural = group.First().Value.name_plural,
                              rounding = group.First().Value.rounding,
                              symbol_native = group.First().Value.symbol_native,
                          }).ToList();
                    }
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "TaxTypes.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.TaxTypes = JsonConvert.DeserializeObject<List<TaxTypeDTO>>(json);
                    }
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "SubTypes.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.SubTypes = JsonConvert.DeserializeObject<List<SubTypeDTO>>(json);
                    }
                    return View(response);
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
                //return View();
            }


        }

        [HttpGet]
        [ActionName("edit_document")]
        public ActionResult edit_document(string InternalId)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<NewDocumentVM>(_httpClient.GET("api/document/getdocumentbyinternalid?InternalId=" + InternalId).Info);
                if (response != null)
                {
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "Quantities.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.Quans = JsonConvert.DeserializeObject<List<QuantityDTO>>(json);
                    }
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "Currencies.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.Currencies = JsonConvert.DeserializeObject<Dictionary<string, CurrencyDTO>>(json).GroupBy(item => item.Key)
                          .Select(group => new CurrencyDTO()
                          {
                              code = group.First().Value.code,
                              name = group.First().Value.name,
                              decimal_digits = group.First().Value.decimal_digits,
                              name_plural = group.First().Value.name_plural,
                              rounding = group.First().Value.rounding,
                              symbol_native = group.First().Value.symbol_native,
                          }).ToList();
                    }
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "TaxTypes.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.TaxTypes = JsonConvert.DeserializeObject<List<TaxTypeDTO>>(json);
                    }
                    using (StreamReader r = new StreamReader(Path.Combine(Server.MapPath("~/Content/Uploads/"), "SubTypes.json")))
                    {
                        string json = r.ReadToEnd();
                        ViewBag.SubTypes = JsonConvert.DeserializeObject<List<SubTypeDTO>>(json);
                    }
                    return View(response);
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
                //return View();
            }
        }

        [HttpPost]
        [ActionName("ajax_new_document")]
        public ActionResult ajaxtaxpayerdetails(doc obj)
        {
            try
            {
                foreach (var item in obj.InvoiceLines)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.SalesTotal = item.Quantity * item.AmountEGP;
                    item.NetTotal   = item.SalesTotal - item.DiscountAmount;
                    item.Total   = item.NetTotal + item.TaxableItems.Sum(i => i.Amount) - item.ItemsDiscount;
                    item.ItemType = item.ItemCode?.Split('-')[0] == "EG" ? "EGS" : "GS1";
                }
                obj.Document.isInternallyCreated = true;
                obj.Document.ReceiverId = obj.Document.ReceiverType == "B" ? obj.Document.RGN : obj.Document.ReceiverType == "P" ? obj.Document.NID : obj.Document.PID;
                obj.Document.IssuerId = obj.Document.RegistrationNumber;
                obj.Document.IssuerName = obj.Document.TaxPayerNameEn;
                obj.Document.IssuerType = "B";
                obj.Document.InvoiceLines = obj.InvoiceLines;
                obj.Document.DateTimeIssued = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                obj.Document.DateTimeReceived = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                obj.Document.TotalSalesAmount = obj.Document.InvoiceLines.Sum(i => i.SalesTotal);
                obj.Document.TotalDiscountAmount = obj.Document.InvoiceLines.Sum(i => i.DiscountAmount);
                obj.Document.TotalItemsDiscountAmount = obj.Document.InvoiceLines.Sum(i => i.ItemsDiscount);
                obj.Document.Status = "New";
                obj.Document.NetAmount = obj.Document.TotalSalesAmount - obj.Document.TotalDiscountAmount;
                obj.Document.TotalItemsDiscountAmount = obj.Document.InvoiceLines.Sum(i => i.ItemsDiscount);
                obj.Document.TotalAmount = (obj.Document.NetAmount + obj.Document.InvoiceLines.SelectMany(b => b.TaxableItems)?.Distinct().Sum(x => x.Amount)) - (obj.Document.TotalItemsDiscountAmount);
                var response = _httpClient.POST("api/document/new", obj.Document).Message;
                if (response.ToLower() == "success")
                {
                    return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ActionName("ajax_edit_document")]
        public ActionResult ajax_edit_document(doc obj)
        {
            try
            {
                foreach (var item in obj.InvoiceLines)
                {
                    item.Id = item.Id?? Guid.NewGuid().ToString();
                    item.SalesTotal = item.Quantity * item.AmountEGP;
                    item.NetTotal = item.SalesTotal - item.DiscountAmount;
                    item.Total = item.NetTotal + item.TaxableItems.Sum(i => i.Amount) - item.ItemsDiscount;
                    item.ItemType = item.ItemCode?.Split('-')[0] == "EG" ? "EGS" : "GS1";
                    item.DocumentId = obj.Document.Id;
                }
                obj.Document.isInternallyCreated = true;
                obj.Document.ReceiverId = obj.Document.ReceiverType == "B" ? obj.Document.RGN : obj.Document.ReceiverType == "P" ? obj.Document.NID : obj.Document.PID;
                obj.Document.IssuerId = obj.Document.IssuerId;
                obj.Document.IssuerName = obj.Document.IssuerName;
                obj.Document.IssuerType = "B";
                obj.Document.InvoiceLines = obj.InvoiceLines;
                obj.Document.DateTimeIssued = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                obj.Document.DateTimeReceived = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                obj.Document.TotalSalesAmount = obj.Document.InvoiceLines.Sum(i => i.SalesTotal);
                obj.Document.TotalDiscountAmount = obj.Document.InvoiceLines.Sum(i => i.DiscountAmount);
                obj.Document.TotalItemsDiscountAmount = obj.Document.InvoiceLines.Sum(i => i.ItemsDiscount);
                obj.Document.Status = "New";
                obj.Document.NetAmount = obj.Document.TotalSalesAmount - obj.Document.TotalDiscountAmount;
                obj.Document.TotalItemsDiscountAmount = obj.Document.InvoiceLines.Sum(i => i.ItemsDiscount);
                obj.Document.TotalAmount = (obj.Document.NetAmount + obj.Document.InvoiceLines.SelectMany(b => b.TaxableItems)?.Distinct().Sum(x => x.Amount)) - (obj.Document.TotalItemsDiscountAmount);
                var response = _httpClient.POST("api/document/edit", obj.Document).Message;
                if (response.ToLower() == "success")
                {
                    return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [ActionName("printout")]
        public void GetDocumentPrintOut(string uuid)
        {
            try
            {
                var response = _httpClient.GET("api/document/printout?uuid=" + uuid, "application/pdf");
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContext.Response.Clear();
                    HttpContext.Response.ClearContent();
                    HttpContext.Response.ClearHeaders();
                    HttpContext.Response.AddHeader("content-disposition", "attachment; filename="+ uuid + ".pdf");
                    //Set the content type as file extension type  
                    HttpContext.Response.ContentType = "pdf";
                    //Write the file content  
                    HttpContext.Response.BinaryWrite(response.HttpResponseMessage.Content.ReadAsByteArrayAsync().Result);
                    HttpContext.Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [ActionName("canceldocument")]
        public ActionResult canceldocument(string uuid)
        {
            try
            {
                var response = _httpClient.GET("api/document/canceldocument?uuid=" + uuid);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (response.Message.ToLower() == "success")
                    {
                        return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("UpdateDocumentByInternalId")]
        public ActionResult UpdateDocumentByInternalId(string InternalId)
        {
            try
            {
                var response = _httpClient.GET("api/document/UpdateDocumentByInternalId?InternalId=" + InternalId);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (response.Message.ToLower() == "success")
                    {
                        return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("print")]
        public ActionResult GetDocumentPrint(string uuid)
        {
            try
            {
                var response = _httpClient.GET("api/document/printout?uuid=" + uuid, "application/pdf");
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {

                    return File(response.HttpResponseMessage.Content.ReadAsByteArrayAsync().Result, "application/pdf");
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
