using System;
using System.IO;
using System.Threading.Tasks;

namespace Stereome
{
    /// <summary>
    /// Convert .NET objects to Json string and save to compressed or convert Json string to .NET objects load from decompressed file.
    /// </summary>
    public struct JsonSystem
    {
        public static void Write<T>(string path, T data, bool prettyPrint = false, bool overwrite = true)
        {
            bool exist = FileSystem.CheckFile(path);
            if (!overwrite && exist)
            {
                return;
            }
            if (!exist)
            {
                FileSystem.CreateFile(path);
            }
            try
            {
                ByteSystem.WriteAllBytes(ByteSystem.Serialize(data, prettyPrint), path);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }



        public static T Read<T>(string path)
        {
            if (!FileSystem.CheckFile(path))
            {
                FileSystem.CreateFile(path);
            }
            try
            {
                if (File.Exists(path))
                {
                    var bytes = ByteSystem.ReadAllBytes(path);
                    return ByteSystem.Deserialize<T>(bytes);
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
            return default;
        }


        public static void WriteAsync<T>(string path, T data, bool prettyPrint = false, bool overwrite = true)
        {
            bool exist = FileSystem.CheckFile(path);
            if (!overwrite && exist)
            {
                return;
            }
            if (!exist)
            {
                FileSystem.CreateFile(path);
            }
            try
            {
                ByteSystem.WriteAllBytesAsync(ByteSystem.Serialize(data, prettyPrint), path);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }



        public static async Task<T> ReadAsync<T>(string path) where T : class
        {
            if (!FileSystem.CheckFile(path))
            {
                FileSystem.CreateFile(path);
            }
            try
            {
                if (File.Exists(path))
                {
                    var bytes = await ByteSystem.ReadAllBytesAsync(path);
                    return ByteSystem.Deserialize<T>(bytes);
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
            return null;
        }
    }
}