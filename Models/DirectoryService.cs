using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWPF.Models
{
    public class DirectoryService
    {
        private static Dictionary<string, DirectoryImage> MainImageCollection;
        private static string MainImageDirectory;

        public DirectoryService()
        {
            MainImageDirectory = "C:\\Users\\Owner\\Pictures\\images";
            MainImageCollection = new Dictionary<string, DirectoryImage>();
            CreateCollection();
        }

        public Dictionary<string, DirectoryImage> GetImageDirectories()
        {
            return MainImageCollection;
        }

        public List<DirectoryImage> GetListOfImages()
        {
            List<DirectoryImage> images = new List<DirectoryImage>(); ;

            foreach (KeyValuePair<string, DirectoryImage> e in MainImageCollection)
                images.Add(e.Value);
            return images;
        }

        private void CreateCollection()
        {
            IEnumerable<string> images = Directory.EnumerateFiles(MainImageDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Where(input => ImageFileExtensions.Extensions.Any(e => e.Equals(Path.GetExtension(input))));

            foreach (string image in images)
            {
                MainImageCollection.Add(Path.GetFileName(image), new DirectoryImage(image));
                System.Diagnostics.Debug.WriteLine(image);
            }
        }

        public void LoadNewCollection()
        {
            IEnumerable<string> images = Directory.EnumerateFiles(MainImageDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Where(input => ImageFileExtensions.Extensions.Any(e => input.EndsWith(e)));
            
            foreach (string image in images)
            {
                MainImageCollection.Add(Path.GetFileName(image), new DirectoryImage(image));
                System.Diagnostics.Debug.WriteLine(image);
            }
        }

        public void Add(string path)
        {
            if (Directory.Exists(path))
                MainImageCollection.Add(Path.GetFileName(path), new DirectoryImage(path));
        }

        public bool Update(DirectoryImage image)
        {
            bool isUpdated = false;
            return isUpdated;
        }

        public DirectoryImage displayFirstImage()
        {
            if (MainImageCollection.Count > 0)
            {
                return MainImageCollection.OrderBy(k => k.Key).First().Value;
            }
            else
            {
                return null;
            }
        }

        public async Task ChangeWindowsBackground(DirectoryImage dImage)
        {
            const int SET_DESKTOP_BACKGROUND = 20;
            const int UPDATE_INI_FILE = 1;
            const int SEND_WINDOWS_INI_CHANGE = 2;

            win32.SystemParametersInfo(SET_DESKTOP_BACKGROUND, 0, dImage.Directory, UPDATE_INI_FILE | SEND_WINDOWS_INI_CHANGE);
        }
    }
}
