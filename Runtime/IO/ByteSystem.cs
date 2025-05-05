using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stereome
{
    public struct ByteSystem
    {

        public static byte[] Serialize<T>(T data, bool prettyPrint = false)
        {
            return Encoding.UTF8.GetBytes(Serialization.Serialize<T>(data, prettyPrint));
        }

        public static T Deserialize<T>(byte[] data)
        {
            return Serialization.Deserialize<T>(Encoding.UTF8.GetString(data));
        }

        public static void WriteAllBytes(byte[] bytes, string path)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static byte[] ObjectToBytes(object obj)
        {
            //구조체 사이즈 
            int iSize = Marshal.SizeOf(obj);
            //사이즈 만큼 메모리 할당 받기
            byte[] arr = new byte[iSize];
            IntPtr ptr = Marshal.AllocHGlobal(iSize);
            //구조체 주소값 가져오기
            Marshal.StructureToPtr(obj, ptr, false);
            //메모리 복사 
            Marshal.Copy(ptr, arr, 0, iSize);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }


        public static async void WriteAllBytesAsync(byte[] bytes, string path)
        {
            try
            {
                await File.WriteAllBytesAsync(path, bytes);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static byte[] ReadAllBytes(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return null;
            }
        }


        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            try
            {
                var result = await File.ReadAllBytesAsync(path);
                return result;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());

                return null;
            }
        }
    }
}