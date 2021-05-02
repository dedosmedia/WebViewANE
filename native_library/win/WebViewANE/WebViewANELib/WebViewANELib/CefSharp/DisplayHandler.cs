﻿using System;
using System.Collections.Generic;
using CefSharp;
using CefSharp.Enums;
using CefSharp.Structs;

namespace WebViewANELib.CefSharp {
    public class DisplayHandler : IDisplayHandler {
        public event EventHandler<double> OnLoadingProgressChangeFired;

        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs) { }

        public bool OnAutoResize(IWebBrowser browserControl, IBrowser browser, Size newSize) {
            return false;
        }

        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs) { }

        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls) { }

        public void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen) { }

        public bool OnTooltipChanged(IWebBrowser browserControl, ref string text) {
            return false;
        }

        public void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs) { }

        public bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs) {
            return false;
        }

        public void OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress) {
            OnLoadingProgressChangeFired?.Invoke(chromiumWebBrowser, progress);
        }

        public bool OnCursorChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr cursor, CursorType type, CursorInfo customCursorInfo) {
            return false;
        }
    }
}