using System.Drawing;

namespace WhatInBus.Croppers
{
    public interface ICropper
    {
        public byte[] CropImage(byte[] image, Rectangle area);
    }
}
