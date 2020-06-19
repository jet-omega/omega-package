using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omega.Package
{
    public static class SelectableExtensions
    {
        /// TODO: Перенести в Omega Constructor
        /// <summary>
        /// Подписывает обработчик на переданный тип события 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="triggerType">Тип события</param>
        /// <param name="handler">Обработчик события</param>
        public static void On(this Selectable self, EventTriggerType triggerType, Action handler)
        {
            var missingComponent = self.gameObject.MissingComponent<EventTrigger>();
            var triggers = missingComponent.triggers;

            if (!missingComponent.triggers.TryFind(x => x.eventID == triggerType, out var entry))
                triggers.Add(entry = new EventTrigger.Entry {eventID = triggerType});

            entry.callback.AddListener(_ => handler());
        }

        //TODO: Перенести в отдельный класс для расширений коллекций
        /// <summary>
        /// Возвращает элемен типа T, найденный в списке. Если элемента не было, создаёт его, добавляет в список и возвращает.
        /// </summary>
        /// <param name="list">Лист для поиска</param>
        /// <param name="match">Делегат</param>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <returns></returns>
        private static T FindOrCreate<T>(this List<T> list, Predicate<T> match) where T : class, new()
        {
            var result = list.Find(match);

            if (result is default(T))
            {
                result = new T();
                list.Add(result);

                return result;
            }

            return result;
        }

        //TODO: Перенести в отдельный класс для расширений коллекций
        /// <summary>
        /// Возвращает элемен типа T, если тот есть в списке. 
        /// </summary>
        /// <param name="list">Лист для поиска</param>
        /// <param name="match">Делегат</param>
        /// <param name="obj"></param>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <returns>Вернет true если совпадение было и false, если нет</returns>
        private static bool TryFind<T>(this List<T> list, Predicate<T> match, out T obj)
        {
            var index = list.FindIndex(match);
            obj = index >= 0 ? list[index] : default;
            return index >= 0;
        }
    }
}