using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CTT_Tools.src
{
    internal class ImageManager
    {
        internal Bitmap ToBitmap(Stream s, int width, int height)
        { 
            Bitmap map = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            if (s.Length < width * height) throw new ArgumentException("the length of the stream must at least be as large as width * height of your image!");
            else if (s.Length > width * height) Console.WriteLine("Too large stream. excess Bytes are ignored.");

            s.Position = 0; 
            while (s.Position < s.Length)
            {
                byte ColorValue = (byte)s.ReadByte();
                map.SetPixel((int)s.Position / width, (int)s.Position % height, Color.FromArgb(255, ColorValue, ColorValue, ColorValue)); 
            }
            return map;
        }
    }
}
