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
        private static Dictionary<string, ImagesPreset> MainPresetCollection;

        private static string MainPreset;
        private static string PresetsDirectory;

        public PresetService() 
        {
            PresetsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + "BackgroundPresets";
            MainPresetCollection = new Dictionary<string, ImagesPreset>();
            CreatePresetFolderCollection();
        }

        public void CreatePresetFolderCollection()
        {
            var PresetFiles = Directory.GetFiles(PresetsDirectory, "*.xml");

            foreach (var presetFile in PresetFiles)
            {
                ImagesPreset preset = new ImagesPreset(presetFile);
                MainPresetCollection.Add(preset.GetPresetName(), preset);
            }
        }

        public List<string> GetPresetNames()
        {
            return new List<string>(MainPresetCollection.Keys);
        }

        public Dictionary<string, ImagesPreset> GetMainPresetCollection()
        {
            return MainPresetCollection;
        }

        public ImagesPreset GetPreset(string preset)
        {
            return MainPresetCollection[preset];
        }

        public void AddImage(string preset, DirectoryImage path)
        {
            MainPresetCollection[preset].AddImage(path);
        }

        public void RemoveImage(ImagesPreset preset, DirectoryImage di)
        {
            preset.RemoveImage(di);
        }

        public void AddPreset(ImagesPreset preset)
        {
            MainPresetCollection.Add(preset.GetPresetName(), preset);
        }
    }
}
