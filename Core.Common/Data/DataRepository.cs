using Core.Common.Contracts;
using Core.Common.Extensions;
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

                    context.Entry(existingEntity).State = EntityState.Detached;
                    AttachEntity(context, entity);
                    context.Entry(entity).State = EntityState.Modified;

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
            return Get(order: null);
        }

        public IReadOnlyList<T> Get(string order)
        {
            using (var context = new U())
            {
                var query = GetEntities(context);

                if (order != null)
                    query = query.ApplySort(order);

                return query.ToList();
            }
        }

        public DataPage<T> Get(string order, int page, int pageSize)
        {
            using (var context = new U())
            {
                var query = GetEntities(context);
                return GetPage(query, order, page, pageSize);
            }
        }

        protected DataPage<T> GetPage(IQueryable<T> query, string order, int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page), "Page should be a positive value");

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page should be a positive value");

            if (order != null)
                query = query.ApplySort(order);

            var totalCount = query.Count();

            var entities = query
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToList();

            return new DataPage<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Entities = entities
            };
        }

        public T Get(int id)
        {
            using (var context = new U())
                return GetEntity(context, id);
        }

        protected abstract T AddEntity(U context, T entity);
        protected abstract IQueryable<T> GetEntities(U context);
        protected abstract T GetEntity(U context, int id);
        protected abstract void AttachEntity(U context, T entity);

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
