using Bootstrap.Extensions.StartupTasks;

namespace Marv.StartupTasks
{
    public class ShowMainWindow : IStartupTask
    {
        public void Run()
        {
            var window = new MainWindow();
            var controller = new MainWindowViewModel(window);
            window.DataContext = controller;
            window.Show();
        }

        public void Reset()
        {
        }
    }
}