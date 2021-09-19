using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using eInvoicing.Service.Helper;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

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
                var sortDirection = Request["sort[sort]"];
                var sortColumnName = Request["sort[field]"];
                var searchValue = Request["query[generalSearch]"];
                var status = Request["query[status]"];
                string url = "api/document/pending?pageNumber=" + Convert.ToInt32(pageNumber) + "&pageSize=" +
                    Convert.ToInt32(pageSize) + "&searchValue=" + searchValue + "&sortColumnName=" + sortColumnName + "&sortDirection=" + sortDirection + "&status=" + status;
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
        [ActionName("submitteditems")]
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
                var sortDirection = Request["sort[sort]"];
                var sortColumnName = Request["sort[field]"];
                var searchValue = Request["query[generalSearch]"];
                var status = Request["query[status]"];
                string url = "api/document/submitted?pageNumber=" + Convert.ToInt32(pageNumber) + "&pageSize=" +
                    Convert.ToInt32(pageSize) + "&searchValue=" + searchValue + "&sortColumnName=" + sortColumnName + "&sortDirection=" + sortDirection + "&status=" + status;
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

                var result = JsonConvert.DeserializeObject<GetDocumentResponse>(_httpClient.GET("api/document/raw?uuid=" + uuid).Info);
                GetDocumentResponse document = result;
                if (document != null)
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
                }
                return View(document);
            }
            catch (Exception ex)
            {
                // display error page
                return View(ex.Message.ToString());
            }
        }
    }
}
