using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetBy(Func<TEntity, bool> predicate);
        TEntity SingleBy(Func<TEntity, bool> predicate);
        bool AddRange(IEnumerable<TEntity> entities);
        TEntity Get(object id);
        bool Add(TEntity entity);
        bool Edit(TEntity entity);
        bool Remove(object id);
        bool Remove(TEntity entity);
        bool RemoveRange(IEnumerable<TEntity> entities);

    }
}
