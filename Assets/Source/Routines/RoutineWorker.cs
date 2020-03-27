using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Package;
using UnityEngine;

namespace Omega.Routines
{
    internal sealed class RoutineWorker : MonoBehaviour
    {
        private static RoutineWorker _instance;
        private static RoutineWorkerContainer _globalRoutines = new RoutineWorkerContainer();
        private RoutineWorkerContainer _sceneRoutines;

        internal static RoutineWorker Instance
        {
            get
            {
                if (!_instance)
                {
                    var go = new GameObject($"[{nameof(RoutineWorker)}]");
                    _instance = go.AddComponent<RoutineWorker>();
                }

                return _instance;
            }
        }

        internal void Add(Routine routine, RoutineExecutionScope scope, ExecutionOrder executionOrder)
        {
            var container = scope == RoutineExecutionScope.Scene
                ? _sceneRoutines
                : _globalRoutines;
            
            container.Add(routine, executionOrder);
        }
        
        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Debug.LogError("RoutineWorker already exist on scene");
                DestroyImmediate(this);
                return;
            }

            _sceneRoutines = new RoutineWorkerContainer();
            _instance = this;
        }

        private void UpdateRoutines(List<Routine> routines)
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

        private void Update()
        {
            UpdateRoutines(_sceneRoutines.UpdateRoutines);
            UpdateRoutines(_globalRoutines.UpdateRoutines);
        }

        private void FixedUpdate()
        {
            UpdateRoutines(_sceneRoutines.FixedUpdateRoutines);
            UpdateRoutines(_globalRoutines.FixedUpdateRoutines);
        }

        private void LateUpdate()
        {
            UpdateRoutines(_sceneRoutines.LateUpdateRoutines);
            UpdateRoutines(_globalRoutines.LateUpdateRoutines);
        }
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
#pragma warning disable 612
                case ExecutionOrder.EndOfFrame:
#pragma warning restore 612
                    throw new NotSupportedException();
            }
        }
    }

    public enum ExecutionOrder
    {
        LateUpdate,
        Update,
        FixedUpdate,
        [Obsolete]
        EndOfFrame,
    }

    public enum RoutineExecutionScope
    {
        Scene,
        Global
    }
}