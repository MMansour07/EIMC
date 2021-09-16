using eInvoicing.DomainEntities.Base;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace eInvoicing.Repository.Implementation.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public T Save<T>(T entity) where T : IEntity
        {
            SetAuditProperties();
            _context.SaveChanges();
            return entity;
        }

        public void Save()
        {
            try
            {
                //SetAuditProperties();
                var dd = _context.SaveChanges();

            }
            catch (DbUpdateException ex)
            {

                Console.WriteLine(ex);
                throw ex;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw e;
            }
        }

        public Task SaveAsync()
        {
            try
            {
                SetAuditProperties();
                return _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private void SetAuditProperties()
        {
            var entities = _context.ChangeTracker
                                   .Entries()
                                   .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added && x.Entity != null && typeof(BaseEntity).IsAssignableFrom(x.Entity.GetType()))
                                   .ToList();

            foreach (var entry in entities)
            {
                if (entry.Entity is BaseEntity)
                {
                    var entity = entry.Entity as BaseEntity;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            {
                                entity.CreatedOn = DateTime.Now;

                                entity.CreatedBy = "";

                                //entity.ModifiedOn = DateTime.Now;

                                break;
                            }
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            {
                                entity.ModifiedOn = DateTime.Now;
                                _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                                _context.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                                entity.CreatedBy = entity.CreatedBy?.Trim();

                                entity.LastModifiedBy = "";
                                break;
                            }
                    }
                }
            }
        }


    }
}
