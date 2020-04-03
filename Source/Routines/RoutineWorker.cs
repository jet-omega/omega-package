using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Package;
using Omega.Text;
using Omega.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Omega.Package.Logger;
using Object = UnityEngine.Object;

namespace Omega.Routines
{
    public sealed class RoutineExecutionHandler
    {
        public readonly Routine Routine;
        public readonly RoutineExecutionScope Scope;
        public readonly ExecutionOrder ExecutionOrder;

        public void Stop() => RoutineWorkerHub.Stop(this);

        public RoutineExecutionHandler(Routine routine, RoutineExecutionScope scope, ExecutionOrder executionOrder)
        {
            Routine = routine;
            Scope = scope;
            ExecutionOrder = executionOrder;
        }
    }

    internal static class RoutineWorkerHub
    {
        private readonly static RichTextFactory RichTextFactory = new RichTextFactory(60);

        public readonly static Logger Logger =
            Routine.Logger.CreateSubLogger("WORKER▸", new Color32(0xFF, 0xB5, 0x80, 0xFF), FontStyle.Bold);

        private static RoutineWorker _routineSceneWorker;
        private static RoutineWorker _routinePermanentWorker;

        private static IRoutineWorker GetSceneWorker()
        {
            if (!_routineSceneWorker || !_routineSceneWorker.gameObject)
            {
                Debug.Log("imh");
                _routineSceneWorker = GameObjectFactory.New()
                    .SetName("[ROUTINE] SCENE WORKER")
                    .AddComponent<RoutineWorker>()
                    .Build<RoutineWorker>();

                Debug.Log("wtrff");
                var message = RichTextFactory.UnstyledText("Scene worker was created on scene: ")
                    .Default.Bold.Text(SceneManager.GetActiveScene().name).ToString(true);

                _routineSceneWorker.gameObject.hideFlags = HideFlags.HideInHierarchy;

                Logger.Log(message);
            }

            return _routineSceneWorker;
        }

        private static IRoutineWorker GetPermanentWorker()
        {
            if (!_routinePermanentWorker || !_routinePermanentWorker.gameObject)
            {
                _routinePermanentWorker = GameObjectFactory.New()
                    .SetName("[ROUTINE] PERMANENT WORKER")
                    .AddComponent<RoutineWorker>()
                    .Build<RoutineWorker>();

                Object.DontDestroyOnLoad(_routinePermanentWorker.gameObject);
                _routinePermanentWorker.gameObject.hideFlags = HideFlags.HideInHierarchy;

                Logger.Log("Permanent worker was created");
            }

            return _routinePermanentWorker;
        }

        public static RoutineExecutionHandler Add(Routine routine, RoutineExecutionScope executionScope,
            ExecutionOrder executionOrder,
            bool prelude)
        {
            // if (prelude && !RoutineUtilities.OneStep(routine))
            //     return new RoutineExecutionHandler(routine, executionScope, executionOrder);

            var worker = executionScope == RoutineExecutionScope.Scene
                ? GetSceneWorker()
                : GetPermanentWorker();

            worker.Add(routine, executionOrder);

            return new RoutineExecutionHandler(routine, executionScope, executionOrder);
        }

        public static void Stop(RoutineExecutionHandler handler)
        {
            var worker = handler.Scope == RoutineExecutionScope.Scene
                ? GetSceneWorker()
                : GetPermanentWorker();

            worker.Stop(handler.Routine, handler.ExecutionOrder);
        }
    }

    internal interface IRoutineWorker
    {
        void Add(Routine routine, ExecutionOrder executionOrder);
        void Stop(Routine routine, ExecutionOrder executionOrder);
    }

    internal sealed class RoutineWorker : MonoBehaviour, IRoutineWorker
    {
        private RoutineWorkerContainer _container;

        public void Add(Routine routine, ExecutionOrder executionOrder)
        {
            _container.Add(routine, executionOrder);
        }

        public void Stop(Routine routine, ExecutionOrder executionOrder)
        {
            switch (executionOrder)
            {
                case ExecutionOrder.Update:
                    _container.UpdateRoutines.Remove(routine);
                    break;
                case ExecutionOrder.LateUpdate:
                    _container.LateUpdateRoutines.Remove(routine);
                    break;
                case ExecutionOrder.FixedUpdate:
                    _container.FixedUpdateRoutines.Remove(routine);
                    break;
            }
        }

        private void Awake()
        {
            _container = new RoutineWorkerContainer();
        }

        private static void UpdateRoutines(List<Routine> routines)
        {
            using (ListPool<int>.Get(out var removeIndexes))
            {
                for (var i = 0; i < routines.Count; i++)
                {
                    var routineEnumerator = (IEnumerator) routines[i];
                    if (!routineEnumerator.MoveNext())
                        removeIndexes.Add(i);
                }

                // Удалять элементы с конца проще
                removeIndexes.Reverse();
                foreach (var index in removeIndexes)
                    routines.RemoveAt(index);
            }
        }

        private void Update() => UpdateRoutines(_container.UpdateRoutines);
        private void FixedUpdate() => UpdateRoutines(_container.FixedUpdateRoutines);
        private void LateUpdate() => UpdateRoutines(_container.LateUpdateRoutines);
    }

    internal class RoutineWorkerContainer
    {
        public readonly List<Routine> LateUpdateRoutines;
        public readonly List<Routine> UpdateRoutines;
        public readonly List<Routine> FixedUpdateRoutines;

        public RoutineWorkerContainer()
        {
            LateUpdateRoutines = new List<Routine>();
            UpdateRoutines = new List<Routine>();
            FixedUpdateRoutines = new List<Routine>();
        }

        public void Add(Routine routine, ExecutionOrder order)
        {
            switch (order)
            {
                case ExecutionOrder.LateUpdate:
                    LateUpdateRoutines.Add(routine);
                    break;
                case ExecutionOrder.Update:
                    UpdateRoutines.Add(routine);
                    break;
                case ExecutionOrder.FixedUpdate:
                    FixedUpdateRoutines.Add(routine);
                    break;
            }
        }
    }

    public enum ExecutionOrder
    {
        LateUpdate,
        Update,
        FixedUpdate,
    }

    public enum RoutineExecutionScope
    {
        Scene = 0,
        Permanent = 1
    }
}