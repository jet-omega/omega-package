using System;
using System.Text;
using NUnit.Framework;
using Omega.Package.Experimental;
using Unity.PerformanceTesting;

namespace Omega.Package.Tests
{
    public sealed class SlimStringBuilderTests
    {
        [Test]
        public void AppendCharStackCase()
        {
            var slimStringBuilder = new SlimStringBuilder();
            slimStringBuilder.Append('a');
            slimStringBuilder.Append('b');
            slimStringBuilder.Append('c');
            var result = slimStringBuilder.ToString();
            Assert.AreEqual("abc", result);
        }

        [Test]
        public void AppendCharHeapCase()
        {
            var @string = string.Empty;
            var stringLength = 1024;
            for (int i = 0; i < stringLength; i++) @string += (char) (i % 26 + 65);
            var slimStringBuilder = new SlimStringBuilder();
            foreach (var @char in @string)
                slimStringBuilder.Append(@char);
            var result = slimStringBuilder.ToString();
            Assert.AreEqual(@string, result);
        }

        [Test]
        public void AppendStringStackCase()
        {
            var slimStringBuilder = new SlimStringBuilder();
            slimStringBuilder.Append("abc");
            slimStringBuilder.Append("def");
            slimStringBuilder.Append("ghi");
            var result = slimStringBuilder.ToString();
            Assert.AreEqual("abcdefghi", result);
        }

        [Test]
        public void AppendStringsHeapCase()
        {
            string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty;
            for (int i = 0; i < 1024; i++) string1 += (char) (i % 26 + 65);
            for (int i = 0; i < 1024; i++) string2 += (char) (i % 26 + 65);
            for (int i = 0; i < 1024; i++) string3 += (char) (i % 26 + 65);

            var slimStringBuilder = new SlimStringBuilder();
            slimStringBuilder.Append(string1);
            slimStringBuilder.Append(string2);
            slimStringBuilder.Append(string3);
            var result = slimStringBuilder.ToString();

            Assert.AreEqual(string1 + string2 + string3, result);
        }

        [Test]
        public void ClearHeapCase()
        {
            string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty;
            var stringLength = 1024;
            for (int i = 0; i < stringLength; i++) string1 += (char) (i % 26 + 65);
            for (int i = 0; i < stringLength; i++) string2 += (char) (i % 26 + 65);
            for (int i = 0; i < stringLength; i++) string3 += (char) (i % 26 + 65);

            var slimStringBuilder = new SlimStringBuilder();
            slimStringBuilder.Append(string1);
            slimStringBuilder.Append(string2);
            slimStringBuilder.Append(string3);
            slimStringBuilder.Clear();
            var result = slimStringBuilder.ToString();
            Assert.AreEqual(string.Empty, result);
            slimStringBuilder.Append(string1);
            slimStringBuilder.Append(string2);
            slimStringBuilder.Append(string3);
            result = slimStringBuilder.ToString();
            Assert.AreEqual(string1 + string2 + string3, result);
        }

        [Test]
        public void ClearStackCase()
        {
            string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty;
            var stringLength = 16;
            for (int i = 0; i < stringLength; i++) string1 += (char) (i % 26 + 65);
            for (int i = 0; i < stringLength; i++) string2 += (char) (i % 26 + 65);
            for (int i = 0; i < stringLength; i++) string3 += (char) (i % 26 + 65);

            var slimStringBuilder = new SlimStringBuilder();
            slimStringBuilder.Append(string1);
            slimStringBuilder.Append(string2);
            slimStringBuilder.Append(string3);
            slimStringBuilder.Clear();
            var result = slimStringBuilder.ToString();
            Assert.AreEqual(string.Empty, result);
            slimStringBuilder.Append(string1);
            slimStringBuilder.Append(string2);
            slimStringBuilder.Append(string3);
            result = slimStringBuilder.ToString();
            Assert.AreEqual(string1 + string2 + string3, result);
        }

        [Test]
        public void ReplaceHeapCase()
        {
            var @string = string.Empty;
            var stringLength = 1024;
            for (int i = 0; i < stringLength; i++) @string += (char) (i % 26 + 65);

            var slimStringBuilder = new SlimStringBuilder();
            slimStringBuilder.Append(@string);
            slimStringBuilder.Replace('A', 'Z');
            var result = slimStringBuilder.ToString();
            Assert.AreEqual(@string.Replace('A', 'Z'), result);
        }

        [Test]
        public void ReplaceStackCase()
        {
            var @string = string.Empty;
            var stringLength = 16;
            for (int i = 0; i < stringLength; i++) @string += (char) (i % 26 + 65);

            var slimStringBuilder = new SlimStringBuilder();
            slimStringBuilder.Append(@string);
            slimStringBuilder.Replace('A', 'Z');
            var result = slimStringBuilder.ToString();
            Assert.AreEqual(@string.Replace('A', 'Z'), result);
        }

