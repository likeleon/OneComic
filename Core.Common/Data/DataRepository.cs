using Core.Common.Contracts;
using Core.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Core.Common.Data
{
    public abstract class DataRepository<T, U> : IDataRepository<T>
        where T : class, IIdentifiableEntity, new()
        where U : DbContext, new()
    {
        public RepositoryActionResult<T> Add(T entity)
        {
            try
            {
                using (var context = new U())
                {
                    var addedEntity = AddEntity(context, entity);
                    var result = context.SaveChanges();
                    if (result > 0)
                        return Created(addedEntity);
                    else
                        return NothingModified(addedEntity);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        // TODO: NotFound State
        public RepositoryActionResult<T> Remove(T entity)
        {
            try
            {
                using (var context = new U())
                {
                    context.Entry(entity).State = EntityState.Deleted;
                    context.SaveChanges();
                    return Deleted();
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public RepositoryActionResult<T> Remove(int id)
        {
            try
            {
                using (var context = new U())
                {
                    var entity = GetEntity(context, id);
                    if (entity == null)
                        return NotFound();

                    context.Entry(entity).State = EntityState.Deleted;
                    context.SaveChanges();
                    return Deleted();
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public RepositoryActionResult<T> Update(T entity)
        {
            try
            {
                using (var context = new U())
                {
                    var existingEntity = GetEntity(context, entity.EntityId);
                    if (existingEntity == null)
                        return NotFound();

                    SimpleMapper.PropertyMap(entity, existingEntity);

                    var result = context.SaveChanges();
                    if (result > 0)
                        return Updated(existingEntity);
                    else
                        return NothingModified(existingEntity);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public IReadOnlyList<T> Get()
        {
            using (var context = new U())
                return GetEntities(context).ToList();
        }

        public T Get(int id)
        {
            using (var context = new U())
                return GetEntity(context, id);
        }

        protected abstract T AddEntity(U context, T entity);
        protected abstract IQueryable<T> GetEntities(U context);
        protected abstract T GetEntity(U context, int id);

        private RepositoryActionResult<T> Created(T entity)
        {
            return new RepositoryActionResult<T>(entity, RepositoryActionState.Created);
        }

        private RepositoryActionResult<T> NothingModified(T entity)
        {
            return new RepositoryActionResult<T>(entity, RepositoryActionState.NothingModified);
        }

        private RepositoryActionResult<T> Deleted()
        {
            return new RepositoryActionResult<T>(null, RepositoryActionState.Deleted);
        }

        private RepositoryActionResult<T> NotFound()
        {
            return new RepositoryActionResult<T>(null, RepositoryActionState.NotFound);
        }

        private RepositoryActionResult<T> Updated(T entity)
        {
            return new RepositoryActionResult<T>(entity, RepositoryActionState.Updated);
        }

        private RepositoryActionResult<T> Error(Exception ex)
        {
            return new RepositoryActionResult<T>(null, RepositoryActionState.Error, ex);
        }
    }
}
