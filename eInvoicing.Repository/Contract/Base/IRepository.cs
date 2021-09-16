using eInvoicing.DomainEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Contract.Base
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {

        IQueryable<TEntity> All { get; }
        bool CheckIsExist(Expression<Func<TEntity, bool>> filter);
        TEntity Add(TEntity entity);
        //Task<TEntity> AddAsync(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        //Task AddRangeAsync(IEnumerable<TEntity> dto);
        TEntity Update(TEntity entity);
        void Update(TEntity entity, params string[] properties);
        void UpdateBulk(TEntity entity);
        TEntity Delete(string id);
        Task<TEntity> DeleteAsync(string id);
        void DeleteRange(IList<string> idList);
        void DeleteRange(string Id);
        void SaveChanges();
        IEnumerable<TEntity> GetAll(int pageNumber, int pageSize, string sort, bool ascending);
        //IQueryable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int pageSize, string sort, bool ascending);

        TEntity Get(string id);
        Task<TEntity> GetAsync(string id);
        bool IsExist<TSource>(Expression<Func<TSource, bool>> filter) where TSource : BaseEntity;
        void SetUserId(string userId);
        //void SetRolePermission(IList<RolePermission> rolePermissions);

        IEnumerable<TEntity> GetAllIncluding(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          string includeProperties = "");
        IQueryable<TEntity> Get(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          string includeProperties = "");
        int Count(Expression<Func<TEntity, bool>> filter = null);
    }
}
