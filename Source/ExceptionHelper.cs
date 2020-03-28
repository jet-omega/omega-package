using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Omega.Experimental.Event;
using Omega.Routines;
using Omega.Text;
using UnityEngine;


namespace Omega.Package
{
    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    internal static class ExceptionHelper
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

        public static Exception SetResultCannotCalledWhenRoutineIsNotDefined
            => new AggregateException(Messages.SetResultCannotCalledWhenRoutineIsNotDefined);

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

            // Невозможно установить результат, так как рутина не задана. Если RoutineControl используется как мост между
            // IEnumerator`ом и рутиной, то необходимо использовать Routine.ByEnumerator
            public static readonly string SetResultCannotCalledWhenRoutineIsNotDefined =
                $"It is not possible to set the result because the {nameof(Routine)} is not defined. " +
                $"If {nameof(RoutineControl)} is used as a bridge between {nameof(IEnumerator)} and {nameof(Routine)}, " +
                $"Then you must use {nameof(Routine)}.{nameof(Routine.ByEnumerator)}";

            // В одной из рутин было выброшено исключение
            public static readonly string ExceptionInRoutineMessageFormattable =
                "Exception thrown inside routine :  " +
                "\nROUTINE : {0}" +
                "\n\tEXCEPTION : {1})" +
                "\n\tSTACKTRACE : {2}";


            public static string CreateExceptionMessageForRoutine(Routine routine, Exception exception)
            {
                var creationStackTrace = routine.GetCreationStackTraceInternal();
                var message = RichTextFactory.New()
                    .Text("Exception thrown inside routine ▸ ").Bold.Text(exception.GetType().ToString())
                    .NewLine.Italic.Text("ROUTINE TYPE ▸ ").UnstyledText(routine.GetType().Namespace).UnstyledText(".").Bold.Text(routine.GetType().Name)
                    .NewLine.UnstyledText("▾ ▾ ▾ ▾ ▾")
                    .NewLine.Bold.Text("▸ ▸ ▸ EXCEPTION ◂ ◂ ◂")
                    .NewLine.UnstyledText(exception.Message)
                    .NewLine
                    .NewLine.Bold.Text("▸ ▸ ▸ STACK TRACE ◂ ◂ ◂")
                    .NewLine.UnstyledText(StackTraceUtility.ExtractStringFromException(exception))
                    .ToString();
                
                // Application.dataPath.Replace('/', Path.DirectorySeparatorChar)
                
                return message;
                // if (string.IsNullOrEmpty(creationStackTrace))
                // {
                //     return message +
                //            "\n\nYou can call CreationStackTrace method at routine to show creation stack trace";
                // }
                //
                // return message + $"\n\nRoutine was creation here:\n{creationStackTrace}";
            }

            public static string ObjectIsNotInstanceOfIEventHandler(Type eventType)
                => string.Format(ObjectIsNotInstanceOfIEventHandlerFormattable,
                    string.Join(".", eventType.Namespace, eventType.Name));
        }
    }
}