using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract;
using eInvoicing.Service.Helper;
using DatingApp.API.Dtos;
using eInvoicing.DTO;
using eInvoicing.Service.Helper.Extension;
using System.Diagnostics;

namespace eInvoicing.Service.AppService.Implementation
{
    public class BusinessGroupService : IBusinessGroupService
    {
        private readonly IBusinessGroupRepository repository;

        public BusinessGroupService(IBusinessGroupRepository _repository)
        {
            this.repository = _repository;
        }
        public bool Create(BusinessGroupDTO model)
        {
            try
            {
                var businessGroup = AutoMapperConfiguration.Mapper.Map<BusinessGroup>(model);
                businessGroup.Id = Guid.NewGuid().ToString();
                var res = repository.Add(businessGroup);
                if (res != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : {0}", ex.Message);
                return false;
            }
        }
        public IEnumerable<BusinessGroupDTO> GetBusinessGroups()
        {
            try
            {
                var Groups = repository.GetAllIncluding(u => u.GroupName != "Product Owner", null, "").ToList();
                return AutoMapperConfiguration.Mapper.Map<IEnumerable<BusinessGroupDTO>>(Groups); ;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : {0}", ex.Message);
                return new List<BusinessGroupDTO>();
            }
        }
        public BusinessGroupDTO GetBusinessGroup(string Id)
        {
            try
            {
                return AutoMapperConfiguration.Mapper.Map<BusinessGroupDTO>(repository.GetAllIncluding(i => i.Id == Id, null, "")?.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : {0}", ex.Message);
                return new BusinessGroupDTO();
            }
        }
        public bool Edit(BusinessGroupDTO obj)
        {
            try
            {
                var res = repository.Get(obj.Id);
                res.BusinessType = obj.Email;
                res.SyncType = obj.SyncType;
                res.IsDBSync = obj.IsDBSync;
                res.Phone = obj.Phone;
                res.Email = obj.Email;
                res.Address = obj.Address;
                var Group = repository.Update(res);

                if (Group != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : {0}", ex.Message);
                return false;
            }
        }
        public bool Delete(string id)
        {
            try
            {
                repository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //Debug.WriteLine("Error : {0}", ex.Message);
                //return true;
            }
        }
    }
}
