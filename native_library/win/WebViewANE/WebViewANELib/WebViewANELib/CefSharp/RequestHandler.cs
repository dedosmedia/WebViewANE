﻿using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CefSharp;

namespace WebViewANELib.CefSharp {
    public sealed class RequestHandler : IRequestHandler {
        private readonly ArrayList _whiteList;
        private readonly ArrayList _blackList;
        public event EventHandler<string> OnUrlBlockedFired;

        public RequestHandler(ArrayList whiteList, ArrayList blackList) {
            _whiteList = whiteList;
            _blackList = blackList;
        }

        private bool IsWhiteListBlocked(string url) {
            return _whiteList != null && _whiteList.Count != 0 &&
                   !_whiteList.Cast<string>().Any(s => url.ToLower().Contains(s.ToLower()));
        }

        private bool IsBlackListBlocked(string url) {
            return _blackList != null && _blackList.Count != 0 &&
                   _blackList.Cast<string>().Any(s => url.ToLower().Contains(s.ToLower()));
        }

        public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request,
            bool userGesture,
            bool isRedirect) {
            if (!IsWhiteListBlocked(request.Url) && !IsBlackListBlocked(request.Url)) return false;
            OnUrlBlockedFired?.Invoke(chromiumWebBrowser, request.Url);
            return true;
        }

        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture) {
            return OnOpenUrlFromTab(browserControl, browser, frame, targetUrl, targetDisposition, userGesture);
        }

        private bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture) {
            return false;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode,
            string requestUrl, ISslInfo sslInfo, IRequestCallback callback) {
            return false;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath) { }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser bsrowser, IFrame frame,
            IRequest request, IRequestCallback callback) {
            var manager = UrlRequestHeaderManager.GetInstance();
            var userHeaders = manager.Headers;
            if (userHeaders == null) return CefReturnValue.Continue;
            if (userHeaders.Count == 0) return CefReturnValue.Continue;
            try {
                var uri = new Uri(request.Url);
                var host = uri.Host;
                if (!userHeaders.ContainsKey(host)) return CefReturnValue.Continue;
                var domainHeaders = userHeaders[host];
                var headers = request.Headers;
                foreach (var domainHeader in domainHeaders) {
                    headers[domainHeader.Key] = domainHeader.Value;
                }

                request.Headers = headers;
                if (!manager.PersistRequestHeaders) {
                    manager.Remove(host);
                }
            }
            catch {
                // ignored
            }

            return CefReturnValue.Continue;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback) {
            return false;
        }

        bool IRequestHandler.OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy,
            string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback) {
            return false;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser,
            CefTerminationStatus status) { }

        public bool CanGetCookies(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request) {
            return true;
        }

        public bool CanSetCookie(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            Cookie cookie) {
            return true;
        }

        bool IRequestHandler.OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl,
            long newSize, IRequestCallback callback) {
            return false;
        }

        void IRequestHandler.OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request, IResponse response, ref string newUrl) { }

        bool IRequestHandler.OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url) {
            return false;
        }

        void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser) { }

        bool IRequestHandler.OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request, IResponse response) {
            return false;
        }

        IResponseFilter IRequestHandler.GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser,
            IFrame frame, IRequest request, IResponse response) {
            return null;
        }

        void IRequestHandler.OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength) { }
    }
}