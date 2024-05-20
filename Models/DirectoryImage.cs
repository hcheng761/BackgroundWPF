using System;
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

        public DirectoryImage()
        {
            directory = string.Empty;
            imageName = string.Empty;
            imageNameShortened = string.Empty;
            identifier = CreateID();
        }

        public DirectoryImage(string dir)
        {
            directory = dir;
            imageName = Path.GetFileName(dir);
            imageNameShortened = shortenName(ImageName);
            identifier = CreateID();
        }

        public DirectoryImage(DirectoryImage di, string p)
        {
            directory = di.directory;
            imageName = di.imageName;
            imageNameShortened = shortenName(ImageName);
            identifier = di.identifier;
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

        private double identifier;
        public double Identifier
        {
            get { return identifier; }
            set { identifier = value; OnPropertyChanged(nameof(Identifier)); }
        }

        private DateTime dateTimeChange;
        public DateTime DateTimeChange
        {
            get { return dateTimeChange; }
            set { dateTimeChange = value; OnPropertyChanged(nameof(DateTimeChange)); }
        }
        private string shortenName(string name)
        {
            if (name.Length > 8)
                return name.Substring(0, 8) + "...";
            else
                return name;
        }

        private double CreateID()
        {
            //Random rnd = new Random();
            //return rnd.Next(1, Int32.MaxValue);

            return Math.Round(DateTime.Now.Subtract(DateTime.MinValue).TotalMilliseconds);
        }
    }
}
