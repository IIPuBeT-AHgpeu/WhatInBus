using System.Drawing;

namespace WhatInBus
{
    public interface ICropper
    {
        public byte[] CropImage(byte[] image, Rectangle area);
    }
}
