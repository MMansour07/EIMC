using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using DatingApp.API.Dtos;
using eInvoicing.API.Filters;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using ProductLicense;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class BusinessGroupController : ApiController
    {
        private readonly IBusinessGroupService _auth;
        public BusinessGroupController(IBusinessGroupService auth)
        {
            _auth = auth;
        }
        
        [HttpPost]
        [Route("api/BG/Create")]
        public IHttpActionResult Create(BusinessGroupDTO model)
        {
            try
            {
                if (!_auth.Create(model))
                {
                    return InternalServerError();
                }
                {
                    Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                    var connectionStringsSection = (ConnectionStringsSection)objConfig.GetSection("connectionStrings");
                    //Edit
                    if (objAppsettings != null)
                    {
                        if (objAppsettings.Settings["Environment"].Value == "Prod")
                        {
                            connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings()
                            {
                                Name = "EInvoice_" + model.GroupName?.Replace(" ", ""),
                                ConnectionString = "Data Source=.;Initial Catalog=" + model.GroupName?.Replace(" ", "") + ";User ID=sa;Password=123",
                                ProviderName = "System.Data.SqlClient"
                            });
                        }
                        else
                        {
                            connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings()
                            {
                                Name = "EInvoice_" + model.GroupName?.Replace(" ", ""),
                                ConnectionString = "Data Source=.;Initial Catalog=" + model.GroupName?.Replace(" ", "") + "_PreProd;User ID=sa;Password=123",
                                ProviderName = "System.Data.SqlClient"
                            });
                        }
                        objConfig.Save();
                        return Ok();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [HttpGet]
        [Route("api/BG/GetGroups")]
        public IHttpActionResult GetGroups()
        {
            try
            {
                var users = _auth.GetBusinessGroups();
                return Ok(users.ToList());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/BG/edit")]
        public IHttpActionResult edit(string Id)
        {
            try
            {
                var Group = _auth.GetBusinessGroup(Id);
                return Ok(Group);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/BG/edit")]
        public IHttpActionResult edit(BusinessGroupDTO obj)
        {
            try
            {
                if(_auth.Edit(obj))
                return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/BG/delete")]
        public IHttpActionResult Delete(string Id, string GrpName)
        {
            try
            {
                if (_auth.Delete(Id))
                {
                    Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                    var connectionStringsSection = (ConnectionStringsSection)objConfig.GetSection("connectionStrings");
                    //Edit
                    if (objAppsettings != null)
                    {
                        if (objAppsettings.Settings["Environment"].Value == "Prod")
                        {
                            connectionStringsSection.ConnectionStrings.Remove(new ConnectionStringSettings()
                            {
                                Name = "EInvoice_" + GrpName,
                                ConnectionString = "Data Source=.;Initial Catalog=" + GrpName + ";User ID=sa;Password=123",
                                ProviderName = "System.Data.SqlClient"
                            });
                        }
                        else
                        {
                            connectionStringsSection.ConnectionStrings.Remove(new ConnectionStringSettings()
                            {
                                Name = "EInvoice_" + GrpName,
                                ConnectionString = "Data Source=.;Initial Catalog=" + GrpName + "_PreProd;User ID=sa;Password=123",
                                ProviderName = "System.Data.SqlClient"
                            });
                        }
                        objConfig.Save();
                        return Ok();
                    }
                }
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}