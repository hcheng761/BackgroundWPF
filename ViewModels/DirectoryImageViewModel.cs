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

        FolderService folderService;
        PresetService presetService;
        public DirectoryImageViewModel()
        {
            folderService = new FolderService();
            presetService = new PresetService();
            DirectoryImages = new ObservableCollection<DirectoryImage>();
            changeBGCommand = new RelayCommand(ButtonChangeBackground);
            loadMainFolderImagesCommand = new RelayCommand(LoadMainDirectoryImages);
            loadFolderCommand = new RelayCommand(LoadFolderModeImages);
            loadPresetsCommand = new RelayCommand(LoadPresetBox);
            ImagesFolderPath = folderService.GetMainDirectory();
            createFolderPresetCommand = new RelayCommand(CreateFolderPresetFile);
            selectionChangedCommand = new RelayCommand(ComboSelectionChangedCommand);
            addImageToPresetCommand = new RelayCommand(AddImageToPresetCollection);
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

        private string secondMinValue;
        public string SecondMinValue
        {
            get { return secondMinValue; }
            set { secondMinValue = value; OnPropertyChanged(nameof(SecondMinValue));}
        }

        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set { statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        private List<ImagesPreset> comboBoxItems;
        public List<ImagesPreset> ComboBoxItems
        {
            get { return comboBoxItems; }
            set { comboBoxItems = value; OnPropertyChanged(nameof(ComboBoxItems)); }
        }

        private ImagesPreset selectedPreset;
        public ImagesPreset SelectedPreset
        {
            get { return selectedPreset; }
            set 
            { 
                    selectedPreset = value; 
                    OnPropertyChanged(nameof(SelectedPreset)); 
                    ComboSelectionChangedCommand(); 
            }
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

        private RelayCommand loadPresetsCommand;
        public RelayCommand LoadPresetsCommand
        {
            get { return loadPresetsCommand; }
        }

        public void LoadPresetBox()
        {
            ComboBoxItems = presetService.GetMainPresetCollection().Values.ToList();
            LoadPresetDirectoryImages();
        }

        public void LoadPresetDirectoryImages()
        {
            if (SelectedPreset != null)
            {
                ImagesPreset p = presetService.GetPreset(SelectedPreset.PresetName);
                DirectoryImages = new ObservableCollection<DirectoryImage>(p.GetDirectoryImages());
            }
        }

        private RelayCommand selectionChangedCommand;
        public RelayCommand SelectionChangedCommand
        {
            get { return selectionChangedCommand; } 
        }

        public void ComboSelectionChangedCommand()
        {
            if (PresetModeChecked)
                LoadPresetDirectoryImages();
            //else if (FolderModeChecked)
            //    LoadFolderModeImages();
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
                folderService.ChangeWindowsBackground(_selectedImage);
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
                    folderService.ChangeMainDirectory(SelectedPath);
                    DirectoryImages = new ObservableCollection<DirectoryImage>(folderService.GetListOfFolderImages());
                    ImagesFolderPath = folderService.GetMainDirectory();
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
            DirectoryImages = new ObservableCollection<DirectoryImage>(folderService.GetListOfFolderImages());
            ImagesFolderPath = folderService.GetMainDirectory();
        }

        private RelayCommand createFolderPresetCommand;
        public RelayCommand CreateFolderPresetCommand
        {
            get
            {
                return createFolderPresetCommand;
            }
        }

        public void CreateFolderPresetFile()
        {
            if (ImagesFolderPath != string.Empty && DirectoryImages.Count > 0)
            {
                string path = folderService.GetMainDirectory();
                ImagesPreset newPreset = new ImagesPreset(folderService.GetMainDirectory(), folderService.GetListOfFolderImages());

                presetService.AddPreset(newPreset);
                folderService.CreatePresetFromFolder();
                ChangeStatusText();
            }

        }

        public async void ChangeStatusText()
        {
            StatusText = "Preset created from selected folder.";
            await Task.Delay(5000);
            StatusText = string.Empty;
        }

        private RelayCommand addImageToPresetCommand;
        public RelayCommand AddImageToPresetCommand
        {
            get
            {
                return addImageToPresetCommand;
            }
        }

        public void AddImageToPresetCollection()
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.PNG)|*.BMP;*.bmp;*.JPG;*.jpg;*.JPEG;*.jpeg;*.PNG*;*.png";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            if (filePath != string.Empty)
            {
                DirectoryImage di = new DirectoryImage(filePath);
                DirectoryImages.Add(di);
                presetService.AddImage(SelectedPreset.PresetName, di);
            }
        }
    }
}
