using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BackgroundWPF.Models
{
    public class PresetService
    {
        private static Dictionary<string, List<DirectoryImage>> MainPresetCollection;
        private static string MainPreset;
        private static string PresetsDirectory;
        public PresetService() 
        {
            PresetsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + "BackgroundPresets";
            MainPresetCollection = new Dictionary<string, List<DirectoryImage>>();
            CreatePresetCollection();
        }

        public void CreatePresetCollection()
        {
            var PresetFiles = Directory.GetFiles(PresetsDirectory, "*.xml");

            foreach (var presetFile in PresetFiles)
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNodeList path = xmlDoc.GetElementsByTagName("Path");

                MainPresetCollection.Add(path[0].InnerText, new List<DirectoryImage>());
                xmlDoc.Load(presetFile);

            }
        }
    }
}
