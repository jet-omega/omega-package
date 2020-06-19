using System;
using Omega.Routines.Web;
using UnityEngine;
using UnityEngine.Networking;

namespace Omega.Package
{
    public static class UnityWebRequestExtensions
    {
        public static WebRequestRoutine AsRoutine(this UnityWebRequest request)
        {
            if (request is null)
                throw new NullReferenceException(nameof(request));

            return new WebRequestRoutine(request);
        }
    }
}