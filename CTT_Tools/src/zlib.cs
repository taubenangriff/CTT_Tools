using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTT_Tools.src
{
    internal class zlib
    {
        internal Stream Compress(Stream stream, int CompressionLevel)
        {
            return Compress(stream, CompressionLevel, 0, stream.Length - 1);
        }

        internal Stream Decompress(Stream stream)
        {
            return Decompress(stream, 0, stream.Length - 1);
        }
        /// <summary>
        /// Compresses a stream using zlib.
        /// </summary>
        /// <param name="stream">Original Stream</param>
        /// <param name="CompressionLevel">Compression level for zlib</param>
        /// <returns>zlib-compressed byte array</returns>
        internal Stream Compress(Stream stream, int CompressionLevel, long StartPosition, long EndPosition)
        {
            var memoryStream = new MemoryStream();
            using (var deflaterStream = new DeflaterOutputStream(memoryStream, new Deflater(CompressionLevel)))
            {
                //write input stream to the deflater stream 
                stream.Position = StartPosition;
                stream.CopyTo(deflaterStream);
                deflaterStream.SetLength(EndPosition - StartPosition);
                return memoryStream;
            }
        }

        /// <summary>
        /// decompresses a zlib file
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>decompressed byte array</returns>
        public Stream Decompress(Stream stream, long StartPosition, long EndPosition)
        {
            var decompressedFileStream = new MemoryStream();
            using (var decompressionStream = new InflaterInputStream(stream))
            
            {
                stream.Position = StartPosition;
                decompressionStream.CopyTo(decompressedFileStream);
                decompressedFileStream.SetLength(EndPosition - StartPosition);
                return decompressedFileStream;
            }
        }
    }
}
