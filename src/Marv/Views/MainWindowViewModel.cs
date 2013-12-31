using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MarkdownSharp;
using Marv.Properties;
using Marv.Xaml;
using PropertyChanged;

namespace Marv
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel 
    {
        public Size WindowSize { get; set; }
        public string Html { get; set; }
        public string RawHtml { get; set; }
        public bool ShowRawHtml { get; set; }
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
        private IConfigurableWindow _window;

        public MainWindowViewModel(IConfigurableWindow window, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _markdownConverter = new Markdown();
            _window = window;

            FileOpenCommand = new DelegateCommand(OpenFile);
            ExitCommand = new RelayCommand(CloseWindow);

            InitializeFileWatcher();

            ApplySettings(Settings.Default);
            _window.SizeChanged += (sender, args) => WindowSize = args.NewSize;
            _window.Closing += (sender, args) => SaveSettings();
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

            RawHtml = html;
            Html = InjectScripts(html);

            _dispatcherTimer.Start();
        }

        private string InjectScripts(string html)
        {
            return html + @"
<script
    type=""text/javascript"">
function getVerticalScrollPosition() {
return document.body.scrollTop.toString();
}
function setVerticalScrollPosition(position) {
document.body.scrollTop = position;
}
function getHorizontalScrollPosition() {
return document.body.scrollLeft.toString();
}
function setHorizontalScrollPosition(position) {
document.body.scrollLeft = position;
}
</script>
";
        }

        private DateTime GetLastWriteTime()
        {
            var file = _fileSystem.FileInfo.FromFileName(PathToSource);
            var lastWriteTime = file.LastWriteTimeUtc;
            return lastWriteTime;
        }

        private void SaveSettings()
        {
            var settings = Settings.Default;
            ReadSettings(settings);
            settings.Save();
        }

        private void ReadSettings(Settings settings)
        {
            settings.WindowLocation = new Point(_window.Left, _window.Top);
            settings.WindowSize = WindowSize;
            settings.WindowState = _window.WindowState;
            settings.PathToLastSource = PathToSource;
            settings.ShowRawHtml = ShowRawHtml;
        }

        void ApplySettings(Settings settings)
        {
            Size sz = settings.WindowSize;
            _window.Width = sz.Width;
            _window.Height = sz.Height;

            Point loc = settings.WindowLocation;

            // If the user's machine had two monitors but now only
            // has one, and the Window was previously on the other
            // monitor, we need to move the Window into view.
            bool outOfBounds =
                loc.X < 0 || 
                loc.Y < 0 ||
                loc.X <= -sz.Width ||
                loc.Y <= -sz.Height ||
                SystemParameters.VirtualScreenWidth <= loc.X ||
                SystemParameters.VirtualScreenHeight <= loc.Y;

            if (outOfBounds)
            {
                _window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                _window.WindowStartupLocation = WindowStartupLocation.Manual;

                _window.Left = loc.X;
                _window.Top = loc.Y;

                // We need to wait until the HWND window is initialized before
                // setting the state, to ensure that this works correctly on
                // a multi-monitor system.  Thanks to Andrew Smith for this fix.
                _window.SourceInitialized += delegate
                {
                    _window.WindowState = settings.WindowState;
                };
            }

            ShowRawHtml = settings.ShowRawHtml;
            PathToSource = settings.PathToLastSource;
        }
    }
}
