using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine.TestTools;
using Random = System.Random;

namespace Omega.Routines.IO.Tests
{
    public class FileTests
    {
        [UnityTest]
        public IEnumerator WriteAllBytesShouldCreateFileTest()
        {
            var testData = new byte[1024];
            
            var pathToTestFile = $"~{nameof(WriteAllBytesShouldCreateFileTest)}";
            var random = new Random();
            random.NextBytes(testData);

            yield return FileRoutine.WriteAllBytesRoutine(pathToTestFile, testData)
                .GetSelf(out var writeAllBytesRoutine);

            if (writeAllBytesRoutine.IsError)
            {
                File.Delete(pathToTestFile);
                Assert.Fail();
            }

            var afterWriteData = File.ReadAllBytes(pathToTestFile);

            File.Delete(pathToTestFile);
            
            Assert.AreEqual(afterWriteData.Length, testData.Length);

            for (int i = 0; i < afterWriteData.Length; i++)
                if (afterWriteData[i] != testData[i])
                    Assert.Fail($"Arrays {nameof(afterWriteData)} and {nameof(testData)} are not equals (element №{i})");
        }
        
        [UnityTest]
        public IEnumerator ReadAllBytesShouldReadBytesInFileTest()
        {
            var testData = new byte[1024];
            
            var pathToTestFile = $"~{nameof(ReadAllBytesShouldReadBytesInFileTest)}";
            var random = new Random();
            random.NextBytes(testData);

            File.WriteAllBytes(pathToTestFile, testData);

            yield return FileRoutine.ReadAllBytesRoutine(pathToTestFile)
                .Result(out var readAllBytesResult);
            
            File.Delete(pathToTestFile);

            if (readAllBytesResult.Routine.IsError)
                throw readAllBytesResult.Routine.Exception;

            var readData = readAllBytesResult.Result;

            Assert.AreEqual(readData.Length, testData.Length);

            for (int i = 0; i < readData.Length; i++)
                if (readData[i] != testData[i])
                    Assert.Fail($"Arrays {nameof(readData)} and {nameof(testData)} are not equals (element №{i})");
        }
    }
}