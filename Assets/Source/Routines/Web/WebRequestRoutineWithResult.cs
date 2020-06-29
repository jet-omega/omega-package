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
            yield return WebRequest.SendWebRequest().Self(out _webRequestAsyncOperation);
            if (WebRequest.isNetworkError || WebRequest.isHttpError)
                throw new HttpRequestException(WebRequestRoutine.GetErrorMessage(WebRequest));

            var downloadHandler = WebRequest.downloadHandler;
            
            var result = downloadHandler != null ? _resultProvider(WebRequest.downloadHandler) : default;
            SetResult(result);
        }

        protected override void OnCancel()
        {
            WebRequest.Abort();
            base.OnCancel();
        }

        public float GetProgress()
        {
            var rawProgress = _webRequestAsyncOperation?.progress ?? 0;
            var normalized = (rawProgress - 0.5f) * 2;
            var clamped = Mathf.Clamp(normalized, 0, 1);
            return clamped;
        }
    }
}
