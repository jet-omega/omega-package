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
            if (!(WebRequest.downloadHandler is DownloadHandlerBuffer)) // https://forum.unity.com/register/genesis?code=_UvNtsaF5RvD9KIFBuquqg009f&state=jCSxJa1buUuJN8frul6z2Lum21q2LyTtebDolLp9%253B%252Fthreads%252Fdownloadhandlerbuffer-data-gc-allocation-problems.535829%252F&locale=en&session_state=fec5146120a63a60fc97523532f4e9b0880e3e23da52161573a50f42654f033e.ycHAZjGDn-JdGw9i8pzNnw00af
                return rawProgress;
            
            var normalized = (rawProgress - 0.5f) * 2;
            var clamped = Mathf.Clamp(normalized, 0, 1);
            return clamped;
        }
    }
}
