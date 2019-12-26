using ChannakyaAccounting.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Repository.UnitOfWork.Interface_Classes
{
   public interface IGenericUnitOfWork
    {
        int Commit();
        ChannakyaAccEntities GetContext();
        int executeProcedure(string query, params object[] parameter);
    }
}
