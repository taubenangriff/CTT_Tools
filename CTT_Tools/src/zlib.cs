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
        internal static MemoryStream Compress(Stream stream, int CompressionLevel)
        {
            return Compress(stream, CompressionLevel, 0, stream.Length);
        }

        internal static MemoryStream Decompress(Stream stream)
        {
            return Decompress(stream, 0, stream.Length);
        }
        /// <summary>
        /// Compresses a stream using zlib.
        /// </summary>
        /// <param name="stream">Original Stream</param>
        /// <param name="CompressionLevel">Compression level for zlib</param>
        /// <returns>zlib-compressed byte array</returns>
        internal static MemoryStream Compress(Stream stream, int CompressionLevel, long StartPosition, long EndPosition)
        {
            var memoryStream = new MemoryStream();

            var streamCopy = new MemoryStream();
            stream.Position = StartPosition;
            stream.CopyTo(streamCopy);
            streamCopy.SetLength(EndPosition - StartPosition);
            streamCopy.Position = 0;

            using (var deflaterStream = new DeflaterOutputStream(streamCopy, new Deflater(CompressionLevel)))
            {
                //write input stream to the deflater stream
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
        public static MemoryStream Decompress(Stream stream, long StartPosition, long EndPosition)
        {
            var decompressedFileStream = new MemoryStream();

            var streamCopy = new MemoryStream();
            stream.Position = StartPosition;
            stream.CopyTo(streamCopy);
            streamCopy.SetLength(EndPosition-StartPosition);
            streamCopy.Position = 0;

            using (var decompressionStream = new InflaterInputStream(streamCopy))
            {
                decompressionStream.CopyTo(decompressedFileStream);
                return decompressedFileStream;
            }
        }
    }
}
