using System.Drawing;

namespace WhatInBus.Croppers
{
    public interface ICropper<T> where T : struct
    {
        public byte[] CropImage(byte[] image, T area);
    }
}
