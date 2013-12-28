using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Marv
{
    public interface IMainWindow
    {
        double Top { get; set; }
        double Left { get; set; }
        WindowStartupLocation WindowStartupLocation { get; set; }
        WindowState WindowState { get; set; }
        double ActualWidth { get; }
        double ActualHeight { get; }
        double Width { get; set; }
        double Height { get; set; }
        event EventHandler StateChanged;
        event EventHandler LocationChanged;
        event CancelEventHandler Closing;
        event SizeChangedEventHandler SizeChanged;
        event RoutedEventHandler Loaded;
        event EventHandler SourceInitialized;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var controller = new MainWindowViewModel(this);
            this.DataContext = controller;
        }
    }
}
