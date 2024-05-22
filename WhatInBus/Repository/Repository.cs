using Microsoft.EntityFrameworkCore;
using WhatInBus.Database;

namespace WhatInBus.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private PfHistoryContext _db;
        private DbSet<T> _entities;

        public Repository(PfHistoryContext db)
        {
            _db = db;
            _entities = _db.Set<T>();
        }

        public bool Create(T entity)
        {
            try
            {
                if (entity == null) return false;
                else
                {
                    _entities.Add(entity);
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                T? entity = _entities.Find(id);

                if (entity == null) return false;
                else
                {
                    _entities.Remove(entity);

                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsNoTracking().ToList();
        }

        public T? GetOne(int id)
        {
            return _entities.Find(id);
        }

        public bool Update(T entity)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
