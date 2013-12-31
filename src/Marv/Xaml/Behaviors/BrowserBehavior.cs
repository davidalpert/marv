using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Marv.Xaml.Behaviors
{
    /// <summary>
    /// Adds an Html property that can be set with a string of HTML text
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

        public static readonly DependencyProperty PinToBottomProperty = DependencyProperty.RegisterAttached(
            "PinToBottom", typeof(bool), typeof(BrowserBehavior), new FrameworkPropertyMetadata(OnPinToBottomChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static bool GetPinToBottom(WebBrowser browser)
        {
            return (bool)browser.GetValue(PinToBottomProperty);
        }

        public static void SetPinToBottom(WebBrowser browser, bool value)
        {
            browser.SetValue(PinToBottomProperty, value);
        }
 
        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var browser = dependencyObject as WebBrowser;
            if (browser != null)
            {
                var html = e.NewValue as string ?? string.Empty;

                var pos = GetVerticalScrollPosition(browser);

                SubscribeToNavigatedHandler(browser, pos);
                
                browser.NavigateToString(html);
            }
        }

        private static void OnPinToBottomChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var browser = dependencyObject as WebBrowser;
            if (browser != null)
            {
                ScrollToBottom(browser);
            }
        }

        private static void SubscribeToNavigatedHandler(WebBrowser browser, string pos)
        {
            UnsubscribeNavigatedHandler(browser);

            var callbackHandler = BuildNavigatedHandler(pos);
            browser.LoadCompleted += callbackHandler;
            handlers[browser] = callbackHandler;
        }

        private static void UnsubscribeNavigatedHandler(WebBrowser browser)
        {
            if (handlers.ContainsKey(browser))
            {
                var oldhandler = handlers[browser];
                browser.LoadCompleted -= oldhandler;
                handlers.Remove(browser);
            }
        }

        static readonly Dictionary<WebBrowser, LoadCompletedEventHandler> handlers = new Dictionary<WebBrowser, LoadCompletedEventHandler>();

        private static LoadCompletedEventHandler BuildNavigatedHandler(string originalScrollPosition)
        {
            return (sender, e) =>
                {
                    var browser = sender as WebBrowser;
                    if (browser != null)
                    {
                        if (GetPinToBottom(browser))
                        {
                            ScrollToBottom(browser);
                        }
                        else
                        {
                            SetVerticalScrollPosition(browser, originalScrollPosition);
                        }
                        UnsubscribeNavigatedHandler(browser);
                    }
                };
        }

        private static string GetVerticalScrollPosition(WebBrowser browser)
        {
            try
            {
                return (browser.InvokeScript("getVerticalScrollPosition") ?? "0").ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        private static void SetVerticalScrollPosition(WebBrowser browser, string scrollPosition)
        {
            try
            {
                browser.InvokeScript("setVerticalScrollPosition", scrollPosition ?? "0");
            }
            catch (Exception ex)
            {
            }
        }

        private static void ScrollToBottom(WebBrowser browser)
        {
            try
            {
                browser.InvokeScript("scrollToBottom");
            }
            catch (Exception ex)
            {
            }
        }

    }
}