using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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
            CreatePresetFolderCollection();
        }

        public void CreatePresetFolderCollection()
        {
            var PresetFiles = Directory.GetFiles(PresetsDirectory, "*.xml");

            foreach (var presetFile in PresetFiles)
            {
                XDocument doc = XDocument.Load(presetFile);
                var nodes = doc.Root.Elements("Image");

                if (nodes.Count() > 0)
                {
                    List<DirectoryImage> images = new List<DirectoryImage>();
                    foreach (XElement node in nodes)
                    {
                        images.Add(new DirectoryImage(node.Elements("Path").First().Value));
                    }
                    MainPresetCollection.Add(doc.Root.Elements("Name").First().Value, images);
                }
            }
        }

        public List<string> GetPresetNames()
        {
            return new List<string>(MainPresetCollection.Keys);
        }

        public Dictionary<string, List<DirectoryImage>> GetPresetCollection()
        {
            return MainPresetCollection;
        }

        public List<DirectoryImage> GetPresetImagesList(string preset)
        {
            if (preset != null)
                return MainPresetCollection[preset];
            else
                return new List<DirectoryImage>();
        }
    }
}
