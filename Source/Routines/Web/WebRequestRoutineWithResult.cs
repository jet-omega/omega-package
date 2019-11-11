using System;
using System.Collections;
using System.Net.Http;
using UnityEngine.Networking;

namespace Omega.Routines.Web
{
    public abstract class WebRequestRoutine<T> : Routine<T>
    {
        public readonly UnityWebRequest WebRequest;
        private Func<DownloadHandler, T> _resultProvider;

        public WebRequestRoutine(UnityWebRequest webRequest, Func<DownloadHandler, T> resultProvider)
        {
            WebRequest = webRequest ?? throw new ArgumentNullException(nameof(webRequest));
            _resultProvider = resultProvider ?? throw new ArgumentNullException(nameof(resultProvider));
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return WebRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(WebRequest.error))
                throw new HttpRequestException(WebRequest.error);

            var result = _resultProvider(WebRequest.downloadHandler);
            SetResult(result);
        }
    }
}