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
            string path = @"c:\Users\József\Documents\" + fileName + ".txt";
            if (!File.Exists(path))
                File.WriteAllText(path, jsonString);
            else
                MessageBox.Show("The file name is incorrect!");
        }
        public static List<string> GetAllFileFromFolder()
        {
            List<string> list = new List<string>();
            foreach (string s in Directory.GetFiles(@"c:\Users\József\Documents\", "*.txt").Select(Path.GetFileName))
                list.Add(s);
            return list;
        }
        public static List<RobotModel> GetTXTFileToList(string fileName)
        {
            List<RobotModel> list = new List<RobotModel>();
            try
            {
                string content = File.ReadAllText(@"c:\Users\József\Documents\" + fileName + ".txt");
                list = JsonConvert.DeserializeObject<List<RobotModel>>(content);
            }
            catch
            {
                MessageBox.Show("The selected file doesn't exist!");
            }
            return list;
        }
    }
}

