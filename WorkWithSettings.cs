using System;
using System.IO;

namespace WinTetris
{
    public static class WorkWithSettings
    {
        /// <summary>
        /// Save setting in file
        /// </summary>
        /// <param name="deep">deep for save</param>
        /// <param name="width">width for save </param>
        /// <param name="speed">speed for save</param>
        public static void SaveSettings(int deep, int width, int speed)
        {
            CreateFileIfItIsNotExist();

            WriteData(deep, width, speed);
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
        /// </summary>
        /// <param name="deepForWrite">deep for write in file</param>
        /// <param name="widthForWrite">width for write in file</param>
        /// <param name="speedForWrite">speed for write in file</param>
        private static void WriteData(int deep, int width, int speed)
        {
            using (StreamWriter sw = new StreamWriter(Constants.PathOfSettingFile))
            {
                sw.WriteLine(deep);
                sw.WriteLine(width);
                sw.WriteLine(speed);
            }
        }

        /// <summary>
        /// get saved settings or default
        /// </summary>
        /// <returns>settings</returns>
        public static (int deep, int width, int speed) GetSettings()
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
        /// </summary>
        /// <returns>read data converted to integer</returns>
        private static (int deep, int width, int speed) ReadData()
        {
            int deep;
            int width;
            int speed;

            using (StreamReader sr = new StreamReader(Constants.PathOfSettingFile))
            {
                deep = Convert.ToInt32(sr.ReadLine());
                width = Convert.ToInt32(sr.ReadLine());
                speed = Convert.ToInt32(sr.ReadLine());
            }

            return (deep, width, speed);
        }

        /// <summary>
        /// set default setting and save it in file
        /// </summary>
        /// <returns></returns>
        private static (int deep, int width, int speed) SetDefaultSetting()
        {
            SaveSettings(Constants.DefaultDeep, Constants.DefaultWidth, Constants.DefaultSpeed);
            return (Constants.DefaultDeep, Constants.DefaultWidth, Constants.DefaultSpeed);
        }
    }
}