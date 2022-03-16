using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Upload.DB.Repositories
{
    public abstract class RepositoryBase<T, U>
    where T : class, new()
    where U : DbContext, new()
    {
        public U entityContext { get; set; }
        protected abstract T AddEntity(T entity);
        protected abstract IQueryable<T> GetEntities();
        protected abstract T GetEntity(int id);
        protected abstract T UpdateEntity(T entity);
        protected abstract int GetMax();

        public int Add(T entity)
        {
            int result = 0;

            var obj = AddEntity(entity);
            result = entityContext.SaveChanges();

            return result;

        }

        public T AddWithGetObj(T entity)
        {
            var obj = AddEntity(entity);
            if (entityContext.SaveChanges() > 0)
            {
                return obj;
            }
            return null;
        }
        public int Remove(T entity)
        {

            entityContext.Entry<T>(entity).State = EntityState.Deleted;
            return entityContext.SaveChanges();

        }

        public int Remove(int id)
        {

            T entity = GetEntity(id);
            entityContext.Entry<T>(entity).State = EntityState.Deleted;
            return entityContext.SaveChanges();

        }

        public int update(T entity)
        {
            try
            {
                T existingEntity = UpdateEntity(entity);

                //SimpleMapper.PropertyMap(entity, existingEntity);

                return entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var x = ex.InnerException;
                return 0;
            }


        } //Need to re-factor

        public IQueryable<T> Get()
        {
            return GetEntities();

        }

        public int GetMaxID()
        {
            return GetMax();
        }

        public T Get(int id)
        {

            return GetEntity(id);
        }

        public T AddorUpdate(T obj, int id)
        {
            T result = null;
            if (id == default(int))
            {
                result = AddWithGetObj(obj);
            }
            else
            {
                T model = UpdateEntity(obj);
                if (model != null)
                {
                    entityContext.Entry(model).State = EntityState.Detached;
                    entityContext.Set<T>().Attach(obj);
                    entityContext.Entry<T>(obj).State = EntityState.Modified;
                    if (entityContext.SaveChanges() > 0)
                        result = obj;
                }

            }

            return result;
        }

        public T UpdateObj(T obj)
        {
            T result = null;
            var model = UpdateEntity(obj);

            //SimpleMapper.PropertyMap(obj, model);
            if (entityContext.SaveChanges() > 0)
                result = model;

            return result;
        } //Need to re-factor
    }
}
