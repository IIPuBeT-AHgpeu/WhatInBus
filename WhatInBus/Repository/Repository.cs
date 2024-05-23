using Microsoft.EntityFrameworkCore;
using WhatInBus.Database;

namespace WhatInBus.Repository
{
    public class Repository<T> : IRepository<T> where T : Recognize
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
                    if (_entities.Count() == 0) entity.Id = 1;
                    else
                    {
                        var orderedEntities = _entities.AsNoTracking().AsEnumerable().Order(new IdComparer());
                        entity.Id = ++orderedEntities.Last().Id;
                    }

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

        private class IdComparer : IComparer<T>
        {
            public int Compare(T? x, T? y)
            {
                if (x.Id > y.Id) return 1;
                else if (x.Id < y.Id) return -1;
                else return 0;
            }
        }
    }
}
