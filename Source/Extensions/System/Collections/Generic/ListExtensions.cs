using System;
using System.Collections.Generic;

namespace Omega.Package
{
    public static class ListExtensions
    {
        /// <summary>
        /// Возвращает элемен типа T, если тот есть в списке. 
        /// </summary>
        /// <param name="list">Лист для поиска</param>
        /// <param name="match">Делегат</param>
        /// <param name="item"></param>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <returns>Вернет true если совпадение было и false, если нет</returns>
        public static bool TryFind<T>(this List<T> list, Predicate<T> match, out T item)
        {
            var index = list.FindIndex(match);
            item = index >= 0 ? list[index] : default;
            return index >= 0;
        }
    }
}