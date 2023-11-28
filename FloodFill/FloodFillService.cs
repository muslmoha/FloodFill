using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace FloodFill
{
    internal class FloodFillService
    {

        const string outPath = "C:\\Users\\admin\\source\\repos\\FloodFill\\FloodFill\\bin\\Debug\\net6.0\\edited\\";

        public void FloodFill(Bitmap bitMap, string fileName)
        {
            Rectangle rec = new Rectangle(0, 0, bitMap.Width, bitMap.Height); //rectangle of the whole image
            System.Drawing.Imaging.BitmapData bmpData = 
                bitMap.LockBits(rec, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitMap.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmpData.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes); //copy array values from ptr to rgb
                                                                                   // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
            {
                Color pixelColour = Color.FromArgb(255, rgbValues[counter-2], rgbValues[counter-1], rgbValues[counter]);
                if(IsShadeOfBlue(pixelColour))
                {
                    Color grey = ConvertToGrayscale(rgbValues[counter - 2], rgbValues[counter - 1], rgbValues[counter]);
                    rgbValues[counter - 2] = grey.B;
                    rgbValues[counter - 1] = grey.G;
                    rgbValues[counter] = grey.R;
                }
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bitMap.UnlockBits(bmpData);

            try
            {
                if (System.IO.File.Exists(outPath + fileName + "_edited.jpg"))
                    System.IO.File.Delete(outPath + fileName + "_edited.jpg");
                bitMap.Save(outPath + fileName + "_edited.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                bitMap.Dispose();
            }
            catch(Exception e) { }
        }

        static bool IsShadeOfBlue(Color color)
        {
            // You can adjust these thresholds based on your specific shade of blue
            /*int redThreshold = 50;
            int greenThreshold = 50;
            int blueThreshold = 200;*/
            int redThreshold = 255;   // Adjust based on your observation
            int greenThreshold = 255; // Adjust based on your observation
            int blueThreshold = 50;  // Adjust based on your observation

            // Check if the color is within the specified thresholds
            return color.R < redThreshold && color.G < greenThreshold && color.B > blueThreshold;
        }

        static Color ConvertToGrayscale(int red, int green, int blue)
        {
            int average = (int)(0.299 * red + 0.587 * green + 0.114 * blue);

            // Ensure that the grayscale values are within the valid range (0 to 255)
            average = Math.Max(0, Math.Min(255, average));

            // Create a grayscale Color object
            return Color.FromArgb(255, average, average, average);
        }

    }
}
