using System.Linq;
using System.Collections.Generic;
using PropertyChanged;

namespace Marv
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public string Html { get; set; }

        public MainWindowViewModel()
        {
            var fileSystem = new System.IO.Abstractions.FileSystem();
            string pathToSource = @".\sample.html";
            var html = fileSystem.File.ReadAllText(pathToSource);
            Html = html;
        }
    }
}