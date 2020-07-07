using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Omega.Package
{
    /// <summary>
    /// Based on UnityEngine.StackTraceUtility
    /// </summary>
    internal static class StackTraceUtility
    {
        private static string _projectFolder = "";

        internal static string ExtractStringFromException(object exception)
        {
            ExtractStringFromException(exception, out var message, out var stackTrace);
            return message + "\n" + stackTrace;
        }

        private static void SetupProjectFolder()
        {
            _projectFolder = Application.dataPath;
        }

        private static void ExtractStringFromException(
            object exceptionObject,
            out string message,
            out string stackTrace)
        {
            if (exceptionObject == null)
                throw new ArgumentException("ExtractStringFromExceptionInternal called with null exception");
            if (!(exceptionObject is Exception exception))
                throw new ArgumentException(
                    "ExtractStringFromExceptionInternal called with an exception that was not of type System.Exception");
            var stringBuilder = new StringBuilder(exception.StackTrace?.Length * 2 ?? 512);
            message = "";
            var str1 = "";
            for (; exception != null; exception = exception.InnerException)
            {
                if(exception.StackTrace is null)
                    break;

                str1 = str1.Length != 0 ? exception.StackTrace + "\n" + str1 : exception.StackTrace;

                var regex = new Regex("at ");
                // ReSharper disable once AssignNullToNotNullAttribute
                foreach (Match match in regex.Matches(str1))
                    str1 = str1.Replace(match.Value, "");

                regex = new Regex("\\S:.*Assets");
                foreach (Match match in regex.Matches(str1))
                    str1 = str1.Replace(match.Value, "(at Assets");

                regex = new Regex(" \\[0x\\S*\\] in ");
                foreach (Match match in regex.Matches(str1))
                    str1 = str1.Replace(match.Value, " ");

                regex = new Regex("cs:\\d*");
                var matches = regex.Matches(str1);
                foreach (Match match in matches)
                    str1 = str1.Replace(match.Value, match.Value + ")");

                str1 = str1.Replace("\\", "/");


                var str2 = exception.GetType().Name;
                var str3 = "";
                if (exception.Message != null)
                    str3 = exception.Message;
                if (str3.Trim().Length != 0)
                    str2 = str2 + ": " + str3;
                message = str2;
                if (exception.InnerException != null)
                    str1 = "Rethrow as " + str2 + "\n" + str1;
            }

            stringBuilder.Append(str1 + "\n");
            var stackTrace1 = new StackTrace(1, true);
            stringBuilder.Append(ExtractFormattedStackTrace(stackTrace1));
            stackTrace = stringBuilder.ToString();
        }

        internal static string ExtractFormattedStackTrace(StackTrace stackTrace)
        {
            SetupProjectFolder();
            var stringBuilder = new StringBuilder(byte.MaxValue);
            for (int index1 = 0; index1 < stackTrace.FrameCount; ++index1)
            {
                var frame = stackTrace.GetFrame(index1);
                var method = frame.GetMethod();
                if (method == null) continue;

                var declaringType = method.DeclaringType;
                if (declaringType == null) continue;

                string str1 = declaringType.Namespace;
                if (!string.IsNullOrEmpty(str1))
                {
                    stringBuilder.Append(str1);
                    stringBuilder.Append(".");
                }

                stringBuilder.Append(declaringType.Name);
                stringBuilder.Append(":");
                stringBuilder.Append(method.Name);
                stringBuilder.Append("(");
                var index2 = 0;
                var parameters = method.GetParameters();
                var flag = true;
                for (; index2 < parameters.Length; ++index2)
                {
                    if (!flag)
                        stringBuilder.Append(", ");
                    else
                        flag = false;
                    stringBuilder.Append(parameters[index2].ParameterType.Name);
                }

                stringBuilder.Append(")");
                var str2 = frame.GetFileName();
                if (str2 != null &&
                    (declaringType.Name != "Debug" || declaringType.Namespace != "UnityEngine") &&
                    (declaringType.Name != "Logger" || declaringType.Namespace != "UnityEngine") &&
                    (declaringType.Name != "DebugLogHandler" ||
                     declaringType.Namespace != "UnityEngine") &&
                    (declaringType.Name != "Assert" || declaringType.Namespace != "UnityEngine.Assertions") &&
                    (method.Name != "print" || declaringType.Name != "MonoBehaviour" ||
                     declaringType.Namespace != "UnityEngine"))
                {
                    stringBuilder.Append(" (at ");
                    if (!string.IsNullOrEmpty(_projectFolder) &&
                        str2.Replace("\\", "/").StartsWith(_projectFolder))
                        str2 = str2.Substring(_projectFolder.Length,
                            str2.Length - _projectFolder.Length);
                    stringBuilder.Append(str2);
                    stringBuilder.Append(":");
                    stringBuilder.Append(frame.GetFileLineNumber().ToString());
                    stringBuilder.Append(")");
                }

                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString().Replace("\\", "/");
        }
    }
}