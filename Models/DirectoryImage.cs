﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace BackgroundWPF.Models
{
    public class DirectoryImage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public DirectoryImage(string directory)
        {
            Directory = directory;
            ImageName = Path.GetFileName(directory);
            ImageNameShortened = shortenName(ImageName);
        }

        private string directory;
        public string Directory
        {
            get { return directory; }
            set { directory = value; OnPropertyChanged(nameof(Directory)); }
        }

        private string imageName;
        public string ImageName
        {
            get { return imageName; }
            set { imageName = value; OnPropertyChanged(nameof(ImageName)); }
        }

        private string imageNameShortened;
        public string ImageNameShortened
        {
            get { return imageNameShortened; }
            set { imageNameShortened = value; OnPropertyChanged(nameof(ImageNameShortened));}
        }

        private string shortenName(string name)
        {
            if (name.Length > 8)
                return name.Substring(0, 8) + "...";
            else
                return name;
        }
    }
}
