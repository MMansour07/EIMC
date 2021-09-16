using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IService<TDTO> where TDTO : IDTO
    {
        List<object> Repositories { get; set; }
        TDTO Add(TDTO DTO);
        void InsertBulk(IEnumerable<IDTO> dto);
        Task<TDTO> AddAsync(TDTO DTO);
        TDTO Update(TDTO DTO);
        void Update(TDTO DTO, params string[] properties);
        Task<TDTO> UpdateAsync(TDTO DTO);
        TDTO Delete(string id);
        Task<TDTO> DeleteAsync(string id);
        TDTO Get(string id);
        Task<TDTO> GetAsync(string id);
        IEnumerable<TDTO> GetAll(int pageNumber, int pageSize, string sort, bool ascending = true);
        IEnumerable<TDTO> GetAllIncluding();
        Task<IEnumerable<TDTO>> GetAllAsync(int pageNumber, int pageSize, string sort, bool ascending = true);
        void SetUserId(string userId);
    }
}
