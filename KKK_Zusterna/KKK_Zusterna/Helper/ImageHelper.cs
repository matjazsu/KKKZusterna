using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Drawing.Drawing2D;

namespace KKK_Zusterna.Helper
{
    public static class ImageHelper
    {
        #region Functionality

        public static Image ResizeImage(Image slika, Size velikost)
        {
            int newWidth;
            int newHeight;
            
            int originalWidth = velikost.Width;
            int originalHeight = velikost.Height;
            float percentWidth = (float)velikost.Width / (float)originalWidth;
            float percentHeight = (float)velikost.Height / (float)originalHeight;
            float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            newWidth = (int)(originalWidth * percent);
            newHeight = (int)(originalHeight * percent);
            
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(slika, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        #endregion
    }
}