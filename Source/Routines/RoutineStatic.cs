using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public partial class Routine
    {
        [NotNull]
        public static DelayRoutine Delay(float intervalSeconds)
        {
            if (intervalSeconds < 0 || float.IsNaN(intervalSeconds))
                throw new ArgumentOutOfRangeException(nameof(intervalSeconds));

            return new DelayRoutine(TimeSpan.FromSeconds(intervalSeconds));
        }

        [NotNull]
        public static DelayRoutine Delay(TimeSpan interval)
        {
            var intervalDuration = interval.Duration();

            return new DelayRoutine(intervalDuration);
        }

        [NotNull]
        public static TaskRoutine Task([NotNull] Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new TaskRoutine(action);
        }

        [NotNull]
        public static TaskRoutine Task([NotNull] Action<CancellationToken> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new TaskRoutine(action);
        }

        [NotNull]
        public static TaskRoutine<TResult> Task<TResult>([NotNull] Func<TResult> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new TaskRoutine<TResult>(action);
        }

        [NotNull]
        public static TaskRoutine<TResult> Task<TResult>([NotNull] Func<CancellationToken, TResult> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new TaskRoutine<TResult>(action);
        }

        [NotNull]
        public static GroupRoutine WhenAll([NotNull] IEnumerable<Routine> routines)
        {
            if (routines == null)
                throw new ArgumentNullException();

            return new GroupRoutine(routines);
        }

        public static Routine FromCompleted()
        {
            return new EmptyRoutine().Complete();
        }

        public static Routine<T> FromCompleted<T>(T value)
        {
            return new FromResultRoutine<T>(value).Complete();
        }

        public static Routine ByAction(Action action)
        {
            return new ActionRoutine(action);
        }

        public static Routine<T> ByAction<T>(Func<T> action)
        {
            return new ActionRoutine<T>(action);
        }

        public static Routine Empty()
        {
            return new EmptyRoutine();
        }

        [NotNull]
        public static GroupRoutine WhenAll([NotNull] params Routine[] routines)
        {
            if (routines == null)
                throw new ArgumentNullException(nameof(routines));

            return new GroupRoutine(routines);
        }

        public static Routine<T> FromResult<T>(T result) => new FromResultRoutine<T>(result);

        public static ByEnumeratorRoutine ByEnumerator(IEnumerator enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException(nameof(enumerator));

            return new ByEnumeratorRoutine(enumerator);
        }

        public static ByEnumeratorRoutine ByEnumerator(Func<RoutineControl, IEnumerator> enumeratorGetter)
        {
            if (enumeratorGetter == null)
                throw new ArgumentNullException(nameof(enumeratorGetter));

            return new ByEnumeratorRoutine(enumeratorGetter);
        }

        public static ByEnumeratorRoutine<TResult> ByEnumerator<TResult>(
            Func<RoutineControl<TResult>, IEnumerator> enumeratorGetter)
        {
            if (enumeratorGetter == null)
                throw new ArgumentNullException(nameof(enumeratorGetter));

            return new ByEnumeratorRoutine<TResult>(enumeratorGetter);
        }

        public static ByEnumeratorRoutine ByEnumerator<TArg>(Func<TArg, RoutineControl, IEnumerator> enumeratorGetter,
            TArg arg)
        {
            if (enumeratorGetter == null)
                throw new ArgumentNullException(nameof(enumeratorGetter));

            return new ByEnumeratorRoutine(e => enumeratorGetter(arg, e));
        }

        public static ByEnumeratorRoutine<TResult> ByEnumerator<TArg, TResult>(
            Func<TArg, RoutineControl<TResult>, IEnumerator> enumeratorGetter, TArg arg)
        {
            if (enumeratorGetter == null)
                throw new ArgumentNullException(nameof(enumeratorGetter));

            return new ByEnumeratorRoutine<TResult>(e => enumeratorGetter(arg, e));
        }

        public static Routine<TResult> Convert<TSource, TResult>(Routine<TSource> sourceRoutine,
            Func<TSource, TResult> converter)
        {
            return new ConvertResultRoutine<TSource, TResult>(sourceRoutine, converter);
        }

        public static Routine<TResult> WaitOne<TResult>(Routine routine, Func<TResult> resultProvider)
        {
            return new WaitRoutine<TResult>(routine, resultProvider);
        }
    }
}