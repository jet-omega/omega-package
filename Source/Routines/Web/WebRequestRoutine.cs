using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Omega.Routines.Web
{
    public class WebRequestRoutine : Routine, IProgressRoutineProvider
    {
        public readonly UnityWebRequest WebRequest;
        private AsyncOperation _asyncOperation;

        public WebRequestRoutine(UnityWebRequest webRequest)
        {
            WebRequest = webRequest ?? throw new ArgumentNullException(nameof(webRequest));
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return WebRequest.SendWebRequest().Self(out _asyncOperation);
            if (WebRequest.isNetworkError || WebRequest.isHttpError)
                throw new HttpRequestException(GetErrorMessage(WebRequest));
        }

        private static string GetUploadStringPresent(UploadHandler uploadHandler)
        {
            if (uploadHandler == null)
                return "upload handler in undefined";

            switch (uploadHandler.contentType)
            {
                case "application/json":
                case "text/plain":
                case "text/xml":
                case "text/markdown":
                case "message/http":
                case "message/imdn+xml":
                case "application/xml":
                case "application/xop+xml":
                case "application/xml-dtd":
                    return $"{uploadHandler.contentType}:\n" +
                           $"{Encoding.UTF8.GetString(uploadHandler.data)}";

                default: return $"{uploadHandler.contentType}: <RAW DATA>";
            }
        }

        private static string GetDownloadStringPresent(DownloadHandler downloadHandler)
        {
            return downloadHandler.text;
        }

        internal static string GetErrorMessage(UnityWebRequest webRequest)
        {
            return (webRequest.isNetworkError ? "Networking error" : "Http error") + "\n" +
                   $"Error message: {webRequest.error}\n" +
                   $"Path:{webRequest.url}\n" +
                   $"Upload: {GetUploadStringPresent(webRequest.uploadHandler)}\n" +
                   $"Download: {GetDownloadStringPresent(webRequest.downloadHandler)}";
        }

        protected override void OnCancel()
        {
            WebRequest.Abort();
            base.OnCancel();
        }

        public float GetProgress()
        {
            var rawProgress = _asyncOperation?.progress ?? 0;
            if (!(WebRequest.downloadHandler is DownloadHandlerBuffer)) // https://forum.unity.com/register/genesis?code=_UvNtsaF5RvD9KIFBuquqg009f&state=jCSxJa1buUuJN8frul6z2Lum21q2LyTtebDolLp9%253B%252Fthreads%252Fdownloadhandlerbuffer-data-gc-allocation-problems.535829%252F&locale=en&session_state=fec5146120a63a60fc97523532f4e9b0880e3e23da52161573a50f42654f033e.ycHAZjGDn-JdGw9i8pzNnw00af 
                return rawProgress;
            
            var normalized = (rawProgress - 0.5f) * 2;
            var clamped = Mathf.Clamp01(normalized);
            return clamped;
        }
    }
}
