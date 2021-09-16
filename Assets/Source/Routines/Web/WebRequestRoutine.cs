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
        private UnityWebRequestAsyncOperation _webRequestAsyncOperation;

        public WebRequestRoutine(UnityWebRequest webRequest)
        {
            WebRequest = webRequest ?? throw new ArgumentNullException(nameof(webRequest));
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return WebRequest.SendWebRequest().Self(out _webRequestAsyncOperation);
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
            if (downloadHandler is null)
                return "no download data";

            if (downloadHandler is DownloadHandlerBuffer)
                return downloadHandler.text;

            return $"text representation is not not available for {downloadHandler.GetType().Name}";
        }

        internal static string GetErrorMessage(UnityWebRequest webRequest)
        {
            return (webRequest.isNetworkError ? "Networking error" : "Http error") + "\n" +
                   $"Error message: {webRequest.error}\n" +
                   $"Path:{webRequest.url}\n" +
                   $"Upload: {GetUploadStringPresent(webRequest.uploadHandler)}\n" +
                   $"Download: {GetDownloadStringPresent(webRequest.downloadHandler)}";
        }

        internal static float GetProgressFromWebRequest(UnityWebRequest webRequest,
            UnityWebRequestAsyncOperation asyncOperation)
        {
            var rawProgress = asyncOperation?.progress ?? 0;
            // see https://docs.unity3d.com/ScriptReference/Networking.DownloadHandler.GetProgress.html
            // > DownloadHandler.GetProgress: If not overridden, the default behavior of this callback is to return 0.5.
            if (!(webRequest.downloadHandler is DownloadHandlerBuffer))
                return rawProgress;

            var normalized = (rawProgress - 0.5f) * 2;
            return Mathf.Clamp01(normalized);
        }

        public float GetProgress() => GetProgressFromWebRequest(WebRequest, _webRequestAsyncOperation);
    }
}