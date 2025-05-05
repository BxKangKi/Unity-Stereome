using System;
using System.IO;

namespace Stereome
{
    /// <summary>
    /// System.IO.File Utilities
    /// </summary>
    public struct FileSystem
    {
        public static async void WriteLineTextFileAsync(string path, string data)
        {
            try
            {
                if (File.Exists(path))
                {
                    var fi = new FileInfo(path);
                    using (StreamWriter sw = fi.AppendText())
                    {
                        await sw.WriteLineAsync(data);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static bool CheckDirectory(string path)
        {
            try
            {
                DirectoryInfo info = new(path);
                return info.Exists;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return false;
            }
        }


        public static void CreateDirectory(string path)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                if (info.Exists)
                {
                    return;
                }
                info.Create();
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static bool CheckFile(string path)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                return info.Exists;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return false;
            }
        }


        public static void CopyFile(string from, string to)
        {
            try
            {
                FileInfo info = new FileInfo(from);
                if (info.Exists)
                {
                    info.CopyTo(to);
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }

        public static void MoveFile(string from, string to)
        {
            try
            {
                FileInfo info = new FileInfo(from);
                if (info.Exists)
                {
                    File.Move(from, to);
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static void ReplaceFile(string from, string to)
        {
            try
            {
                FileInfo info1 = new FileInfo(from);
                FileInfo info2 = new FileInfo(to);
                if (!info1.Exists)
                {
                    return;
                }
                if (info2.Exists)
                {
                    info1.Replace(from, to);
                }
                else
                {
                    CopyFile(from, to);
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }



        public static void CreateFile(string path)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists)
                {
                    return;
                }
                using (FileStream fs = info.Create())
                {
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static void RemoveFile(string path)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists)
                {
                    info.Delete();
                    LogSystem.Text(StringSystem.Append("File <", path, "> is deleted."));
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static void RemoveDirectory(string path)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                if (info.Exists)
                {
                    info.Delete(true);
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static long GetFileSize(string filePath)
        {
            long fileSize = 0;
            if (File.Exists(filePath))
            {
                FileInfo info = new FileInfo(filePath);
                fileSize = info.Length;
            }
            return fileSize;
        }


        public static string[] GetFileFullNames(string path)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo[] files = info.GetFiles();
                string[] names = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    names[i] = files[i].Name;
                }
                return names;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return null;
            }
        }

        public static int GetFileCounts(string path)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo[] files = info.GetFiles();
                return files.Length;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return 0;
            }
        }


        public static System.DateTime GetFileCreationTime(string path)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists)
                {
                    return info.CreationTime;
                }

            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
            return System.DateTime.Now;
        }



        public static System.DateTime GetFileLastWriteTime(string path)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists)
                {
                    return info.LastWriteTime;
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
            return System.DateTime.Now;
        }



        public static string[] GetFileNames(string path, string extenstion)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo[] files = info.GetFiles();
                string[] names = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    string name = files[i].Name;
                    names[i] = name.Replace(extenstion, "");
                }
                return names;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return null;
            }
        }


        public static string[] GetDirectoryNames(string path)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                DirectoryInfo[] directories = info.GetDirectories();
                string[] names = new string[directories.Length];
                for (int i = 0; i < directories.Length; i++)
                {
                    names[i] = directories[i].Name;
                }
                return names;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return null;
            }
        }


        public static string CheckPathFormat(string path)
        {
            if (!path.EndsWith('/'))
            {
                return StringSystem.Append(path, "/");
            }
            return path;
        }
    }
}