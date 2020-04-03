using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor.VersionControl;
using Omega.Experimental;

namespace Omega.Package.Tests
{
    public sealed class ArrayUtilitiesTests
    {
        [Test]
        public void AddTest()
        {
            var array = new[] { 1, 2, 3, 4 };
            var item = 5;
            var expected = new[] { 1, 2, 3, 4, 5 };

            Utilities.Array.Add(ref array, item);
            Assert.AreEqual(expected, array);

            array = new int[0];
            item = 1;
            expected = new[] { 1 };

            Utilities.Array.Add(ref array, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void AddRangeTest()
        {
            var array = new[] { 1, 2, 3, 4 };
            var items = new[] { 5, 6, 7, 8 };
            var expected = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            Utilities.Array.AddRange(ref array, items);
            Assert.AreEqual(expected, array);

            array = new int[0];
            items = new[] { 1, 2 };
            expected = new[] { 1, 2 };

            Utilities.Array.AddRange(ref array, items);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 2 };
            items = new int[0];
            expected = new[] { 1, 2 };

            Utilities.Array.AddRange(ref array, items);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void RemoveTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var item = 3;
            var expected = new[] { 1, 2, 4, 5 };

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 2, 3, 4, 5 };
            item = 5;
            expected = new[] { 1, 2, 3, 4 };

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 2, 3, 4, 5 };
            item = 1;
            expected = new[] { 2, 3, 4, 5 };

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] { 1 };
            item = 1;
            expected = new int[0];

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void RemoveAtTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var index = 2;
            var expected = new[] { 1, 2, 4, 5 };

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 2, 3, 4, 5 };
            index = 4;
            expected = new[] { 1, 2, 3, 4 };

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 2, 3, 4, 5 };
            index = 0;
            expected = new[] { 2, 3, 4, 5 };

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);

            array = new[] { 1 };
            index = 0;
            expected = new int[0];

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void RemoveAllTest()
        {
            var array = new[] { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 };
            var item = 3;
            var expected = new[] { 1, 2, 4, 5, 1, 2, 4, 5, 1, 2, 4, 5 };

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 2, 3, 4, 5, 5, 5 };
            item = 5;
            expected = new[] { 1, 2, 3, 4 };

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 1, 2, 3, 4, 5 };
            item = 1;
            expected = new[] { 2, 3, 4, 5 };

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] { 1, 1, 1 };
            item = 1;
            expected = new int[0];

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void Insert()
        {
            var array = new[] { 1, 2, 4, 5 };
            var index = 2;
            var item = 3;
            var expected = new[] { 1, 2, 3, 4, 5 };

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);


            array = new[] { 2, 3, 4, 5 };
            index = 0;
            item = 1;
            expected = new[] { 1, 2, 3, 4, 5 };

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);


            array = new[] { 1, 2, 3, 4 };
            index = 4;
            item = 5;
            expected = new[] { 1, 2, 3, 4, 5 };

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);

            array = new int[0];
            index = 0;
            item = 1;
            expected = new[] { 1 };

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void ArrayReferenceEqualsTest()
        {
            var array = new[] { "abc", "def", "ghi" };
            var array2 = new[] { "abc", "def", "ghi" };
            var expected = true;

            var actual = Utilities.Array.ArrayReferenceEquals(array, array2);
            Assert.AreEqual(expected, actual);

            array = new[] { "abc", "def", "ghi" };
            var string1 = new string(new[] { 'a', 'b', 'c' });
            var string2 = new string(new[] { 'd', 'e', 'f' });
            var string3 = new string(new[] { 'g', 'h', 'i' });
            array2 = new[] { string1, string2, string3 };
            expected = false;

            actual = Utilities.Array.ArrayReferenceEquals(array, array2);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void ArrayEqualsTest()
        {
            var array = new[] { "abc", "def", "ghi" };
            var array2 = new[] { "abc", "def", "ghi" };
            var expected = true;

            var actual = Utilities.Array.ArrayEquals(array, array2);
            Assert.AreEqual(expected, actual);

            array = new[] { "abc", "def", "ghi" };
            var string1 = new string(new[] { 'a', 'b', 'c' });
            var string2 = new string(new[] { 'd', 'e', 'f' });
            var string3 = new string(new[] { 'g', 'h', 'i' });
            array2 = new[] { string1, string2, string3 };
            expected = true;

            actual = Utilities.Array.ArrayEquals(array, array2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContainsTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var item = 3;
            var expected = true;

            var actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);

            array = new[] { 1, 2, 3, 4, 5 };
            item = 5;
            expected = true;

            actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);

            array = new[] { 1, 2, 3, 4, 5 };
            item = 6;
            expected = false;

            actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);

            array = new[] { 1 };
            item = 1;
            expected = true;

            actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);
        }
    }
}
