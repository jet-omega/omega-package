using System;
using System.Collections;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

namespace Omega.Routines.Web
{
    public abstract class WebRequestRoutine<T> : Routine<T>, IProgressRoutineProvider
    {
        public readonly UnityWebRequest WebRequest;
        private Func<DownloadHandler, T> _resultProvider;
        private AsyncOperation _webRequestAsyncOperation;

        public WebRequestRoutine(UnityWebRequest webRequest, Func<DownloadHandler, T> resultProvider)
        {
            WebRequest = webRequest ?? throw new ArgumentNullException(nameof(webRequest));
            _resultProvider = resultProvider ?? throw new ArgumentNullException(nameof(resultProvider));
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return WebRequest.SendWebRequest().GetSelf(out _webRequestAsyncOperation);
            if (WebRequest.isNetworkError || WebRequest.isHttpError)
                throw new HttpRequestException(WebRequestRoutine.GetErrorMessage(WebRequest));

            var downloadHandler = WebRequest.downloadHandler;
            
            var result = downloadHandler != null ? _resultProvider(WebRequest.downloadHandler) : default;
            SetResult(result);
        }

        public float GetProgress()
        {
            return _webRequestAsyncOperation?.progress ?? 0;
        }
    }
}