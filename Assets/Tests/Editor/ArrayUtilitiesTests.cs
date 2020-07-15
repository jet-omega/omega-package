using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Omega.Package.Tests
{
    public sealed class ArrayUtilitiesTests
    {
        [Test]
        public void AddTest()
        {
            var array = new[] {1, 2, 3, 4};
            var item = 5;
            var expected = new[] {1, 2, 3, 4, 5};

            Utilities.Array.Add(ref array, item);
            Assert.AreEqual(expected, array);

            array = new int[0];
            item = 1;
            expected = new[] {1};

            Utilities.Array.Add(ref array, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void AddRangeTest()
        {
            var array = new[] {1, 2, 3, 4};
            var items = new[] {5, 6, 7, 8};
            var expected = new[] {1, 2, 3, 4, 5, 6, 7, 8};

            Utilities.Array.AddRange(ref array, items);
            Assert.AreEqual(expected, array);

            array = new int[0];
            items = new[] {1, 2};
            expected = new[] {1, 2};

            Utilities.Array.AddRange(ref array, items);
            Assert.AreEqual(expected, array);

            array = new[] {1, 2};
            items = new int[0];
            expected = new[] {1, 2};

            Utilities.Array.AddRange(ref array, items);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void RemoveTest()
        {
            var array = new[] {1, 2, 3, 4, 5};
            var item = 3;
            var expected = new[] {1, 2, 4, 5};

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] {1, 2, 3, 4, 5};
            item = 5;
            expected = new[] {1, 2, 3, 4};

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] {1, 2, 3, 4, 5};
            item = 1;
            expected = new[] {2, 3, 4, 5};

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] {1};
            item = 1;
            expected = new int[0];

            Utilities.Array.Remove(ref array, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void RemoveAtTest()
        {
            var array = new[] {1, 2, 3, 4, 5};
            var index = 2;
            var expected = new[] {1, 2, 4, 5};

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);

            array = new[] {1, 2, 3, 4, 5};
            index = 4;
            expected = new[] {1, 2, 3, 4};

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);

            array = new[] {1, 2, 3, 4, 5};
            index = 0;
            expected = new[] {2, 3, 4, 5};

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);

            array = new[] {1};
            index = 0;
            expected = new int[0];

            Utilities.Array.RemoveAt(ref array, index);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void RemoveAllTest()
        {
            var array = new[] {1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5};
            var item = 3;
            var expected = new[] {1, 2, 4, 5, 1, 2, 4, 5, 1, 2, 4, 5};

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] {1, 2, 3, 4, 5, 5, 5};
            item = 5;
            expected = new[] {1, 2, 3, 4};

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] {1, 1, 2, 3, 4, 5};
            item = 1;
            expected = new[] {2, 3, 4, 5};

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);

            array = new[] {1, 1, 1};
            item = 1;
            expected = new int[0];

            Utilities.Array.RemoveAll(ref array, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        public void Insert()
        {
            var array = new[] {1, 2, 4, 5};
            var index = 2;
            var item = 3;
            var expected = new[] {1, 2, 3, 4, 5};

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);


            array = new[] {2, 3, 4, 5};
            index = 0;
            item = 1;
            expected = new[] {1, 2, 3, 4, 5};

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);


            array = new[] {1, 2, 3, 4};
            index = 4;
            item = 5;
            expected = new[] {1, 2, 3, 4, 5};

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);

            array = new int[0];
            index = 0;
            item = 1;
            expected = new[] {1};

            Utilities.Array.Insert(ref array, index, item);
            Assert.AreEqual(expected, array);
        }

        [Test]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public void ArrayReferenceEqualsTest()
        {
            var array = new[] {"abc", "def", "ghi"};
            var array2 = new[] {"abc", "def", "ghi"};
            var expected = true;

            var actual = Utilities.Array.ArrayReferenceEquals(array, array2);
            Assert.AreEqual(expected, actual);

            array = new[] {"abc", "def", "ghi"};
            var string1 = new string(new[] {'a', 'b', 'c'});
            var string2 = new string(new[] {'d', 'e', 'f'});
            var string3 = new string(new[] {'g', 'h', 'i'});
            array2 = new[] {string1, string2, string3};
            expected = false;

            actual = Utilities.Array.ArrayReferenceEquals(array, array2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public void ArrayEqualsTest()
        {
            var array = new[] {"abc", "def", "ghi"};
            var array2 = new[] {"abc", "def", "ghi"};
            var expected = true;

            var actual = Utilities.Array.ArrayEquals(array, array2);
            Assert.AreEqual(expected, actual);

            array = new[] {"abc", "def", "ghi"};
            var string1 = new string(new[] {'a', 'b', 'c'});
            var string2 = new string(new[] {'d', 'e', 'f'});
            var string3 = new string(new[] {'g', 'h', 'i'});
            array2 = new[] {string1, string2, string3};
            expected = true;

            actual = Utilities.Array.ArrayEquals(array, array2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public void ContainsTest()
        {
            var array = new[] {1, 2, 3, 4, 5};
            var item = 3;
            var expected = true;

            var actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);

            array = new[] {1, 2, 3, 4, 5};
            item = 5;
            expected = true;

            actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);

            array = new[] {1, 2, 3, 4, 5};
            item = 6;
            expected = false;

            actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);

            array = new[] {1};
            item = 1;
            expected = true;

            actual = Utilities.Array.Contains(array, item);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SelectorSortTest()
        {
            string[] array = {"_a__", "_d__", "_e__", "_b__", "_f__", "_c__"};
            string[] expected = {"_a__", "_b__", "_c__", "_d__", "_e__", "_f__"};

            Utilities.Array.Sort(array, s => s[1]);
            Assert.IsTrue(Utilities.Array.ArrayEquals(array, expected));

            array = new[] {"_a__", "_d__", "_e__", "_b__", "_f__", "_c__"};
            expected = new[] {"_a__", "_d__", "_b__", "_e__", "_f__", "_c__"};
            const int index = 2;
            const int length = 3;

            Utilities.Array.Sort(array, index, length, s => s[1]);
            Assert.IsTrue(Utilities.Array.ArrayEquals(array, expected));
        }

        [Test]
        public void BinarySearchTest()
        {
            string[] array = {"_a__", "_b__", "_c__", "_d__", "_e__", "_f__"};
            const char value = 'c';
            const int expected = 2;

            var result = Utilities.Array.BinarySearch(array, value, s => s[1]);
            Assert.AreEqual(result, expected);
        }

        [Test]
        public void FillTest()
        {
            var array = new char[5];

            Utilities.Array.Fill(ref array, 'a');

            Assert.IsTrue(Utilities.Array.ArrayEquals(array, new[] {'a', 'a', 'a', 'a', 'a'}));
        }

        [Test]
        public void FillIndexTest()
        {
            var array = new char[5];

            Utilities.Array.Fill(ref array, 1, 3, 'a');

            Assert.IsTrue(Utilities.Array.ArrayEquals(array, new[] {'\0', 'a', 'a', 'a', '\0'}));
        }
        
        [Test]
        public void FillSelectorTest()
        {
            var array = new char[5];

            Utilities.Array.Fill(ref array, i => i % 2 == 0 ? 'a' : 'b');

            Assert.IsTrue(Utilities.Array.ArrayEquals(array, new[] {'a', 'b', 'a', 'b', 'a'}));
        }

        [Test]
        public void FillSelectorIndexTest()
        {
            var array = new char[5];

            Utilities.Array.Fill(ref array, 1, 3, i => i % 2 == 0 ? 'a' : 'b');

            Assert.IsTrue(Utilities.Array.ArrayEquals(array, new[] {'\0', 'b', 'a', 'b', '\0'}));
        }
    }
}