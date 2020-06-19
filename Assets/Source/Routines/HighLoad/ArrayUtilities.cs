using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace Omega.Package.Internal
{
    public class ArrayUtilities
    {
        /// <summary>
        /// Добавить элемент item в конец массива array
        /// </summary>
        public void Add<T>(ref T[] array, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }

        /// <summary>
        /// Добавить массив items в конец массива array
        /// </summary>
        public void AddRange<T>(ref T[] array, T[] items)
        {
            var sourceArrayLength = array.Length;
            var newArrayLength = sourceArrayLength + items.Length;

            Array.Resize(ref array, newArrayLength);

            for (var i = 0; i < items.Length; i++)
                array[i + sourceArrayLength] = items[i];
        }

        /// <summary>
        /// Удалить первое вхождение item в массив array
        /// </summary>
        /// <returns>True если элемент был удален</returns>
        public bool Remove<T>(ref T[] array, T item)
        {
            var index = Array.IndexOf(array, item);
            if (index == -1) return false;

            for (var j = index; j < array.Length - 1; j++)
                array[j] = array[j + 1];
            Array.Resize(ref array, array.Length - 1);
            return true;
        }

        /// <summary>
        /// Удалить элемент массива array с индексом index
        /// </summary>
        public void RemoveAt<T>(ref T[] array, int index)
        {
            for (var i = index; i < array.Length - 1; i++)
                array[i] = array[i + 1];

            Array.Resize(ref array, array.Length - 1);
        }

        /// <summary>
        /// Удалить все вхождения элемента item в массив array
        /// </summary>
        ///<returns>Количество удаленных элементов</returns>
        public int RemoveAll<T>(ref T[] array, T item)
        {
            var comparer = EqualityComparer<T>.Default;
            var startArrayLength = array.Length;

            int resultArrayIndex = 0;
            for (var i = 0; i < startArrayLength; ++i)
            {
                if (!comparer.Equals(array[i], item))
                {
                    array[resultArrayIndex] = array[i];
                    resultArrayIndex++;
                }
            }

            Array.Resize(ref array,
                resultArrayIndex); // Из-за "лишнего" инкремента на последней итерации здесь индекс на 1 больше,
            //чем последний индекс. То есть выступает в качестве Length
            return startArrayLength - resultArrayIndex;
        }

        /// <summary>
        /// Вставить элемент item в массив array на индекс index
        /// </summary>
        public void Insert<T>(ref T[] array, int index, T item)
        {
            Array.Resize(ref array, array.Length + 1);

            for (var i = array.Length - 1; i > index; i--) // array.Length здесь на 1 больше, чем исходный
                array[i] = array[i - 1];

            array[index] = item;
        }

        /// <summary>
        /// Проверка массивов на равенство. Сравнение элементов по значениям
        /// </summary>
        public bool ArrayEquals<T>(T[] lhs, T[] rhs)
        {
            if (lhs == rhs)
                return true;
            if (lhs.Length != rhs.Length)
                return false;
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < lhs.Length; i++)
            {
                if (!comparer.Equals(lhs[i], rhs[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Проверка массивов на равенство. Сравнение элементов по ссылкам
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public bool ArrayReferenceEquals<T>(T[] lhs, T[] rhs) where T : class
        {
            if (lhs == null || rhs == null)
                return lhs == rhs;
            if (lhs.Length != rhs.Length)
                return false;
            for (var index = 0; index < lhs.Length; ++index)
            {
                if (!ReferenceEquals(lhs[index], rhs[index]))
                    return false;
            }

            return true;
        }

        public T[] FindAll<T>(T[] array, Predicate<T> match)
        {
            return Array.FindAll(array, match);
        }

        public T Find<T>(T[] array, Predicate<T> match)
        {
            return Array.Find(array, match);
        }

        public int FindIndex<T>(T[] array, Predicate<T> match)
        {
            return Array.FindIndex(array, match);
        }

        public int IndexOf<T>(T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }

        public int LastIndexOf<T>(T[] array, T value)
        {
            return Array.LastIndexOf(array, value);
        }

        public bool Contains<T>(T[] array, T item)
        {
            var comparer = EqualityComparer<T>.Default;
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < array.Length; i++)
            {
                if (comparer.Equals(array[i], item))
                    return true;
            }

            return false;
        }

        public void Clear<T>(ref T[] array)
        {
            Array.Clear(array, 0, array.Length);
            Array.Resize(ref array, 0);
        }

        private readonly struct SelectComparer<T, TComparision> : IComparer<T>
            where TComparision : IComparable<TComparision>
        {
            private readonly Func<T, TComparision> _selector;

            public SelectComparer(Func<T, TComparision> selector) => _selector = selector;

            public int Compare(T x, T y) => _selector(x).CompareTo(_selector(y));
        }

        public void Sort<T, TOut>(T[] array, Func<T, TOut> selector) where TOut : IComparable<TOut>
        {
            Array.Sort(array, new SelectComparer<T, TOut>(selector));
        }

        public void Sort<T, TOut>(T[] array, int index, int length, Func<T, TOut> selector)
            where TOut : IComparable<TOut>
        {
            Array.Sort(array, index, length, new SelectComparer<T, TOut>(selector));
        }

        public int BinarySearch<T, TOut>(T[] array, TOut value, Func<T, TOut> selector) where TOut : IComparable<TOut>
        {
            return BinarySearch(array, 0, array.Length, value, selector);
        }

        public int BinarySearch<T, TOut>(T[] array, int index, int length, TOut value, Func<T, TOut> selector)
            where TOut : IComparable<TOut>
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            var lowerBound = array.GetLowerBound(0);
            if (index < lowerBound || length < 0)
                throw new ArgumentOutOfRangeException(index < lowerBound ? nameof(index) : nameof(length));
            if (array.Length - (index - lowerBound) < length)
                throw new ArgumentException();
            if (array.Rank != 1)
                throw new RankException("Array rank 1 is only supported");

            var low = index;
            var hi = index + length - 1;

            while (low <= hi)
            {
                var median = low + (hi - low >> 1);
                int num;
                try
                {
                    num = selector(array[median]).CompareTo(value);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("IComparer failed");
                }

                if (num == 0)
                    return median;
                if (num < 0)
                    low = median + 1;
                else
                    hi = median - 1;
            }

            return ~low;
        }
    }
}