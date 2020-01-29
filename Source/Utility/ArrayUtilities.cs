using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omega.Tools
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
            bool isRemoved = false;
            for (var i = 0; i < array.Length - (isRemoved ? 1 : 0); ++i)
            {
                if (array[i].Equals(item))
                    isRemoved = true;

                if (isRemoved && i < array.Length - 1)
                    array[i] = array[i + 1];
            }

            if (isRemoved)
                Array.Resize(ref array, array.Length - 1);

            return isRemoved;
        }

        /// <summary>
        /// Удалить элемент массива array с индексом index
        /// </summary>
        public void RemoveAt<T>(ref T[] array, int index)
        {
            if (array.Length == 1 && index == 0)
            {
                Array.Resize(ref array, 0);
                return;
            }

            if (index != array.Length - 1) // Если нужно удалить НЕ последний элемент, нужно сдвинуть все остальные
            {
                for (var i = index; i < array.Length - 1; i++)
                    array[i] = array[i + 1];
            }

            Array.Resize(ref array, array.Length - 1);
        }

        /// <summary>
        /// Удалить все вхождения элемента item в массив array
        /// </summary>
        ///<returns>Количество удаленных элементов</returns>
        public int RemoveAll<T>(ref T[] array, T item)
        {
            int j = 0;
            var startArrayLength = array.Length;

            for (var i = 0; i < startArrayLength; ++i)
            {
                if (!array[i].Equals(item))
                {
                    array[j] = array[i];
                    j++;
                }
            }

            Array.Resize(ref array, j);
            return startArrayLength - j;
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
            return lhs.SequenceEqual(rhs);
        }

        /// <summary>
        /// Проверка массивов на равенство. Сравнение элементов по ссылкам
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public bool ArrayReferenceEquals<T>(T[] lhs, T[] rhs)
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
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(item))
                    return true;
            }

            return false;
        }

        public void Clear<T>(ref T[] array)
        {
            Array.Clear(array, 0, array.Length);
            Array.Resize(ref array, 0);
        }
    }
}
