using ChannakyaAccounting.Models.Models;
using ChannakyaAccounting.Repository.Repository;
using ChannakyaAccounting.Repository.UnitOfWork.Interface_Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

using ChannakyaAccounting.Repository.Repository.Implementation_Classes;

namespace ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes
{
    public sealed class GenericUnitOfWork : IGenericUnitOfWork, IDisposable
    {
        private ChannakyaAccEntities entities = null;
        public DbContext _context = null;
        
        
        public GenericUnitOfWork()
        {
            entities = new ChannakyaAccEntities();
            
           
        }


        public Dictionary<System.Type, object> repositories = new Dictionary<System.Type, object>();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IGenericRepository<T>;
            }
            IGenericRepository<T> repo = new GenericRepository<T>(entities);
            repositories.Add(typeof(T), repo);
            return repo;
        }
       
        //public void ExecStoredProcedure()
        //{

        //}

        public int Commit()
        {
            return entities.SaveChanges();
        }

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ChannakyaAccEntities GetContext()
        {
            ChannakyaAccEntities chaAcc = new ChannakyaAccEntities();
            return chaAcc;
            
        }

        public int executeProcedure(string query, params object[] parameter)
        {
          return  entities.Database.ExecuteSqlCommand("exec" + query, parameter);
           
        }
    }
}
