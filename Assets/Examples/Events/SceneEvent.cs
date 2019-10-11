using System;
using Omega.Tools.Experimental.Events;
using Omega.Tools.Experimental.Events.Attributes;
using UnityEngine;

namespace Omega.Tools.Examples.Events.SceneEvent
{
    /*
     * Task: Создать событие которое будет уведомлять подписчиков о том что игрок вошел в опасную зону
     *
     * Note: Если повесить атрибут SceneEvent на класс/структуру которое представляет собой событие, то
     * все подписчики этого события будут "принадлежать" сцене, таким образом при переключении на другую сцену
     * одно и тоже событие будет уведомлять разную группу подписчиков.
     *
     * По умолчанию события глобальны и все их подписчики хранятся без всякой привязки к сцене
     */

    [SceneEvent]
    internal readonly struct PlayerEnterOnDangerArea
    {
        public readonly string DangerAreaName;

        public PlayerEnterOnDangerArea(string dangerAreaName)
        {
            DangerAreaName = dangerAreaName;
        }
    }

    internal class DangerAreaTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(!other.tag.Equals("area-danger"))
                return;
            
            var areaName = other.gameObject.name; 
            EventAggregator.Event(new PlayerEnterOnDangerArea(areaName));
        }
    }
}