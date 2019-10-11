using System;
using Omega.Tools.Experimental.Events;
using UnityEngine;


/*
 * Task: Необходимо добавить логирование при вхождение игрока в область объекта который имеет тег "LogWhenTrigger"
 */
namespace Omega.Tools.Examples.Events.CreateEventWithActionHandler
{
    /*
     * 1. Create event
     */
    internal readonly struct PlayerTriggeredWithOtherObjectEvent
    {
        public readonly Collider OtherCollider;
        public readonly Player PlayerInstance;

        public PlayerTriggeredWithOtherObjectEvent(Collider otherCollider, Player playerInstance)
        {
            OtherCollider = otherCollider;
            PlayerInstance = playerInstance;
        }
    }

    /*
     * Create Reciver 
     */
    internal sealed class LogWhenTriggeredController : MonoBehaviour
    {
        [SerializeField] private string LogWhenTriggerTag = "LogWhenTrigger";

        private void OnEnable()
            => Experimental.Events.EventAggregator.AddHandler<PlayerTriggeredWithOtherObjectEvent>(EventHandler);

        private void OnDisable()
            => Experimental.Events.EventAggregator.RemoveHandler<PlayerTriggeredWithOtherObjectEvent>(EventHandler);
        
        private void EventHandler(PlayerTriggeredWithOtherObjectEvent arg)
        {
            var otherGameObject = arg.OtherCollider.gameObject;
            if(LogWhenTriggerTag.Equals(otherGameObject.tag))
                Debug.Log($"Trigger {otherGameObject.name}" );
        }
    }
    
    /*
     * 3. Create Player and event activation source
     */
    internal sealed class Player : MonoBehaviour
    {
        // Activation our event when player triggered with other object
        private void OnTriggerEnter(Collider other)
            => Experimental.Events.EventAggregator.Event(new PlayerTriggeredWithOtherObjectEvent(other, this));
    }
}