using System.IO.Abstractions;
using Bootstrap.Extensions.StartupTasks;
using Common.Logging;

namespace Marv.StartupTasks
{
    public class ShowMainWindow : IStartupTask
    {
        public void Run()
        {
            var window = new MainWindow();
            var windowLogger = LogManager.GetLogger<MainWindowViewModel>();
            window.DataContext = new MainWindowViewModel(windowLogger, window, new FileSystem());
            window.Show();
        }

        public void Reset()
        {
        }
    }
}