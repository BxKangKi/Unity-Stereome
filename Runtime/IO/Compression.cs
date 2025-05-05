using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Stereome
{
    // Reference: https://slaner.tistory.com/83
    /// <summary>
    /// Compression Utility.
    /// </summary>
    public readonly struct Compression
    {
        public static byte[] Compress(byte[] source)
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionLevel.Fastest))
                {
                    ds.Write(source, 0, source.Length);
                }
                result = ms.ToArray();
            }
            return result;
        }


        public static byte[] Decompress(byte[] source)
        {
            MemoryStream resultStream = new MemoryStream();
            using (MemoryStream ms = new MemoryStream(source))
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    ds.CopyTo(resultStream);
                    ds.Close();
                }
            }
            byte[] result = resultStream.ToArray();
            resultStream.Dispose();
            return result;
        }


        public static DeflateStream Compress(Stream stream)
        {
            return new DeflateStream(stream, CompressionLevel.Fastest);
        }


        public static MemoryStream Decompress(Stream stream)
        {
            MemoryStream result = new MemoryStream();
            // MemoryStream으로 파일 스트림의 내용을 복사
            using (MemoryStream ms = new MemoryStream())
            {
                // 스트림을 메모리 스트림으로 복사
                stream.CopyTo(ms);
                // 메모리 스트림의 위치를 처음으로 되돌림 (데이터 읽기 위해)
                ms.Position = 0;
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    ds.CopyTo(result);
                    ds.Close();
                }
            }
            result.Position = 0;
            return result;
        }


        public static async Task<MemoryStream> DecompressAsync(Stream stream)
        {
            MemoryStream result = new MemoryStream();
            // MemoryStream으로 파일 스트림의 내용을 복사
            using (MemoryStream ms = new MemoryStream())
            {
                // 파일 스트림을 메모리 스트림으로 복사
                await stream.CopyToAsync(ms);
                // 메모리 스트림의 위치를 처음으로 되돌림 (데이터 읽기 위해)
                ms.Position = 0;
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    await ds.CopyToAsync(result);
                    ds.Close();
                }
            }
            result.Position = 0;
            return result;
        }


        public static async Task<byte[]> CompressAsync(byte[] source)
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionLevel.Fastest))
                {
                    await ds.WriteAsync(source, 0, source.Length);
                }
                result = ms.ToArray();
            }
            return result;
        }


        public static async Task<byte[]> DecompressAsync(byte[] source)
        {
            MemoryStream resultStream = new MemoryStream();
            using (MemoryStream ms = new MemoryStream(source))
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    await ds.CopyToAsync(resultStream);
                    ds.Close();
                }
            }
            byte[] result = resultStream.ToArray();
            resultStream.Dispose();
            return result;
        }
    }
}