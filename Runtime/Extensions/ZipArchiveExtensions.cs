using System.IO.Compression;

namespace Stereome
{
    public static class ZipArchiveExtensions
    {
        public static ZipArchiveEntry TryGetEntry(this ZipArchive archive, string name)
        {
            var entry = archive.GetEntry(name);
            if (entry == null) entry = archive.CreateEntry(name);
            return entry;
        }
    }
}