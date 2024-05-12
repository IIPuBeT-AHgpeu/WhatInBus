namespace WhatInBus
{
    public class ImageInDataset
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public byte[] Data { get; set; }
        public string GetFullName()
        {
            return Path + "\\" + FileName;
        }
    }
}
