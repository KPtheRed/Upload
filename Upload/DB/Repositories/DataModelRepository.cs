using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.DB.Models;

namespace Upload.DB.Repositories
{
    public class DataModelRepository : RepositoryBase<DataModel, DataContext>
    {
        public DataModelRepository()
        {
            this.entityContext = new DataContext();
        }

        public DataModelRepository(DataContext dbContext)
        {
            this.entityContext = dbContext;
        }
        protected override DataModel AddEntity(DataModel entity)
        {
            return entityContext.DataModel.Add(entity).Entity;
        }

        protected override IQueryable<DataModel> GetEntities()
        {
            return entityContext.DataModel.AsQueryable();
        }

        protected override DataModel GetEntity(int id)
        {
            return entityContext.DataModel.Where(a => a.DataModelID == id).FirstOrDefault();
        }

        protected override int GetMax()
        {
            throw new NotImplementedException();
        }

        protected override DataModel UpdateEntity(DataModel entity)
        {
            return entityContext.DataModel.FirstOrDefault(x => x.DataModelID == entity.DataModelID);
        }
    }
}
