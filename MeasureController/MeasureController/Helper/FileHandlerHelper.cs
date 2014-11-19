using MeasureController.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MeasureController.Helper
{
    public static class FileHandlerHelper
    {
        public static void SaveListToTXTFile<T>(List<T> list, string fileName)
        {
            string jsonString;
            jsonString = JsonConvert.SerializeObject(list);
            string path = Helper.Config.FilePath + fileName + Helper.Config.FileExtension;
            if (!File.Exists(path))
                File.WriteAllText(path, jsonString);
            else
                MessageBox.Show("The file name is incorrect!");
        }
        public static List<string> GetAllFileFromFolder()
        {
            List<string> list = new List<string>();
            foreach (string s in Directory.GetFiles(Helper.Config.FilePath, "*"+Helper.Config.FileExtension).Select(Path.GetFileName))
                list.Add(s);
            return list;
        }
        public static List<Measure> GetTXTFileToList(string fileName)
        {
            List<Measure> list = new List<Measure>();
            try
            {
                string content = File.ReadAllText(Helper.Config.FilePath + fileName);
                list = JsonConvert.DeserializeObject<List<Measure>>(content);
            }
            catch
            {
                MessageBox.Show("The selected file doesn't exist!");
            }
            return list;
        }
    }
}

