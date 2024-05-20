using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BackgroundWPF.Models
{
    public class FolderService
    {
        private static Dictionary<string, DirectoryImage> FolderImagesCollection;
        private static string MainImagesFolder;
        private static string PresetsDirectory;

        public FolderService()
        {
            MainImagesFolder = string.Empty;
            //MainImageDirectory = "H:\\Pictures";
            //MainImageDirectory = "C:\\Users\\Owner\\Pictures\\images";
            PresetsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + "BackgroundPresets";
            FolderImagesCollection = new Dictionary<string, DirectoryImage>();
            CreateCollection();
        }

        public Dictionary<string, DirectoryImage> GetImageDirectories()
        {
            return FolderImagesCollection;
        }

        public List<DirectoryImage> GetListOfFolderImages()
        {
            List<DirectoryImage> images = new List<DirectoryImage>(); ;

            if (FolderImagesCollection.Count == 0)
                CreateCollection();
            foreach (KeyValuePair<string, DirectoryImage> e in FolderImagesCollection)
                images.Add(e.Value);
            return images;
        }

        private void CreateCollection()
        {
            if (MainImagesFolder == string.Empty)
            {
                return;
            }

            IEnumerable<string> images = Directory.EnumerateFiles(MainImagesFolder, "*.*", SearchOption.TopDirectoryOnly)
                .Where(input => ImageFileExtensions.Extensions.Any(e => e.Equals(Path.GetExtension(input))));

            foreach (string image in images)
            {
                DirectoryImage di = new DirectoryImage(image);
                FolderImagesCollection.Add(di.ImageName, di);
            }
        }

        public void LoadNewCollection()
        {
            if (MainImagesFolder == string.Empty)
                return;
            if (FolderImagesCollection.Count > 0)
                FolderImagesCollection.Clear();
            CreateCollection();
        }

        public void Add(string path)
        {
            if (Directory.Exists(path))
                FolderImagesCollection.Add(Path.GetFileName(path), new DirectoryImage(path));
        }

        public bool Update(DirectoryImage image)
        {
            bool isUpdated = false;
            return isUpdated;
        }

        public DirectoryImage displayFirstImage()
        {
            if (FolderImagesCollection.Count > 0)
            {
                return FolderImagesCollection.OrderBy(k => k.Key).First().Value;
            }
            else
            {
                return null;
            }
        }

        public void ChangeWindowsBackground(DirectoryImage dImage)
        {
            const int SET_DESKTOP_BACKGROUND = 20;
            const int UPDATE_INI_FILE = 1;
            const int SEND_WINDOWS_INI_CHANGE = 2;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue("WallpaperStyle", 6);
            win32.SystemParametersInfo(SET_DESKTOP_BACKGROUND, 0, dImage.Directory, UPDATE_INI_FILE | SEND_WINDOWS_INI_CHANGE);
        }

        public void ChangeMainDirectory(string directory)
        {
            MainImagesFolder = directory;
            LoadNewCollection();
        }

        public string GetMainDirectory()
        {
            return MainImagesFolder;
        }

        public void CreatePreset(string presetName)
        {
            Directory.CreateDirectory(PresetsDirectory);
            XDocument xdoc = new XDocument(new XElement("Preset", new XElement("Name", presetName)));
            xdoc.Save(PresetsDirectory + "\\" + presetName + DateTime.Today.Millisecond + ".xml");
        }

        public bool CreatePresetFromFolder()
        {
            Directory.CreateDirectory(PresetsDirectory);
            if (MainImagesFolder != string.Empty)
            {
                string dirName = new DirectoryInfo(MainImagesFolder).Name;
                XDocument xdoc = new XDocument(new XElement("Preset", new XElement("Name", dirName)));

                int counter = 0;
                foreach (var k in FolderImagesCollection.Values)
                {
                    xdoc.Element("Preset").Add(new XElement("Image",
                        new XElement("Path", k.Directory), new XElement("ID", k.Identifier))
                        );
                    counter++;
                }

                xdoc.Save(PresetsDirectory + "\\" + dirName + Math.Round(DateTime.Now.Subtract(DateTime.MinValue).TotalMilliseconds) + ".xml");
                return true;
            }
            return false;
        }
    }
}
