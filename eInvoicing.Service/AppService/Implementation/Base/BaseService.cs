using eInvoicing.DomainEntities.Base;
using eInvoicing.DTO.Base;
using eInvoicing.Repository.Contract.Base;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.Helper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Implementation.Base
{
    public class BaseService<TEntity, TDTO> : IService<TDTO> where TEntity : IEntity where TDTO : IDTO
    {
        protected string UserId { get; private set; }
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            Repositories.Add(_repository);
        }

        public List<object> Repositories { get; set; } = new List<object>();

        public TDTO Add(TDTO dto)
        {
            var entity = dto.ToEntity<TEntity>();
            var updatedEnity = _repository.Add(entity);
            _unitOfWork.Save();
            return updatedEnity.ToDto<TDTO>();
        }
        public void InsertBulk(IEnumerable<IDTO> dto)
        {
            try
            {
                var entities = dto.ToEntityList<TEntity>();
                _repository.AddRange(entities);
                _unitOfWork.Save();
            }
            catch(Exception e)
            {
                throw e;
            }
           
        }

        public async Task<TDTO> AddAsync(TDTO dto)
        {
            var entity = dto.ToEntity<TEntity>();
            var updatedEnity = _repository.Add(entity);
            await _unitOfWork.SaveAsync();
            return updatedEnity.ToDto<TDTO>();
        }

        public TDTO Update(TDTO dto)
        {
            var entity = dto.ToEntity<TEntity>();
            var updatedEnity = _repository.Update(entity);
            _unitOfWork.Save();
            return updatedEnity.ToDto<TDTO>();
        }

        public void Update(TDTO dto, params string[] properties)
        {
            var entity = dto.ToEntity<TEntity>();
            _repository.Update(entity, properties);
            _unitOfWork.Save();
        }

        public async Task<TDTO> UpdateAsync(TDTO dto)
        {
            var entity = dto.ToEntity<TEntity>();
            var updatedEnity = _repository.Update(entity);
            await _unitOfWork.SaveAsync();
            return updatedEnity.ToDto<TDTO>();
        }

        public TDTO Delete(string id)
        {
            var updatedEnity = _repository.Delete(id);
            _unitOfWork.Save();
            return updatedEnity.ToDto<TDTO>();
        }

        public async Task<TDTO> DeleteAsync(string id)
        {
            var updatedEnity = await _repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return updatedEnity.ToDto<TDTO>();
        }

        public TDTO Get(string id)
        {
            var obj = _repository.Get(id).ToDto<TDTO>();
            return obj;
        }

        public async Task<TDTO> GetAsync(string id)
        {
            var entity = await _repository.GetAsync(id);
            return entity.ToDto<TDTO>();
        }

        public IEnumerable<TDTO> GetAll(int pageNumber, int pageSize, string sort, bool ascending = true)
        {
            return _repository.GetAll(pageNumber, pageSize, sort, ascending).Select(e => e.ToDto<TDTO>());
        }
        public IEnumerable<TDTO> GetAllIncluding()
        {
            return _repository.GetAllIncluding(null, null, "ISSUER, RECEIVER, PAYMENT, DELIVERY, INVOICELINES, TAXTOTALS, SIGNATURES").Select(e => e.ToDto<TDTO>());
        }

        public async Task<IEnumerable<TDTO>> GetAllAsync(int pageNumber, int pageSize, string sort, bool @ascending = true)
        {
            var result = await _repository.GetAllAsync(pageNumber, pageSize, sort, ascending);
            return result.Select(e => e.ToDto<TDTO>());
        }

        public void SetUserId(string userId)
        {
            this.UserId = userId;
            _repository.SetUserId(this.UserId);
        }
    }
}
