using System;
using System.ComponentModel;
using System.Windows;

namespace Marv.Xaml
{
    /// <summary>
    /// Exposes the members of the framework-level <see cref="Window"/> class that are needed
    /// to facilitate saving and restoring window-related preferences (e.g. size, location, etc).
    /// </summary>
    /// <remarks>
    /// derived from: http://joshsmithonwpf.wordpress.com/2007/12/27/a-configurable-window-for-wpf/
    /// </remarks>
    public interface IConfigurableWindow
    {
        double Width { get; set; }
        double Height { get; set; }
        WindowStartupLocation WindowStartupLocation { get; set; }
        double Top { get; set; }
        double Left { get; set; }
        WindowState WindowState { get; set; }

        event CancelEventHandler Closing;
        event SizeChangedEventHandler SizeChanged;
        event EventHandler SourceInitialized;
    }
}