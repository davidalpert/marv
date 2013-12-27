using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MarkdownSharp;
using Marv.Xaml;
using PropertyChanged;

namespace Marv
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public string Html { get; set; }
        public DateTime LastWriteTime { get; set; }

        public string PathToSource
        {
            get { return _pathToSource; }
            set
            {
                _pathToSource = Path.GetFullPath(value);
                PathChanged();
            }
        }

        private void PathChanged()
        {
            SourceUpdated();
        }

        public ICommand FileOpenCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        private string _pathToSource;
        private IFileSystem _fileSystem;
        private Markdown _markdownConverter;
        private DispatcherTimer _dispatcherTimer;

        public MainWindowViewModel() : this(new FileSystem())
        {
            PathToSource = @".\welcome.md"; 
        }

        public void OpenFile()
        {
            // Configure open file dialog box 
            var dlg = new Microsoft.Win32.OpenFileDialog
                {
                    InitialDirectory = string.IsNullOrWhiteSpace(PathToSource) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : Path.GetDirectoryName(PathToSource),
                    FileName = string.IsNullOrWhiteSpace(PathToSource) ? "document" : Path.GetFileName(PathToSource),
                    DefaultExt = ".md",
                    Filter = "Markdown documents (.md)|*.md"
                };

            // Show open file dialog box 
            var result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                PathToSource = dlg.FileName;
            }
        }

        public MainWindowViewModel(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _markdownConverter = new Markdown();

            FileOpenCommand = new DelegateCommand(OpenFile);
            ExitCommand = new RelayCommand(CloseWindow);

            InitializeFileWatcher();
        }

        private void CloseWindow(object obj)
        {
            var window = obj as Window;
            if (window != null)
            {
                window.Close();
            }
        }

        private void InitializeFileWatcher()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += OnDispatcherTimerOnTick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void OnDispatcherTimerOnTick(object sender, EventArgs args)
        {
            var lastWriteTime = GetLastWriteTime();

            if (lastWriteTime != LastWriteTime)
            {
                SourceUpdated();
            }
        }

        private void SourceUpdated()
        {
            if (_dispatcherTimer.IsEnabled)
            {
                _dispatcherTimer.Stop();
            }

            LastWriteTime = GetLastWriteTime();

            var md = _fileSystem.File.ReadAllText(PathToSource);
            var html = _markdownConverter.Transform(md);

            Html = html;
            _dispatcherTimer.Start();
        }

        private DateTime GetLastWriteTime()
        {
            var file = _fileSystem.FileInfo.FromFileName(PathToSource);
            var lastWriteTime = file.LastWriteTimeUtc;
            return lastWriteTime;
        }
    }
}
