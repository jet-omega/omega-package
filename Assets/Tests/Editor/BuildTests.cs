using System;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Package.Tests
{
    public class BuildTests
    {
        private const string PathToTestBuildFolder = "Build";

        [Test]
        public void BuildTest()
        {
            var version = Application.version;
            var unityVersion = Application.unityVersion;
            var pathToExecutable =
                Path.Combine(PathToTestBuildFolder, $"{version}+unity-{unityVersion}+unit-test-build.not-extension");

            var buildTarget = EditorUserBuildSettings.activeBuildTarget;

            Assert.AreNotEqual(buildTarget, BuildTarget.NoTarget);

            if (Directory.Exists(PathToTestBuildFolder))
            {
                foreach (var file in Directory.EnumerateFiles(PathToTestBuildFolder))
                    File.Delete(file);
                foreach (var directory in Directory.EnumerateDirectories(PathToTestBuildFolder))
                    Directory.Delete(directory, true);
            }
            else Directory.CreateDirectory(PathToTestBuildFolder);

            LogAssert.ignoreFailingMessages = true;
            var buildReport = BuildPipeline.BuildPlayer(Array.Empty<string>(), pathToExecutable,
                buildTarget, BuildOptions.None);
            LogAssert.ignoreFailingMessages = false;

            var summary = buildReport.summary;
            Assert.Zero(summary.totalErrors, $"Build were failed with {summary.totalErrors} errors");
            Assert.Zero(summary.totalWarnings, $"Build were complete with {summary.totalWarnings} warnings");
        }
    }
}