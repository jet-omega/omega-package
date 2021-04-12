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
        /// <param name="obj"></param>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <returns>Вернет true если совпадение было и false, если нет</returns>
        public static bool TryFind<T>(this List<T> list, Predicate<T> match, out T obj)
        {
            var index = list.FindIndex(match);
            obj = index >= 0 ? list[index] : default;
            return index >= 0;
        }
    }
}