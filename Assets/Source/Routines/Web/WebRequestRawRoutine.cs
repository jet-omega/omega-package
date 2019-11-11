using System;
using UnityEngine.Networking;

namespace Omega.Routines.Web
{
    public class WebRequestRawRoutine : WebRequestRoutine<byte[]>
    {
        public WebRequestRawRoutine(UnityWebRequest webRequest)
            : base(webRequest, BytesFromDownloadHandlerProvider)
        {
        }

        private static byte[] BytesFromDownloadHandlerProvider(DownloadHandler handler)
            => handler?.data ?? Array.Empty<byte>();
    }
}