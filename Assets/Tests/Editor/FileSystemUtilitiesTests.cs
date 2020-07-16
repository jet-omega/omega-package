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
        public void CorrectPathShouldReturnDirectoryInfo()
        {
            var path = Application.dataPath;
            var info = Utilities.FileSystem.MissingDirectoryPath(path);
            Assert.AreEqual("Assets", info.Name);
        }

        [Test]
        public void UnknownVolumeShouldThrowException()
        {
            var driveInfos = DriveInfo.GetDrives();
            var unknownVolumeLetterCode = (int) 'A';
            while (driveInfos.Any(t => t.Name[0] == (char) unknownVolumeLetterCode))
                unknownVolumeLetterCode++;
            var path = (char) unknownVolumeLetterCode + @":\";
            Assert.Catch<ArgumentException>(() => Utilities.FileSystem.MissingDirectoryPath(path));
        }

        [Test]
        public void PartialPathShouldReturnNewDirectoryInfo()
        {
            var directoryName = DateTime.Now.Ticks.ToString();
            var path = Path.Combine(Application.dataPath, directoryName);
            var info = Utilities.FileSystem.MissingDirectoryPath(path);
            Assert.AreEqual(directoryName, info.Name);
            Directory.Delete(path);
        }
    }
}