using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Omega.Experimental.Event;
using Omega.Routines;
using Omega.Routines.Exceptions;
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

        public static Exception GetCantContinueBecauseNestedRoutineHaveError(Routine nestedRoutine)
        {
            return new NestedRoutineException(Messages.CantContinueBecauseNestedRoutineHaveError, nestedRoutine);
        }

        public static Exception GetCantContinueBecauseNestedRoutineIsCancelled(Routine nestedRoutine)
        {
            return new NestedRoutineException(Messages.CantContinueBecauseNestedRoutineIsCancelled, nestedRoutine);
        }

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

            public static readonly string CantContinueBecauseNestedRoutineHaveError =
                $"Unable continue execute routine because nested routine have error. " +
                $"You can use the {nameof(RoutineExtension.Catch)} extension-method for nested routine if this behavior is expected";

            public static readonly string CantContinueBecauseNestedRoutineIsCancelled =
                $"Unable continue execute routine because nested routine were cancelled. " +
                $"You can use the <i>{nameof(RoutineExtension.Catch)}</i> extension-method for nested routine if this behavior is expected";

            public static string CreateExceptionMessageForRoutine(Routine routine)
            {
                RichTextFactory AddExceptionInfo(RichTextFactory rtf)
                {
                    var exception = routine.Exception;
                    if (exception != null)
                    {
                        rtf.NewLine.NewLine.Bold.Text(" ▸ EXCEPTION")
                            .NewLine.UnstyledText(exception.Message);

                        var stacktrace = exception.StackTrace;
                        if (exception is NestedRoutineException nestedRoutineException)
                        {
                            var nestedRoutine = nestedRoutineException.NestedRoutine;

                            if (nestedRoutine.Exception != null &&
                                nestedRoutine.Exception.StackTrace != null)
                                rtf.NewLine.NewLine.Bold.Text(" ▸ NESTED STACK TRACE")
                                    .NewLine.UnstyledText(
                                        StackTraceUtility.ExtractStringFromException(nestedRoutineException
                                            .NestedRoutine.Exception));

                            var cancellationStackTrace = nestedRoutine.GetCancellationStackTraceInternal();
                            if (nestedRoutine.IsCanceled)
                                if (cancellationStackTrace == null)
                                {
                                    rtf.NewLine.NewLine.Bold.Text(" ▸ NO NESTED CANCELLATION STACK TRACE")
                                        .NewLine.UnstyledText("You can call the Cancel method with the false parameter to add a stack trace");
                                }
                                else
                                {
                                    rtf.NewLine.NewLine.Bold.Text(" ▸ NESTED CANCELLATION STACK TRACE")
                                        .NewLine.UnstyledText(
                                            StackTraceUtility.ExtractFormattedStackTrace(cancellationStackTrace));
                                }
                        }


                        if (stacktrace == null)
                        {
                            rtf.NewLine.NewLine.Bold
                                .Text(" ▸ exception was created but was not thrown so no stacktrace").NewLine.Drop();
                        }
                        else
                        {
                            rtf.NewLine.NewLine.Bold.Text(" ▸ STACK TRACE")
                                .NewLine.UnstyledText(StackTraceUtility.ExtractStringFromException(exception));
                        }
                    }
                    else
                    {
                        rtf.NewLine.NewLine.Bold.Text(" ▸ ERROR WITHOUT EXCEPTION");
                    }

                    return rtf;
                }

                RichTextFactory AddLogHeader(RichTextFactory rtf)
                {
                    var exception = routine.Exception;
                    if (exception != null)
                    {
                        rtf.Text("Exception inside routine ▸▸▸ ");
                        AddTypeInfo(rtf, exception.GetType());
                    }
                    else
                    {
                        rtf.Text("Error inside routine without exception");
                    }

                    return rtf;
                }

                var creationStackTrace = routine.GetCreationStackTraceInternal();
                var messageFactory = RichTextFactory.New(250);
                AddLogHeader(messageFactory) // .Bold.Text(exception.GetType().ToString())
                    .NewLine.Italic.Text("ROUTINE TYPE ▸ ");

                AddTypeInfo(messageFactory, routine.GetType())
                    .NewLine.UnstyledText("▾ ▾ ▾ ▾ ▾");

                AddExceptionInfo(messageFactory);

                messageFactory.NewLine.Bold.Text(" ▸ CREATION STACK TRACE");
                messageFactory.NewLine.UnstyledText(
                    string.IsNullOrEmpty(creationStackTrace)
                        ? "You can call CreationStackTrace method at routine to show creation stack trace"
                        : creationStackTrace);

                return messageFactory.ToString();
            }

            private static RichTextFactory AddTypeInfo(RichTextFactory richTextFactory, Type type)
            {
                var @namespace = type.Namespace;
                var typeName = type.Name;

                return richTextFactory.DefaultStyle.Text(@namespace).Text(".").Bold.Text(typeName);
            }

            public static string ObjectIsNotInstanceOfIEventHandler(Type eventType)
                => string.Format(ObjectIsNotInstanceOfIEventHandlerFormattable,
                    string.Join(".", eventType.Namespace, eventType.Name));
        }
    }
}