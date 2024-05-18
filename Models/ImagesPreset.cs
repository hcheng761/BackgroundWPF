using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWPF.Models
{
    public class ImagesPreset
    {
        Dictionary<string, DirectoryImage> ImageCollection;
        Dictionary<string, DirectoryImage> AddedImages;
        Dictionary<string, DirectoryImage> RemovedImages;
        string PresetFilePath;
        public ImagesPreset(string path)
        {
            ImageCollection = new Dictionary<string, DirectoryImage>();
            PresetFilePath = path;
        }
    }
}
