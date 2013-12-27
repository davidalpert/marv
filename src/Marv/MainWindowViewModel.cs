using System;
using System.IO.Abstractions;
using System.Linq;
using System.Collections.Generic;
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
                PathChangedTo(_pathToSource);
            }
        }

        private string _pathToSource;
        private IFileSystem _fileSystem;
        private FileInfoBase _file;
        private Markdown _markdownConverter;

        public MainWindowViewModel() : this(new FileSystem())
        {
        }

        public MainWindowViewModel(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;

            PathToSource = @".\sample.md";
        }

        private void PathChangedTo(string pathToSource)
        {
            _file = _fileSystem.FileInfo.FromFileName(PathToSource);
            _markdownConverter = new Markdown();

            LastWriteTime = _file.LastWriteTimeUtc;

            var md = _fileSystem.File.ReadAllText(PathToSource);
            var html = _markdownConverter.Transform(md);
            Html = html;
        }
    }
}
