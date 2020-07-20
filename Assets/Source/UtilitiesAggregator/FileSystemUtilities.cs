using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Omega.Package.Internal
{
    public class FileSystemUtilities
    {
        public DirectoryInfo MissingDirectoryFullPath(string fullPath)
        {
            fullPath = fullPath.Replace('/', '\\');

            if (Directory.Exists(fullPath))
                return new DirectoryInfo(fullPath);

            var directories = fullPath.Split('\\');

            var currentPath = directories[0] + '\\';
            for (var i = 1; i < directories.Length; i++)
            {
                currentPath = Path.Combine(currentPath, directories[i]);
                if (!Directory.Exists(currentPath))
                    Directory.CreateDirectory(currentPath);
            }

            return new DirectoryInfo(currentPath);
        }

        public DirectoryInfo MissingDirectoryRelativePath(string relativePath)
        {
            var fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
            return MissingDirectoryFullPath(fullPath);
        }
    }
}