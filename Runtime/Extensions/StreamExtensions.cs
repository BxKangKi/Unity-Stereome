using System.IO;
using System.Threading.Tasks;

namespace Stereome
{
    public static class StreamExtensions
    {
        public static MemoryStream CopyToMemory(this Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }

        public static async Task<MemoryStream> CopyToMemoryAsync(this Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }
    }
}