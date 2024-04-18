using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BackgroundWPF.Models;
using BackgroundWPF.Commands;


namespace BackgroundWPF.ViewModels
{
    public class DirectoryImageViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //SelectedImage = new DirectoryImage();
        }
        #endregion

        DirectoryService directoryService;
        public DirectoryImageViewModel()
        {
            directoryService = new DirectoryService();
            DirectoryImages = new ObservableCollection<DirectoryImage>(directoryService.GetListOfImages());
            changeBGCommand = new RelayCommand(ButtonChangeBackground);
        }

        private DirectoryImage _selectedImage;
        public DirectoryImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                DisplayImagePath = _selectedImage.Directory;
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

        private RelayCommand changeBGCommand;
        public RelayCommand ChangeBGCommand
        {
            get { return changeBGCommand; }
        }

        public void ButtonChangeBackground()
        {
            try
            {
                directoryService.ChangeWindowsBackground(_selectedImage);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
