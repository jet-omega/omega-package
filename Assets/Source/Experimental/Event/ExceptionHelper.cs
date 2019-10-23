using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Experimental.Event
{
    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    public static class ExceptionHelper
    {
        public static Exception ActionIsNotInstanceOfUnityObjectMethod
            => new InvalidCastException(Messages.ActionIsNotInstanceOfUnityObjectMethod);

        public static Exception ActionCannotCalledWhenObjectIsDestroyed
            => new MissingReferenceException(Messages.ActionCannotCalledWhenObjectIsDestroyed);


        public static Exception HandlerIsNotInstanceOfUnityObject
            => new InvalidCastException(Messages.HandlerIsNotInstanceOfUnityObject);

        public static Exception ObjectIsNotInstanceOfIEventHandler(Type typeEvent)
            => new InvalidCastException(Messages.ObjectIsNotInstanceOfIEventHandler(typeEvent));
        
        public static Exception MethodCannotCalledWhenObjectIsDestroyed
            => new MissingReferenceException(Messages.MethodCannotCalledWhenObjectIsDestroyed);

        public static class Messages
        {
            #region ActionHandlerUnityAdapter

            // Указанный метод не является экземплярным методом объекта унаследованного от UnityEngine.Object 
            public static readonly string ActionIsNotInstanceOfUnityObjectMethod =
                $"The specified method is not an instance method of an object inherited from {nameof(UnityEngine.Object)}";

            // Экземплярный метод не может быть вызван в уничтоженном объекте, согласно политики вызова для этого метода
            // Возможно где-то пропущена отписка от события или не правильно выбрана политика вызова
            public static readonly string ActionCannotCalledWhenObjectIsDestroyed =
                $"The instance method cannot be called when the object is destroyed, according to the {nameof(InvocationPolicy)} for this method. " +
                $"Perhaps the response from the event is missed somewhere or the {nameof(InvocationPolicy)} is not correctly selected";

            // Action был вызван в уничтоженном объекте, согласно политике вызова, однако такое поведение не считается корректным
            public static readonly string ActionWasCalledInTheDestroyedObject =
                $"Action was called in the destroyed object, according to the {nameof(InvocationPolicy)}, however, this behavior is not considered correct";

            #endregion
            
            
            // Указанный обработчик не является объектом унаследованным от UnityEngine.Object 
            public static readonly string HandlerIsNotInstanceOfUnityObject =
                $"The specified handler is not an object inherited from {nameof(UnityEngine.Object)}";
            
            // Указанный объект не наследуется от интерфейса IEventHandler<$0> 
            private static readonly string ObjectIsNotInstanceOfIEventHandlerFormattable =
                "The specified object is not inherited from the IEventHandler<{0}> interface";
            
            // Экземплярный метод не может быть вызван в уничтоженном объекте, согласно политики вызова для этого метода
            // Возможно где-то пропущена отписка от события или не правильно выбрана политика вызова
            public static readonly string MethodCannotCalledWhenObjectIsDestroyed =
                $"The instance method cannot be called when the object is destroyed, according to the {nameof(InvocationPolicy)} for this method. " +
                $"Perhaps the response from the event is missed somewhere or the {nameof(InvocationPolicy)} is not correctly selected";

            // Метод был вызван в уничтоженном объекте, согласно политике вызова, однако такое поведение не считается корректным
            public static readonly string MethodWasCalledInTheDestroyedObject =
                $"Action was called in the destroyed object, according to the {nameof(InvocationPolicy)}, however, this behavior is not considered correct";
            
            public static string ObjectIsNotInstanceOfIEventHandler(Type eventType)
                => string.Format(ObjectIsNotInstanceOfIEventHandlerFormattable,
                    string.Join(".", eventType.Namespace, eventType.Name));
        }
    }
}