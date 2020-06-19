using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omega.Package
{
    public static class SelectableExtensions
    {
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
    }
}