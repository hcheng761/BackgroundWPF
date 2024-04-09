using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgroundWPF.Models;


namespace BackgroundWPF.ViewModels
{
    public class DirectoryImageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private DirectoryImage _displayedImage;
        public DirectoryImage DisplayedImage
        {
            get { return _displayedImage; }
            set 
            {   _displayedImage = value;
                DisplayImagePath = _displayedImage.Directory;
                OnPropertyChanged("DisplayedImage"); 
            }
        }

        private string displayImagePath;
        public string DisplayImagePath
        {
            get { return displayImagePath; }
            set 
            { 
                displayImagePath = value; 
                OnPropertyChanged("DisplayImagePath"); 
            }
        }

        DirectoryService directoryService;
        public DirectoryImageViewModel()
        {
            directoryService = new DirectoryService();
            DirectoryImages = new ObservableCollection<DirectoryImage>(directoryService.GetListOfImages());
            DisplayedImage = directoryService.displayFirstImage();
        }

        private ObservableCollection<DirectoryImage> directoryImages;
        public ObservableCollection<DirectoryImage> DirectoryImages
        {
            get { return directoryImages; }
            set
            {
                directoryImages = value;
                OnPropertyChanged(nameof(directoryImages));
            }
        }
    }
}
