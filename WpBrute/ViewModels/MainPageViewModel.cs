using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static System.Windows.Forms.FolderBrowserDialog;
using WpBrute.Entities;
using WpBrute.Services;
using System.Linq;
using System;

namespace WpBrute.ViewModels
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        private int maxThreads = 1;

        public int MaxThreads
        {
            get => maxThreads;
            set
            {
                if (value == maxThreads)
                {
                    return;
                }
                maxThreads = value;
                OnPropertyChanged("MaxThreads");
            }
        }
        private ObservableCollection<WpData> data;

        public ObservableCollection<WpData> Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged("Data");
            }
        }

        private RelayCommand getLinks;
        public RelayCommand GetLinks
        {
            get => getLinks ?? (getLinks = new RelayCommand(obj =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string path = openFileDialog.FileName;
                    var reader = new LinkReaderService();
                    Data = reader.getLinks(path);
                }
            }
            ));

            set => getLinks = value;
        }

        private RelayCommand start;
        public RelayCommand Start
        {
            get => start ?? (start = new RelayCommand(obj =>
            {
                try
                {
                    var parser = new WordPressParser();
                    parser.run(MaxThreads, Data);
                } catch (Exception e)
                {
                    ErrorText = e.ToString();
                }
            }
            ));

        }

        private string errorText;
        
        public string ErrorText
        {
            get => errorText;
            set
            {
                errorText = value;
                OnPropertyChanged("ErrorText");
            }
        }
        private string path = "";

        public bool CanSave
        {
            get
            {
                return path.Length > 1;
            }
        }

        private RelayCommand selectFolder;

        public RelayCommand SelectFolder
        {
            get => selectFolder ?? (selectFolder = new RelayCommand(obj =>
                {
                    var picker = new System.Windows.Forms.FolderBrowserDialog();
                    System.Windows.Forms.DialogResult result = picker.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(picker.SelectedPath))
                    {
                        path = picker.SelectedPath;
                        OnPropertyChanged("CanSave");
                    }
                }
            ));
        }

        private RelayCommand saveResult;

        public RelayCommand SaveResult
        {
            get => saveResult ?? (saveResult = new RelayCommand(obj =>
            {
                var writer = new FileWriterService();
                writer.save((from r in data where r.StatusCode == WpData.STATUS_SUCCESS select r).ToList(), path);
            }
            )
        );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


    }
}
