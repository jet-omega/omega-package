using System;
using UnityEngine;
using Omega.Tools.Experimental.Events;

namespace Omega.Tools.Examples.Events.CreateSimpleEventNotification
{
    /*
     * 1. Create Event Type with "Event" postfix
     */
    internal struct SimpleEvent { }
    
    /*
     * 2. Create event handler with our event type
     */

    internal class SimpleEventHandler : IEventHandler<SimpleEvent>
    {
        public void Execute(SimpleEvent arg)
        => Debug.Log("Simple event were executed");
    }
    
    /*
     *  3. Subscription handler on event in MonoBehaviour  
     */

    internal class SomeMonoBehaviour : MonoBehaviour
    {
        private SimpleEventHandler _simpleEventHandler;

        // Create instance handler
        private void Awake()
            => _simpleEventHandler = new SimpleEventHandler();

        // Subscribe handler to event SimpleEvent
        private void OnEnable()
            => Experimental.Events.EventAggregator.AddHandler(_simpleEventHandler);

        // Remove subscription from event
        private void OnDisable()
            => Experimental.Events.EventAggregator.RemoveHandler(_simpleEventHandler);

        // Activation event
        private void OnTrigger()
            => Experimental.Events.EventAggregator.Event(new SimpleEvent());
    }
}