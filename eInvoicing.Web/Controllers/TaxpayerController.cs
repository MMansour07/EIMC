using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eInvoicing.DTO;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Newtonsoft.Json;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class TaxpayerController : Controller
    {
        private readonly IUserSession _userSession;
        private readonly IHttpClientHandler _httpClient;
        public TaxpayerController(IHttpClientHandler httpClient, IUserSession userSession)
        {
            _httpClient = httpClient;
            _userSession = userSession;
        }
        
        [HttpGet]
        [ActionName("taxpayer_details")]
        public ActionResult taxpayer_details()
        {
            return View();
        }
        
        [HttpGet]
        [ActionName("ajaxtaxpayerdetails")]
        public ActionResult ajaxtaxpayerdetails()
        {
            try
            {
                var response = JsonConvert.DeserializeObject<TaxpayerDTO>(_httpClient.GET("api/taxpayer/details").Info);
                if (response != null)
                {
                    return Json(new { status = "Success", data = response }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Failed"}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Failed"}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("token")]
        public ActionResult token()
        {
            TaxpayerDTO taxpayerDTO = new TaxpayerDTO();
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
                    using (StreamReader r = new StreamReader(fname))
                    {
                        string json = r.ReadToEnd();
                        taxpayerDTO = JsonConvert.DeserializeObject<TaxpayerDTO>(json);
                    }
                    taxpayerDTO.IRN = taxpayerDTO.Id;
                    taxpayerDTO.Id = 0;
                    taxpayerDTO.Status = true;
                    taxpayerDTO.CreatedBy = User.Identity.Name;
                    var response = _httpClient.POST("api/taxpayer", taxpayerDTO);
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}