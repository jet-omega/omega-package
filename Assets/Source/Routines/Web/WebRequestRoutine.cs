using System;
using System.Collections;
using System.Net.Http;
using UnityEngine.Networking;

namespace Omega.Routines.Web
{
    public class WebRequestRoutine : Routine
    {
        public readonly UnityWebRequest WebRequest;

        public WebRequestRoutine(UnityWebRequest webRequest)
        {
            WebRequest = webRequest ?? throw new ArgumentNullException(nameof(webRequest));
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return WebRequest.SendWebRequest();
            
            if (!string.IsNullOrEmpty(WebRequest.error))
                throw new HttpRequestException(WebRequest.error);
        }
    }
}