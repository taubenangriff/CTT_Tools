using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CTT_Tools.src
{
    internal class ImageHelper
    {
        internal delegate byte ReadMethod(ref BinaryReader reader);

        internal static Bitmap ToBitmap_Uint16(Stream s, int width, int height)
        {
            return ToBitmap(s, width, height, Read_Uint16, 2);
        }
        internal static Bitmap ToBitmap_Byte(Stream s, int width, int height)
        {
            return ToBitmap(s, width, height, Read_Byte, 1);
        }

        internal static Bitmap ToBitmap(Stream s, int width, int height, ReadMethod read, int bpp)
        { 
            Bitmap map = new Bitmap(height, width, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (s.Length < width * height * bpp) throw new ArgumentException("the length of the stream must at least be as large as width * height of your image!");
            else if (s.Length > width * height * bpp) Console.WriteLine("Too large stream. excess Bytes are ignored.");

            s.Position = 0;
            BinaryReader reader = new BinaryReader(s);
            while (reader.BaseStream.Position < reader.BaseStream.Length && reader.BaseStream.Position < width * height * bpp)
            {
                int x = (int)s.Position / bpp % height;
                int y = (int)s.Position / (width * bpp);
                byte ColorValue = read(ref reader);
                    
                map.SetPixel(x, y, Color.FromArgb(255, ColorValue, ColorValue, ColorValue));
            }
            reader.Dispose();
                
            return map;
        }

        internal static Bitmap ToBitmap(byte[] bytes, int width, int height, ReadMethod read, int bpp)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return ToBitmap(ms, width, height, read, bpp);
            }
        }

        internal static byte Read_Uint16(ref BinaryReader reader)
        {
            return (byte)reader.ReadUInt16();
        }

        internal static byte Read_Byte(ref BinaryReader reader)
        {
            return reader.ReadByte();
        }
    }
}
