using System;
using System.Globalization;
using UnityEngine;

namespace Stereome
{
    /// <summary>
    /// Log text messages to Application.persistentDataPath\logs\ folder as .txt file.
    /// </summary>
    public struct LogSystem
    {
        /// <summary>
        /// constant folder name for log text files.
        /// </summary>
        public const string Path = "logs/";

        /// <summary>
        /// Full path for log text files. "Application.persistentDataPath + Path"
        /// </summary>
        public static string LogPath
        {
            get
            {
                return string.Format("{0}/{1}/", Application.persistentDataPath, Path);
            }
        }

        /// <summary>
        /// Show string converted from object to Unity console and record to log-text file.
        /// </summary>
        /// <param name="obj"></param>
        public static void Text(object obj)
        {
#if UNITY_EDITOR
            Debug.Log(obj);
#endif
            CreateLogFile();
            FileSystem.WriteLineTextFileAsync(string.Format("{0}log_latest.txt", LogPath),
                        string.Format("{0} >> {1}", DateTime.Now.ToString(new CultureInfo("en-US")), obj));
        }


        /// <summary>
        /// Create new log text file and rename previous log file at yesterday.
        /// </summary>
        private static void CreateLogFile()
        {
            // create directory if it doesn't exist.
            FileSystem.CreateDirectory(LogPath);
            string p1 = string.Format("{0}log_latest.txt", LogPath);
            if (FileSystem.CheckFile(p1))
            {
                DateTime modifiedDate = FileSystem.GetFileLastWriteTime(p1);
                DateTime currentDate = DateTime.Now.Date;

                if (modifiedDate.ToString("d") != currentDate.ToString("d"))
                {
                    // rename yesterday "log_latest" to "log_year_month_day"
                    string p2 = string.Format("{0}log_{1}{2}{3}.txt", LogPath, modifiedDate.Year, modifiedDate.Month, modifiedDate.Day);
                    FileSystem.MoveFile(p1, p2);
                    FileSystem.CreateFile(p1);
                }
            }
            else
            {
                FileSystem.CreateFile(p1);
            }
        }
    }
}