        // [Test]
        // [Performance]
        // public void SlimStringBuilderConcat4X1024StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 1024;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var slimStringBuilder = new SlimStringBuilder();
        //         slimStringBuilder.Append(string1);
        //         slimStringBuilder.Append(string2);
        //         slimStringBuilder.Append(string3);
        //         slimStringBuilder.Append(string4);
        //         var result = slimStringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void StringBuilderConcat4X1024StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 1024;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var stringBuilder = new StringBuilder();
        //         stringBuilder.Append(string1);
        //         stringBuilder.Append(string2);
        //         stringBuilder.Append(string3);
        //         stringBuilder.Append(string4);
        //         var result = stringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void SlimStringBuilderConcat4X512StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 512;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var slimStringBuilder = new SlimStringBuilder();
        //         slimStringBuilder.Append(string1);
        //         slimStringBuilder.Append(string2);
        //         slimStringBuilder.Append(string3);
        //         slimStringBuilder.Append(string4);
        //         var result = slimStringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void StringBuilderConcat4X512StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 512;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var stringBuilder = new StringBuilder();
        //         stringBuilder.Append(string1);
        //         stringBuilder.Append(string2);
        //         stringBuilder.Append(string3);
        //         stringBuilder.Append(string4);
        //         var result = stringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void SlimStringBuilderConcat4X256StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 256;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var slimStringBuilder = new SlimStringBuilder();
        //         slimStringBuilder.Append(string1);
        //         slimStringBuilder.Append(string2);
        //         slimStringBuilder.Append(string3);
        //         slimStringBuilder.Append(string4);
        //         var result = slimStringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void StringBuilderConcat4X256StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 256;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var stringBuilder = new StringBuilder();
        //         stringBuilder.Append(string1);
        //         stringBuilder.Append(string2);
        //         stringBuilder.Append(string3);
        //         stringBuilder.Append(string4);
        //         var result = stringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void SlimStringBuilderConcat4X128StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 128;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var slimStringBuilder = new SlimStringBuilder();
        //         slimStringBuilder.Append(string1);
        //         slimStringBuilder.Append(string2);
        //         slimStringBuilder.Append(string3);
        //         slimStringBuilder.Append(string4);
        //         var result = slimStringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void StringBuilderConcat4X128StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 128;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var stringBuilder = new StringBuilder();
        //         stringBuilder.Append(string1);
        //         stringBuilder.Append(string2);
        //         stringBuilder.Append(string3);
        //         stringBuilder.Append(string4);
        //         var result = stringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void SlimStringBuilderConcat4X64StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 64;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var slimStringBuilder = new SlimStringBuilder();
        //         slimStringBuilder.Append(string1);
        //         slimStringBuilder.Append(string2);
        //         slimStringBuilder.Append(string3);
        //         slimStringBuilder.Append(string4);
        //         var result = slimStringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void StringBuilderConcat4X64StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 64;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var stringBuilder = new StringBuilder();
        //         stringBuilder.Append(string1);
        //         stringBuilder.Append(string2);
        //         stringBuilder.Append(string3);
        //         stringBuilder.Append(string4);
        //         var result = stringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void SlimStringBuilderConcat4X32StringsPerformanceTest()
        // {
        //     var random = new Random();
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 32;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var slimStringBuilder = new SlimStringBuilder();
        //         slimStringBuilder.Append(string1);
        //         slimStringBuilder.Append(string2);
        //         slimStringBuilder.Append(string3);
        //         slimStringBuilder.Append(string4);
        //         var result = slimStringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void StringBuilderConcat4X32StringsPerformanceTest()
        // {
        //     string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
        //     var lengthOfString = 32;
        //     for (int i = 0; i < lengthOfString; i++) string1 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string2 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string3 += (char) (i % 26 + 65);
        //     for (int i = 0; i < lengthOfString; i++) string4 += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         var stringBuilder = new StringBuilder();
        //         stringBuilder.Append(string1);
        //         stringBuilder.Append(string2);
        //         stringBuilder.Append(string3);
        //         stringBuilder.Append(string4);
        //         var result = stringBuilder.ToString();
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void SlimStringBuilderAppendCharPerformanceTest()
        // {
        //     var slimStringBuilder = new SlimStringBuilder();
        //     var lengthOfString = 32;
        //     var @string = string.Empty;
        //     for (int i = 0; i < lengthOfString; i++) @string += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         slimStringBuilder.Clear();
        //         foreach (var @char in @string)
        //             slimStringBuilder.Append(@char);
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
        //
        // [Test]
        // [Performance]
        // public void StringBuilderAppendCharPerformanceTest()
        // {
        //     var stringBuilder = new StringBuilder();
        //     var lengthOfString = 32;
        //     var @string = string.Empty;
        //     for (int i = 0; i < lengthOfString; i++) @string += (char) (i % 26 + 65);
        //
        //     void Test()
        //     {
        //         stringBuilder.Clear();
        //         foreach (var @char in @string)
        //             stringBuilder.Append(@char);
        //     }
        //
        //     Measure.Method(Test)
        //         .WarmupCount(100)
        //         .IterationsPerMeasurement(20000)
        //         .MeasurementCount(10)
        //         .Run();
        // }
    }
}