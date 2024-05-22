namespace WhatInBus.Repository
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public T? GetOne(int id);
        public bool Update(T entity);
        public bool Delete(int id);
        public bool Create(T entity);
    }
}
