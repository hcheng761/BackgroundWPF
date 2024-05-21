using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BackgroundWPF.Models
{
    public class ImagesPreset : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        Dictionary<string, DirectoryImage> ImageCollection; //Keys are file names
        Dictionary<string, DirectoryImage> AddedImages;
        Dictionary<string, DirectoryImage> RemovedImages;

        public ImagesPreset()
        {
            ImageCollection = new Dictionary<string, DirectoryImage>();
        }
        public ImagesPreset(string path) : this()
        {
            ImageCollection = new Dictionary<string, DirectoryImage>();
            PresetFilePath = path;
            CreateCollectionFromFile();
        }

        public ImagesPreset(string name, List<DirectoryImage> list) : this()
        {
            PresetName = name;
            foreach (DirectoryImage d in list)
            {
                ImageCollection.Add(d.ImageName, d);
            }
        }

        private string presetFilePath;
        public string PresetFilePath
        {
            get { return presetFilePath; }
            set {  presetFilePath = value; OnPropertyChanged(nameof(PresetFilePath)); }
        }

        private string presetName;
        public string PresetName
        {
            get { return presetName; }
            set { presetName = value; OnPropertyChanged(nameof(PresetName)); }
        }

        public void CreateCollectionFromFile()
        {
            XDocument doc = XDocument.Load(PresetFilePath);

            PresetName = doc.Root.Elements("Name").First().Value;
            var nodes = doc.Root.Elements("Image");

            if (nodes.Count() > 0)
            {
                string path = nodes.Elements("Path").First().Value;
                if (File.Exists(path))
                {
                    foreach (var node in nodes)
                    {
                        string imagePath = node.Elements("Path").First().Value;
                        int id = Convert.ToInt32(node.Elements("ID").First().Value);
                        ImageCollection.Add(Path.GetFileName(imagePath), new DirectoryImage(imagePath, id));
                    }
                }
            }
        }

        public string GetPresetName()
        {
            return PresetName;
        }

        public string GetPresetPath()
        {
            return PresetFilePath;
        }

        public DirectoryImage GetImage(string name)
        {
            return ImageCollection[name];
        }

        public List<string> GetImageNames()
        {
            return ImageCollection.Keys.ToList();
        }

        public List<DirectoryImage>GetDirectoryImages()
        {
            return ImageCollection.Values.ToList();
        }

        public void AddImage(DirectoryImage di)
        {
            if (File.Exists(di.Directory))
            {
                AddedImages.Add(di.ImageName, di);
                ImageCollection.Add(di.ImageName, di);
            }
        }

        public void RemoveImage(DirectoryImage di)
        {
            if (File.Exists(di.Directory))
            {
                RemovedImages.Add(di.ImageName, di);
                ImageCollection.Remove(di.ImageName);
            }
        }

        public void CreatePresetFile(string path)
        {

        }

        public void SaveChangesToFile(string PresetsDirectory)
        {
            XDocument doc = XDocument.Load(PresetFilePath);
            Directory.CreateDirectory(PresetsDirectory);

                XDocument xdoc = new XDocument(new XElement("Preset", new XElement("Name", PresetName)));

                int counter = 0;
                foreach (var k in ImageCollection.Keys)
                {
                    xdoc.Element("Preset").Add(new XElement("Image",
                        new XElement("Path", ImageCollection[k].Directory)),
                        new XElement("ID", ImageCollection[k].Identifier));
                    counter++;
                }

                xdoc.Save(PresetsDirectory + "\\" + PresetName + Math.Round(DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds) + ".xml");
        }

    }
}
