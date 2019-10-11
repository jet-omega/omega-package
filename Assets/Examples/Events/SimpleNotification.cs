using System;
using Omega.Tools.Experimental.Events;
using UnityEngine;

namespace Omega.Tools.Examples.Events.SimpleNotification
{
    /*
     * Task:
     * 1. Создать событие которое будет уведомлять подписчиков о том что был собран бонус
     * 2. Сделать компонент который будет логировать поднятие бонуса
     */
    
    /*
     * 1.1 Описываем событие
     */
    internal readonly struct PickupBoundsEvent { }
    
    /*
     * 1.2 Создаем контроллер который будет инициировать поднятие бонуса и эмитировать событие 
     */
    internal class PickupBonusController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(!other.tag.Equals("bonus"))
                return;
            
            Destroy(other.gameObject);
            
            EventAggregator.Event(new PickupBoundsEvent());
        }
    }
    
    /*
     * 2. Создаем компонент который будет реагировать на событие и делать лог
     */
    internal class PickupBonusLogger : MonoBehaviour
    {
        private void OnEnable()
            => EventAggregator.AddHandler<PickupBoundsEvent>(PickupBonusHandler);
        private void OnDisable()
            => EventAggregator.RemoveHandler<PickupBoundsEvent>(PickupBonusHandler);

        private void PickupBonusHandler(PickupBoundsEvent arg)
            => Debug.Log("Pickup bonus!");
    }
}