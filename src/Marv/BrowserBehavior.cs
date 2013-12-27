using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Marv.Xaml
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// source: http://victor.stodell.se/2012/02/binding-html-formatted-string-to-wpf.html
    /// </remarks>
    public class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof(string), typeof(BrowserBehavior), new FrameworkPropertyMetadata(OnHtmlChanged));
 
        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser browser)
        {
            return (string)browser.GetValue(HtmlProperty);
        }
 
        public static void SetHtml(WebBrowser browser, string value)
        {
            browser.SetValue(HtmlProperty, value);
        }
 
        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var browser = dependencyObject as WebBrowser;
            if (browser != null)
            {
                var html = e.NewValue as string ?? string.Empty;
                browser.NavigateToString(html);
            }
        }
    }
}