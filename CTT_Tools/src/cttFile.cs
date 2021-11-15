using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDBSerializing;

namespace CTT_Tools.src
{
    internal class cttFile
    {
        static byte[] MAGIC_BYTES = new byte[] { 0x5F, 0x43, 0x54, 0x54, 0x4D, 0xB8, 0x05, 0x00 };

        public cttFile(String Filename)
        {
            using (FileStream fs = File.OpenRead(Filename))
            {
                //decompress stream;
                var filedb = ReadFileDB(fs);
            }
        }

        public FileDBDocument_V1 ReadFileDB(Stream s)
        {
            var Deserializer = new FileDBDeserializer<FileDBDocument_V1>();
            return Deserializer.Deserialize(s);
        }

        public MemoryStream GetDownsampledBytes(FileDBDocument filedb, out int width, out int height)
        {
            Tag HeightMap = (Tag)filedb.Roots.ElementAt(0);
            Tag Downsampled = (Tag)HeightMap.Children.ElementAt(0);

            Attrib att_width = (Attrib)Downsampled.Children.ElementAt(0);
            Attrib att_height = (Attrib)Downsampled.Children.ElementAt(1);
            Attrib map = (Attrib)Downsampled.Children.ElementAt(2);

            width = BitConverter.ToInt32(att_width.Content);
            height = BitConverter.ToInt32(att_height.Content);

            return map.ContentToStream();
        }
    }
}
