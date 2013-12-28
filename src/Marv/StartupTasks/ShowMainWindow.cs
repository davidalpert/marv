using System.IO.Abstractions;
using Bootstrap.Extensions.StartupTasks;

namespace Marv.StartupTasks
{
    public class ShowMainWindow : IStartupTask
    {
        public void Run()
        {
            var window = new MainWindow();
            window.DataContext = new MainWindowViewModel(window, new FileSystem());
            window.Show();
        }

        public void Reset()
        {
        }
    }
}