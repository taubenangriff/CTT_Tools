using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDBSerializing;

namespace CTT_Tools.src
{
    internal class cttFile
    {
        static byte[] MAGIC_BYTES = new byte[] { 0x5F, 0x43, 0x54, 0x54, 0x4D, 0xB8, 0x05, 0x00 };

        public Bitmap Downsampled;
        public Bitmap[] Deltas;

        public cttFile(String Filename)
        {
            using (FileStream fs = File.OpenRead(Filename))
            using (MemoryStream ms = (MemoryStream)zlib.Decompress(fs, 8, fs.Length ))
            {
                File.WriteAllBytes("shit.f", ms.ToArray());
                //decompress stream;
                var filedb = ReadFileDB(ms);
                Downsampled = GetDownsampledMap(filedb);

                Deltas = GetDeltaResolutions(filedb);
            }
        }

        public FileDBDocument_V1 ReadFileDB(Stream s)
        {
            s.Position = 0;
            var Deserializer = new FileDBDeserializer<FileDBDocument_V1>();
            return Deserializer.Deserialize(s);
        }

        public Bitmap[] GetDeltaResolutions(FileDBDocument filedb)
        {
            Tag HeightMap = (Tag)filedb.Roots.ElementAt(0);
            Tag Deltas = (Tag)HeightMap.Children.ElementAt(1);

            var maps = new Bitmap[Deltas.ChildCount];
            for (int i = 0; i < Deltas.ChildCount; i++)
            {
                Tag Data = (Tag)((Tag)Deltas.Children.ElementAt(i)).Children.ElementAt(0);
                Stream s = GetBitmap(Data, out var width, out var height);
                maps[i] = ImageHelper.ToBitmap_Byte(s, width, height);
            }
            return maps;
        }

        public Bitmap GetDownsampledMap(FileDBDocument filedb)
        {
            var s = GetDownsampledBytes(filedb, out var width, out var height);
            return ImageHelper.ToBitmap_Uint16(s, width, height);
        }

        private MemoryStream GetBitmap(Tag BitmapNode, out int width, out int height)
        {
            Attrib att_width = (Attrib)BitmapNode.SelectNodes("width").ElementAt(0);
            Attrib att_height = (Attrib)BitmapNode.SelectNodes("height").ElementAt(0);
            Attrib map = (Attrib)BitmapNode.SelectNodes("map").ElementAt(0);

            width = BitConverter.ToInt32(att_width.Content);
            height = BitConverter.ToInt32(att_height.Content);
            return map.ContentToStream();
        }

        private MemoryStream GetDownsampledBytes(FileDBDocument filedb, out int width, out int height)
        {
            Tag HeightMap = (Tag)filedb.Roots.ElementAt(0);
            Tag Downsampled = (Tag)HeightMap.Children.ElementAt(0);
            return GetBitmap(Downsampled, out width, out height);
        }
    }
}
