using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected Group4_DB _ctx;
        protected DbSet<TEntity> tbl;
        public Repository()
        {
            _ctx = new Group4_DB();
            tbl = _ctx.Set<TEntity>();
        }
        public bool Add(TEntity entity)
        {
            try
            {
                tbl.Add(entity);
                _ctx.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Edit(TEntity entity)
        {
            try
            {
                _ctx.Entry(entity).State = EntityState.Modified;
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public TEntity Get(object id)
        {
            return tbl.Find(id);
        }
        public TEntity GetId()
        {
            return tbl.Find(GetAll().LastOrDefault());
        }
        public IEnumerable<TEntity> GetAll()
        {
            return tbl;
        }
        public bool Remove(object id)
        {
            try
            {
                tbl.Remove(Get(id));
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TEntity> GetBy(Func<TEntity, bool> predicate)
        {
            return tbl.Where(predicate);
        }

        public TEntity SingleBy(Func<TEntity, bool> predicate)
        {
            return tbl.FirstOrDefault(predicate);
        }

        public bool AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                tbl.AddRange(entities);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false; ;
            }
        }

        public bool Remove(TEntity entity)
        {
            try
            {
                tbl.Remove(entity);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public void Save()
        {
            _ctx.SaveChanges();
        }
        public bool RemoveRange(IEnumerable<TEntity> entities)
        {
            try
            {
                tbl.RemoveRange(entities);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
