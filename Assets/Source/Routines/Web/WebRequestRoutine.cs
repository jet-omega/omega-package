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
            yield return WebRequest.SendWebRequest().GetSelf(out _asyncOperation);
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

        public float GetProgress()
        {
            return _asyncOperation?.progress ?? 0;
        }
    }
}