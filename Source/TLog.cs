using System;
 using System.Collections.Generic;
 using System.Diagnostics;
 using System.Linq;
 using System.Reflection;
 using System.Runtime.CompilerServices;
 using System.Text;
 using UnityEngine;
 
 public class TLog
 {
     public static readonly string TAG_ERROR = "error";
     public static readonly string TAG_WARNING = "warning";
     public static readonly string TAG_INFO = "info";
 
     public static readonly bool doLog = true;
     public static readonly bool doFilter = true;
     public static readonly String[] visibleTags = new String[] { TAG_ERROR, TAG_WARNING, TAG_INFO};
 
     public static readonly bool showParameters = true;
 
 
     private static MethodBase mUnityLog;
 
     static TLog()
     {
         mUnityLog = typeof(UnityEngine.Debug).GetMethod("LogPlayerBuildError", BindingFlags.NonPublic | BindingFlags.Static);
                 
     }
 
 
     public static void Log(string msg, params string[] tags)
     {
         LogArray(msg, tags);
     }
     public static void LogArray(string msg, string[] tags)
     {
         //quit if no log is required
         if (doLog == false)
             return;
         //filter needed? if yes -> check if one of the tags is in our filter list
         if (doFilter && visibleTags.Intersect(tags).Any() == false)
             return;
         if (mUnityLog == null)
         {
             //this happens outside of the editor mode
             //log the normal way and ignore everything that isn't an error
             if (tags != null && tags.Contains(TAG_ERROR))
             {
                 UnityEngine.Debug.LogError(msg);
             }
             return;
         }
 
         StringBuilder message = new StringBuilder();
 
         StackTrace stackTrace = new StackTrace(true);
         StackFrame[] stackFrames = stackTrace.GetFrames();
         int line = 0;
         string file = "";
         int col = 0;
 
         //no tags? just print plain
         if (tags == null || tags.Length == 0)
         {
             message.Append(msg);
         }
         else
         {
             //print tags
             message.Append("<color=green><size=7>");
             for (int k = 0; k < tags.Length; k++)
             {
                 message.Append(tags[k]);
                 if(k + 1 < tags.Length)
                     message.Append("|");
             }
             message.Append("</size></color>");
 
             //print the color tag dependend
             if (tags.Contains(TAG_ERROR))
             {
                 message.Append("<color=red>");
                 message.Append(msg);
                 message.Append("</color>");
             }
             else if (tags.Contains(TAG_WARNING))
             {
                 message.Append("<b><color=yellow>");
                 message.Append(msg);
                 message.Append("</color></b>");
             }
             else
             {
                 message.Append(msg);
             }
         }
         message.Append("\n");
 
         bool foundStart = false;
         //look for the first method call in the stack that isn't from this class.
         //save the first one to jump into it later and add all further lines to the log
         for (int i = 0; i < stackFrames.Length; i++)
         {
             MethodBase mb = stackFrames[i].GetMethod();
             if (foundStart == false && mb.DeclaringType != typeof(TLog))
             {
                 file = FormatFileName(stackFrames[i].GetFileName());
                 line = stackFrames[i].GetFileLineNumber();
                 col = stackFrames[i].GetFileColumnNumber();
                 foundStart = true;
             }
                 
                 
             if(foundStart)
             {
                 message.Append("<color=blue>");
                 message.Append(mb.DeclaringType.FullName);
                 message.Append(":");
                 message.Append(mb.Name);
                 message.Append("(");
                 if (showParameters)
                 {
                     ParameterInfo[] paramters = mb.GetParameters();
                     for (int k = 0; k < paramters.Length; k++)
                     {
                         message.Append(paramters[k].ParameterType.Name);
                         if (k + 1 < paramters.Length)
                             message.Append(", ");
                     }
                 }
                     
                 message.Append(")");
                 message.Append("</color>");
 
 
                 message.Append(" (at ");
                 //the first stack message is found now we add the other stack frames to the log
                 message.Append(FormatFileName(stackFrames[i].GetFileName()));
                 message.Append(":");
                 message.Append(stackFrames[i].GetFileLineNumber());
                 message.Append(")");
                 message.Append("\n");
             }
         }
         mUnityLog.Invoke(null, new object[] { message.ToString(), file, line, col });
     }
 
     private static string FormatFileName(String file)
     {
         //remove everything of the absolute path that is before the Assetfolder
         //using the destination of the Assetfolder to get the right length (not ideal)
         return file.Remove(0, Application.dataPath.Length - "Assets".Length);
     }
 }