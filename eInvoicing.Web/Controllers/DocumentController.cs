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
        [AllowAnonymous]
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
                DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : Convert.ToDateTime(fromDate);
                DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : Convert.ToDateTime(toDate);

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
                        success = true
                    }, JsonRequestBehavior.AllowGet);
                }
                // Returns message that successfully uploaded  
                return Json(new { success = false, message = "File is empty!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Invalid Template!"}, JsonRequestBehavior.AllowGet);
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
                return Json(JsonConvert.DeserializeObject<GetDocumentResponse>(response.Info).document.invoiceLines, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Calling Preparation error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [ActionName("submitted")]
        public ActionResult GetSubmittedDocuments()
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
                DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : Convert.ToDateTime(fromDate);
                DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : Convert.ToDateTime(toDate);

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
        [HttpGet]
        [ActionName("raw")]
        public ActionResult GetDocumentbyuuid(string uuid)
        {
            try
            {
                GetDocumentResponse document = new GetDocumentResponse();
                var obj = _httpClient.GET("api/document/raw?uuid=" + uuid)?.Info;
                if (obj != null)
                {
                    var result = JsonConvert.DeserializeObject<GetDocumentResponse>(obj);
                    document = result;
                    if (document.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        document.dateTimeIssued = DateTime.Parse(document.dateTimeIssued).ToString("dd-MMM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                        document.dateTimeReceived = DateTime.Parse(document.dateTimeReceived).ToString("dd-MMM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                        var status = new List<Catalog>() {
                            new Catalog(){Value = "valid",    Text = "bg-success" },
                            new Catalog(){Value = "invalid",  Text = "bg-danger" },
                            new Catalog(){Value = "submitted",Text = "bg-primary" },
                            new Catalog(){Value = "cancelled",Text = "bg-info" },
                            new Catalog(){Value = "rejected", Text = "bg-warning" } };
                        var docType = new List<Catalog>()
                            {
                             new Catalog(){ Value = "i", Text = "Invoice" },
                             new Catalog(){ Value = "c", Text = "Credit" },
                             new Catalog(){ Value = "d", Text = "Debit" }
                            };
                        document.typeName = docType.FirstOrDefault(i => i.Value == document.typeName.ToLower())?.Text;
                        document.statusClass = status.FirstOrDefault(i => i.Value == document.status.ToLower())?.Text;
                        ViewBag.IsExist = true;
                    }
                    else
                        ViewBag.IsExist = false;
                }
                else
                {
                    ViewBag.IsExist = false;
                }
                return View(document);
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
                var response = JsonConvert.DeserializeObject<NewDocumentVM>(_httpClient.GET("api/taxpayer/details").Info);
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
            catch
            {
                return View();
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
                }
                obj.Document.ReceiverId = obj.Document.ReceiverType == "B" ? obj.Document.RGN : obj.Document.ReceiverType == "P" ? obj.Document.NID : obj.Document.PID;
                obj.Document.IssuerId = obj.Document.RegistrationNumber;
                obj.Document.IssuerName = obj.Document.TaxPayerNameEn;
                obj.Document.IssuerType = "B";
                obj.Document.InvoiceLines = obj.InvoiceLines;
                obj.Document.DateTimeIssued = DateTime.ParseExact(DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                obj.Document.DateTimeReceived = DateTime.ParseExact(DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
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
                return Json(new { status = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
