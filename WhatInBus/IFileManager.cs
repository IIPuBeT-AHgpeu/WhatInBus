namespace WhatInBus
{
    public interface IFileManager<T> where T : class
    {
        public Task<T> GetFileAsync(string path);
        public void SaveFileAsync(T file);
        public Task<ICollection<T>> GetFilesAsync(string directory);
        public void SaveFilesAsync(ICollection<T> files);
    }
}
