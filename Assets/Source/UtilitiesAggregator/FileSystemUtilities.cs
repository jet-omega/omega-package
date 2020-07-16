using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Omega.Package.Internal
{
    public class FileSystemUtilities
    {
        public DirectoryInfo MissingDirectoryPath(string fullPath)
        {
            fullPath = fullPath.Replace('/', '\\');

            if (Directory.Exists(fullPath))
                return new DirectoryInfo(fullPath);

            var root = fullPath.Substring(0, 3);
            var rootRegex = new Regex(@"[A-z]:\\");
            if (!rootRegex.IsMatch(root))
                throw new ArgumentException("Root not found. Path should start with \"x:/\" where 'x' is volume", nameof(fullPath));
                
            if (!Directory.Exists(root))
                throw new ArgumentException($"There is no volume {root} found on this machine");

            var directories = fullPath.Split('\\');

            var currentPath = $@"{directories[0]}\";

            for (int i = 1; i < directories.Length; i++)
            {
                currentPath = Path.Combine(currentPath, directories[i]);
                if (!Directory.Exists(currentPath))
                    Directory.CreateDirectory(currentPath);
            }

            return new DirectoryInfo(currentPath);
        }
    }
}