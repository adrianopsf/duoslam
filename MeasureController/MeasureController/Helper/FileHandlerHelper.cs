using MeasureController.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MeasureController.Helper
{
    public static class FileHandlerHelper
    {
        public static void SaveListToTXTFile<T>(List<T> list, string fileName, string filePath)
        {
            string jsonString;
            jsonString = JsonConvert.SerializeObject(list);
            string path = filePath + @"\" + fileName + Helper.Config.FileExtension;
            string path2 = path.Replace(@"\\", @"\");
            try
            {
                File.WriteAllText(path2, jsonString);
            }
            catch
            {
                MessageBox.Show("The file name is incorrect!");
            }
        }

        public static List<Models.FilesModel> GetAllFileFromFolder(string filePath)
        {
            List<Models.FilesModel> list = new List<Models.FilesModel>();
            try
            {
                foreach (string s in Directory.GetFiles(filePath, "*" + Helper.Config.FileExtension).Select(Path.GetFileName))
                {
                    list.Add(new Models.FilesModel { FileName = s, FilePath = filePath });
                }
            }
            catch
            {
                MessageBox.Show("The directory doesn't exist!");
            }

            return list;
        }

        public static List<Measure> GetTXTFileToList(string fileName, string filePath)
        {
            List<Measure> list = new List<Measure>();
            try
            {
                string path = filePath.Replace(@"\\", @"\");
                string content = File.ReadAllText(path + @"\" + fileName);
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