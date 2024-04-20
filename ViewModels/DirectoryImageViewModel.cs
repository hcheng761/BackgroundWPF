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
using System.IO;
using System.Windows.Forms;


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
            DirectoryImages = new ObservableCollection<DirectoryImage>();
            changeBGCommand = new RelayCommand(ButtonChangeBackground);
            loadMainFolderImagesCommand = new RelayCommand(LoadMainDirectoryImages);
            loadFolderCommand = new RelayCommand(LoadFolderModeImages);
            ImagesFolderPath = directoryService.GetMainDirectory();
        }

        private DirectoryImage _selectedImage;
        public DirectoryImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value; OnPropertyChanged(nameof(SelectedImage));
            }
        }

        private string imagesFolderPath;
        public string ImagesFolderPath
        {
            get { return imagesFolderPath; }
            set { imagesFolderPath = value; OnPropertyChanged(nameof(ImagesFolderPath)); }
        }

        private string displayImagePath;
        public string DisplayImagePath
        {
            get { return displayImagePath; }
            set { displayImagePath = value; OnPropertyChanged("DisplayImagePath"); }
        }

        private string selectedImageHour;
        public string SelectedImageHour
        {
            get { return selectedImageHour; }
            set { selectedImageHour = value; OnPropertyChanged(nameof(SelectedImageHour)); }
        }

        private string selectedImageMin;
        public string SelectedImageMin
        {
            get { return selectedImageMin; }
            set { selectedImageMin = value; OnPropertyChanged(nameof(SelectedImageMin)); }
        }

        private string selectedImageSec;
        public string SelectedImageSec
        {
            get { return selectedImageSec; }
            set { selectedImageSec = value; OnPropertyChanged(nameof(SelectedImageSec)); }
        }

        private ObservableCollection<DirectoryImage> directoryImages;
        public ObservableCollection<DirectoryImage> DirectoryImages
        {
            get { return directoryImages; }
            set { directoryImages = value; OnPropertyChanged(nameof(directoryImages)); }
        }

        private bool folderModeChecked;
        public bool FolderModeChecked
        {
            get { return folderModeChecked; }
            set { folderModeChecked = value; OnPropertyChanged(nameof(FolderModeChecked)); }
        }

        private bool presetModeChecked;
        public bool PresetModeChecked
        {
            get { return presetModeChecked; }
            set { presetModeChecked = value; OnPropertyChanged(nameof(PresetModeChecked)); }
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
            catch
            {
                throw new Exception();
            }
        }

        private RelayCommand loadMainFolderImagesCommand;
        public RelayCommand LoadMainFolderImagesCommand
        {
            get { return loadMainFolderImagesCommand; }
        }

        public void LoadMainDirectoryImages()
        {
            try
            {
                string SelectedPath = ImagesFolderPath;

                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        SelectedPath = fbd.SelectedPath;
                    }
                }
                if (SelectedPath != string.Empty)
                {
                    directoryService.ChangeMainDirectory(SelectedPath);
                    DirectoryImages = new ObservableCollection<DirectoryImage>(directoryService.GetListOfFolderImages());
                    ImagesFolderPath = directoryService.GetMainDirectory();
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        private RelayCommand loadFolderCommand;
        public RelayCommand LoadFolderCommand
        {
            get { return loadFolderCommand; }
        }
        public void LoadFolderModeImages()
        {
            DirectoryImages = new ObservableCollection<DirectoryImage>(directoryService.GetListOfFolderImages());
            ImagesFolderPath = directoryService.GetMainDirectory();
        }
    }
}
