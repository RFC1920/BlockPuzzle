using System;
using System.IO;

namespace BlockPuzzle
{
    public static class WorkWithSettings
    {
        /// <summary>
        /// Save setting in file
        /// <param name="depth">depth for save</param>
        /// <param name="width">width for save </param>
        /// <param name="speed">speed for save</param>
        /// </summary>
        public static void SaveSettings(int depth, int width, int speed)
        {
            CreateFileIfItIsNotExist();

            WriteData(depth, width, speed);
        }

        /// <summary>
        /// Create a file for write data if the file isn't exist
        /// </summary>
        private static void CreateFileIfItIsNotExist()
        {
            if (!File.Exists(Constants.PathOfSettingFile))
            {
                File.Create(Constants.PathOfSettingFile).Close();
            }
        }

        /// <summary>
        /// Write data in file
        /// <param name="depthForWrite">depth for write in file</param>
        /// <param name="widthForWrite">width for write in file</param>
        /// <param name="speedForWrite">speed for write in file</param>
        /// </summary>
        private static void WriteData(int depth, int width, int speed)
        {
            using (StreamWriter sw = new StreamWriter(Constants.PathOfSettingFile))
            {
                sw.WriteLine(depth);
                sw.WriteLine(width);
                sw.WriteLine(speed);
            }
        }

        /// <summary>
        /// get saved settings or default
        /// <returns>settings</returns>
        /// </summary>
        public static (int depth, int width, int speed) GetSettings()
        {
            if (!File.Exists(Constants.PathOfSettingFile))
            {
                return SetDefaultSetting();
            }
            else
            {
                return ReadData();
            }
        }


        /// <summary>
        /// Read data from file
        /// <returns>read data converted to integer</returns>
        /// </summary>
        private static (int depth, int width, int speed) ReadData()
        {
            int depth;
            int width;
            int speed;

            using (StreamReader sr = new StreamReader(Constants.PathOfSettingFile))
            {
                depth = Convert.ToInt32(sr.ReadLine());
                width = Convert.ToInt32(sr.ReadLine());
                speed = Convert.ToInt32(sr.ReadLine());
            }

            return (depth, width, speed);
        }

        /// <summary>
        /// set default setting and save it in file
        /// <returns></returns>
        /// </summary>
        private static (int depth, int width, int speed) SetDefaultSetting()
        {
            SaveSettings(Constants.DefaultDepth, Constants.DefaultWidth, Constants.DefaultSpeed);
            return (Constants.DefaultDepth, Constants.DefaultWidth, Constants.DefaultSpeed);
        }
    }
}