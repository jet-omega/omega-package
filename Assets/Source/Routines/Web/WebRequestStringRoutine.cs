using System;
using System.Collections;
using UnityEngine.Networking;

namespace Omega.Routines.Web
{
    public sealed class WebRequestStringRoutine : WebRequestRoutine<string>
    {
        public WebRequestStringRoutine(UnityWebRequest webRequest) 
            : base(webRequest, StringFromDownloadHandlerProvider)
        {
        }

        private static string StringFromDownloadHandlerProvider(DownloadHandler downloadHandler)
            => downloadHandler?.text ?? string.Empty;
    }
}