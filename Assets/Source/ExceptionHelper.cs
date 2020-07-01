using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Omega.Routines;
using Omega.Text;

namespace Omega.Package
{
    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    internal static class ExceptionHelper
    {
        public static Exception HandlerIsNotInstanceOfUnityObject
            => new InvalidCastException(Messages.HandlerIsNotInstanceOfUnityObject);

        public static Exception ObjectIsNotInstanceOfIEventHandler(Type typeEvent)
            => new InvalidCastException(Messages.ObjectIsNotInstanceOfIEventHandler(typeEvent));

        public static Exception SetResultCannotCalledWhenRoutineIsNotDefined
            => new AggregateException(Messages.SetResultCannotCalledWhenRoutineIsNotDefined);

        public static class Messages
        {
            // Указанный обработчик не является объектом унаследованным от UnityEngine.Object 
            public static readonly string HandlerIsNotInstanceOfUnityObject =
                $"The specified handler is not an object inherited from {nameof(UnityEngine.Object)}";

            // Указанный объект не наследуется от интерфейса IEventHandler<$0> 
            private static readonly string ObjectIsNotInstanceOfIEventHandlerFormattable =
                "The specified object is not inherited from the IEventHandler<{0}> interface";

            // Невозможно установить результат, так как рутина не задана. Если RoutineControl используется как мост между
            // IEnumerator`ом и рутиной, то необходимо использовать Routine.ByEnumerator
            public static readonly string SetResultCannotCalledWhenRoutineIsNotDefined =
                $"It is not possible to set the result because the {nameof(Routine)} is not defined. " +
                $"If {nameof(RoutineControl)} is used as a bridge between {nameof(IEnumerator)} and {nameof(Routine)}, " +
                $"Then you must use {nameof(Routine)}.{nameof(Routine.ByEnumerator)}";


            public static string CreateExceptionMessageForRoutine(Routine routine, Exception exception)
            {
                var creationStackTrace = routine.GetCreationStackTraceInternal();
                var messageFactory = RichTextFactory.New(exception.Message.Length + 200)
                    .Text("Exception thrown inside routine ▸ ").Bold.Text(exception.GetType().ToString())
                    .NewLine.Italic.Text("ROUTINE TYPE ▸ ").UnstyledText(routine.GetType().Namespace).UnstyledText(".")
                    .Bold.Text(routine.GetType().Name)
                    .NewLine.UnstyledText("▾ ▾ ▾ ▾ ▾")
                    .NewLine.Bold.Text("▸ ▸ ▸ EXCEPTION ◂ ◂ ◂")
                    .NewLine.UnstyledText(exception.Message)
                    .NewLine
                    .NewLine.Bold.Text("▸ ▸ ▸ STACK TRACE ◂ ◂ ◂")
                    .NewLine.UnstyledText(StackTraceUtility.ExtractStringFromException(exception));
                

                messageFactory.NewLine.Bold.Text("▸ ▸ ▸ CREATION STACK TRACE ◂ ◂ ◂");
                messageFactory.NewLine.UnstyledText(
                        string.IsNullOrEmpty(creationStackTrace)
                            ? "You can call CreationStackTrace method at routine to show creation stack trace"
                            : creationStackTrace);

                return messageFactory.ToString();
            }

            public static string ObjectIsNotInstanceOfIEventHandler(Type eventType)
                => string.Format(ObjectIsNotInstanceOfIEventHandlerFormattable,
                    string.Join(".", eventType.Namespace, eventType.Name));
        }
    }
}
