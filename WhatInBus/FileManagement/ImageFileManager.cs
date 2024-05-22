namespace WhatInBus.FileManagement
{
    public class ImageFileManager : IFileManager<ImageInDataset>
    {
        public async Task<ImageInDataset> GetFileAsync(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            if (!File.Exists(path)) throw new FileNotFoundException("file doesnt exist");

            var image = new ImageInDataset();
            try
            {
                image.Path = Path.GetDirectoryName(path)!;
                image.FileName = Path.GetFileName(path);
                image.Data = await File.ReadAllBytesAsync(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return image;
        }

        public void SaveFileAsync(ImageInDataset file)
        {
            if (file == null) throw new ArgumentNullException("file");

            if (!Directory.Exists(file.Path)) Directory.CreateDirectory(file.Path);

            string fullName = file.GetFullName();

            if (File.Exists(fullName)) throw new Exception("file already exist");

            try
            {
                File.WriteAllBytesAsync(fullName, file.Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveFilesAsync(ICollection<ImageInDataset> files)
        {
            if (files == null) throw new ArgumentNullException("files");

            try
            {
                foreach (var file in files)
                {
                    SaveFileAsync(file);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<ImageInDataset>> GetFilesAsync(string directory)
        {
            if (directory == null) throw new ArgumentNullException("directory");

            if (!Directory.Exists(directory)) throw new DirectoryNotFoundException("directory doesnt exist");

            try
            {
                List<string> files = Directory.EnumerateFiles(directory).ToList();

                List<ImageInDataset> images = new List<ImageInDataset>();

                foreach (string file in files)
                {
                    images.Add(await GetFileAsync(file));
                }

                return images;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
