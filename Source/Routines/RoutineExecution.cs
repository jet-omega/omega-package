using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;

namespace Omega.Routines
{
    public static class RoutineExecution
    {
        internal static bool ExecuteRound([NotNull] Routine routine)
        {
            //todo use stack on stack
            var stack = new Stack<Frame>();

            {
                var routineStateMachine = routine.GetStateMachine();
                stack.Push(new Frame(routine, routineStateMachine));
            }

            while (stack.Count > 0)
            {
                var (context, stateMachine) = stack.Peek();
                var current = stateMachine.Current;

                switch (current)
                {
                    case Routine currentRoutine:
                        if (currentRoutine.IsComplete)
                            break;
                        else
                        {
                            if (currentRoutine.IsError || currentRoutine.IsCanceled)
                            {
                                if (context is IRoutineContinuation routineContinuation &&
                                    routineContinuation.CanContinue())
                                    break;
                                
                                var exception = currentRoutine.IsError
                                    ? ExceptionHelper.CantContinueBecauseNestedRoutineHaveError
                                    : ExceptionHelper.CantContinueBecauseNestedRoutineIsCancelled;
                                context.SetupErrorInternal(exception);

                                StackBackward(stack);
                                return true;
                            }

                            if (context.IsForcedProcessing)
                                currentRoutine.SetupForcedProcessingInternal();
                            else
                                currentRoutine.SetupProgressingInternal();

                            if (currentRoutine.GetStateMachine() is null)
                            {
                                currentRoutine.SetupCompletedInternal();
                                StackBackward(stack);
                                return true;
                            }


                            stack.Push(new Frame(currentRoutine, currentRoutine.GetStateMachine()));
                            continue;
                        }


                    case AsyncOperation currentAsyncOperation:
                        if (!currentAsyncOperation.CanBeForceComplete())
                        {
                            var exception =
                                new Exception(
                                    $"unable to complete asynchronous operation synchronously. async operation: {currentAsyncOperation}");
                            context.SetupErrorInternal(exception);
                            StackBackward(stack);
                            return true;
                        }

                        StackBackward(stack);
                        return !currentAsyncOperation.isDone;

                    case IEnumerator nestedEnumerator:
                        stack.Push(new Frame(context, nestedEnumerator));
                        continue;
                }

                try
                {
                    var isMoveNext = stateMachine.MoveNext();

                    if (!isMoveNext)
                        if (context.GetStateMachine() == stateMachine && context.IsProcessing)
                            context.SetupCompletedInternal();
                        else
                        {
                            if (StackUnroll(stack.Peek(), stack))
                                return true;

                            if (context.IsProcessing)
                                context.SetupCompletedInternal();
                        }
                }
                catch (Exception e)
                {
                    context.SetupErrorInternal(e);
                }

                StackBackward(stack);
                return true;
            }

            return false;
        }

        private static bool StackUnroll(Frame frame, Stack<Frame> stack)
        {
            var stateMachine = frame.StateMachine;
            while (frame.Context.GetStateMachine() != stateMachine)
            {
                stateMachine = stack.Pop().StateMachine;
                if (stateMachine.MoveNext())
                {
                    StackBackward(stack);
                    return true;
                }
            }

            return false;
        }

        private static void StackBackward(Stack<Frame> stack)
        {
            while (stack.Count > 0)
                stack.Pop().Context.UpdatePulse();
        }

        private readonly struct Frame
        {
            public readonly Routine Context;
            public readonly IEnumerator StateMachine;

            public Frame(Routine context, IEnumerator stateMachine)
            {
                Context = context;
                StateMachine = stateMachine;
            }

            public void Deconstruct(out Routine context, out IEnumerator stateMachine)
            {
                context = Context;
                stateMachine = StateMachine;
            }
        }
    }
}