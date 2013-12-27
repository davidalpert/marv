using System;
using System.IO.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Threading;
using MarkdownSharp;
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
                _pathToSource = value;
                PathChanged();
            }
        }

        private void PathChanged()
        {
            SourceUpdated();

        }

        private string _pathToSource;
        private IFileSystem _fileSystem;
        private FileInfoBase _file;
        private Markdown _markdownConverter;
        private DispatcherTimer dispatcherTimer;

        public MainWindowViewModel()
            : this(new FileSystem())
        {
            PathToSource = @".\sample.md"; // for testing
        }

        public MainWindowViewModel(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _markdownConverter = new Markdown();

            InitializeFileWatcher();
        }

        private void InitializeFileWatcher()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += OnDispatcherTimerOnTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
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
            if (dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Stop();
            }

            LastWriteTime = GetLastWriteTime();

            var md = _fileSystem.File.ReadAllText(PathToSource);
            var html = _markdownConverter.Transform(md);

            Html = html;
            dispatcherTimer.Start();
        }

        private DateTime GetLastWriteTime()
        {
            var file = _fileSystem.FileInfo.FromFileName(PathToSource);
            var lastWriteTime = file.LastWriteTimeUtc;
            return lastWriteTime;
        }
    }
}
