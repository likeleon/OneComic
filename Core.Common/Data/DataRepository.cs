using Core.Common.Contracts;
using Core.Common.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Core.Common.Data
{
    public abstract class DataRepository<T, U> : IDataRepository<T>
        where T : class, IIdentifiableEntity, new()
        where U : DbContext, new()
    {
        public T Add(T entity)
        {
            using (var context = new U())
            {
                var addedEntity = AddEntity(context, entity);
                context.SaveChanges();
                return addedEntity;
            }
        }

        public void Remove(T entity)
        {
            using (var context = new U())
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (var context = new U())
            {
                var entity = GetEntity(context, id);
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public T Update(T entity)
        {
            using (var context = new U())
            {
                var existingEntity = GetEntity(context, entity.EntityId);

                SimpleMapper.PropertyMap(entity, existingEntity);

                context.SaveChanges();
                return existingEntity;
            }
        }

        public IEnumerable<T> Get()
        {
            using (var context = new U())
                return (GetEntities(context)).ToList();
        }

        public T Get(int id)
        {
            using (var context = new U())
                return GetEntity(context, id);
        }

        protected abstract T AddEntity(U context, T entity);
        protected abstract IEnumerable<T> GetEntities(U context);
        protected abstract T GetEntity(U context, int id);
    }
}
