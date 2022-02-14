using eInvoicing.DomainEntities.Base;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Implementation.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public string ConnectionString { get; set; }
        protected Repository(ApplicationContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }
        public void GetTheConnectionString(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
            Context.Database.Connection.ConnectionString = ConnectionString?? "Data Source=.;Initial Catalog=EIMC_Preprod;persist security info=True; Integrated Security=SSPI;";
        }

        public IQueryable<TEntity> All { get { return DbSet.AsNoTracking(); } }

        protected ApplicationContext Context { get; }

        protected DbSet<TEntity> DbSet { get; set; }

        protected string UserId { get; private set; }

        public virtual TEntity Add(TEntity entity)
        {
            var res =  DbSet.Add(entity);
            Context.SaveChanges();
            return res;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
        //public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        //{
        //    var res = DbSet.AddRange(entities);
        //    Context.SaveChangesAsync();
        //    return res;
        //}
        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            var res = DbSet.AddRange(entities);
            Context.SaveChanges();
            return res;
        }

        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                DbSet.AddOrUpdate(entity);
                Context.SaveChanges();
                return entity;
            }
            catch (Exception e) { throw e; }
        }

        public virtual TEntity ModifyById(TEntity entity)
        {
            try
            {
                //Context.Entry<TEntity>(entity).State = EntityState.Modified;
                entity = DbSet.Attach(entity);
                Context.SaveChanges();
                return entity;
            }
            catch (Exception e) { throw e; }
        }
        public void UpdateBulk(TEntity entity)
        {
            //UpdateCreatedAndModifiedDates(entity);
            try
            {
                //Context.Entry<TEntity>(entity).State = EntityState.Modified;
                //entity = DbSet.Attach(entity);
                DbSet.AddOrUpdate(entity);
                Context.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        public void Update(TEntity entity, params string[] properties)
        {
            //UpdateCreatedAndModifiedDates(entity);
            try
            {
                Context.Entry<TEntity>(entity).State = EntityState.Modified;
                entity = DbSet.Attach(entity);
                Context.Entry<TEntity>(entity).Property("ModifiedOn").IsModified = true;
            }
            catch (Exception e) { throw e; }
            if (properties != null)
            {
                foreach (var prp in properties)
                {
                    try
                    {
                        if (prp != null)
                        {
                            Context.Entry<TEntity>(entity).Property(prp).IsModified = true;
                        }
                    }
                    catch (Exception e) { throw e; }
                }
            }
        }

        public TEntity Delete(string id)
        {
            var entity = DbSet.Find(id);
            if (entity != null)
            {
                var RemovedEntity = DbSet.Remove(entity);
                Context.SaveChanges();
                return RemovedEntity;
            }
            return null;
        }

        public async Task<TEntity> DeleteAsync(string id)
        {
            var entity = await DbSet.FindAsync(id);
            return DbSet.Remove(entity);
        }

        public void DeleteRange(IList<string> idList)
        {
            var entities = DbSet.Where(e => idList.Contains(e.Id));
            DbSet.RemoveRange(entities);
        }

        public void DeleteRange(string Id)
        {
            var entities = DbSet.Where(e => e.Id == Id);
            DbSet.RemoveRange(entities);
        }

        public IEnumerable<TEntity> GetAll(int pageNumber = 1, int pageSize = 20, string sort = null, bool ascending = true)
        {
            if (sort == null)
                return All.OrderBy(e => e.Id).ToList();

            if (ascending)
            {
                return All.OrderBy(e => sort).ToList();
            }
            return All.OrderByDescending(e => sort).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber = 1, int pageSize = 20, string sort = null, bool ascending = true)
        {
            if (sort == null)
                return await All.OrderBy(e => e.Id).ToListAsync();

            if (ascending)
            {
                return await All.OrderBy(e => sort).ToListAsync();
            }
            return await All.OrderByDescending(e => sort).ToListAsync();

        }


        public virtual TEntity Get(string id)
        {
            try
            {
                return DbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> GetAsync(string id)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool IsExist<TSource>(Expression<Func<TSource, bool>> filter) where TSource : BaseEntity
        {
            var lookupSet = Context.Set<TSource>();
            return lookupSet.AsNoTracking().Any(filter);
        }


        public void SetUserId(string userId)
        {
            this.UserId = userId;
        }

        public IEnumerable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public bool CheckIsExist(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = DbSet;
            if (query.Any(filter))
                return true;
            else
                return false;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }
        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                return query.Where(filter).Count();
            }
            return 0;
        }

    }
}
