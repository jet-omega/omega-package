using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Tests
{
    public class FileSystemUtilitiesTests
    {
        [Test]
        public void CorrectFullPathShouldReturnDirectoryInfo()
        {
            var path = Application.dataPath;
            var info = Utilities.FileSystem.MissingDirectoryFullPath(path);
            Assert.AreEqual("Assets", info.Name);
        }

        [Test]
        public void PartialFullPathShouldReturnNewDirectoryInfo()
        {
            var directoryName = DateTime.Now.Ticks.ToString();
            var path = Path.Combine(Application.dataPath, directoryName);
            var info = Utilities.FileSystem.MissingDirectoryFullPath(path);
            Assert.AreEqual(directoryName, info.Name);
            Directory.Delete(path);
        }
        
        [Test]
        public void CorrectRelativePathShouldReturnDirectoryInfo()
        {
            var path = "Assets";
            var info = Utilities.FileSystem.MissingDirectoryRelativePath(path);
            Assert.AreEqual("Assets", info.Name);
        }

        [Test]
        public void PartialRelativePathShouldReturnNewDirectoryInfo()
        {
            var path = DateTime.Now.Ticks.ToString();
            var info = Utilities.FileSystem.MissingDirectoryRelativePath(path);
            Assert.AreEqual(path, info.Name);
            Directory.Delete(Path.Combine(Environment.CurrentDirectory, path));
        }
    }
}