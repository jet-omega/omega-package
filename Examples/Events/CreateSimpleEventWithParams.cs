using Omega.Tools.Experimental.Events;
using UnityEngine;


namespace Omega.Tools.Examples.Events.CreateSimpleEventWithParams
{
    /*
     * 1. Create event
     */
    internal readonly struct SimpleEvent
    {
        public readonly string Message;
        public SimpleEvent(string message) 
            => Message = message;
    }
    
    
        /*
         * 2. Create event handler with our event type
         */
        internal class SimpleEventHandler : IEventHandler<SimpleEvent>
        {
            public void Execute(SimpleEvent arg)
                => Debug.Log($"Simple event were executed with message: {arg.Message}");
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

            // Activation event with arg
            private void OnTrigger()
                => Experimental.Events.EventAggregator.Event(new SimpleEvent($"Hello from {gameObject.name}"));
        }
}