using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Omega.Package.Editor
{
    public static class GameObjectScenePath
    {
        // This method invokes as many times as many objects selected on scene
        // As we want to concat all of the paths we've got - we need to get all selected objects at single invocation
        [MenuItem("GameObject/Copy Scene Path", false, int.MinValue)]
        static void CopyGameObjectScenePathToClipboard()
        {
            var selectedGameObjects = Selection.gameObjects;
            var sb = new StringBuilder();
            foreach (var selectedGameObject in selectedGameObjects)
                sb.AppendLine(GetGameObjectScenePath(selectedGameObject));
            GUIUtility.systemCopyBuffer = sb.ToString();
        }

        static string GetGameObjectScenePath(GameObject gameObject)
        {
            string GetPath(Transform transform) => transform.parent
                ? transform.name + '\\' + GetPath(transform.parent)
                : transform.name;

            return GetPath(gameObject.transform);
        }
    }
}