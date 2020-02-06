using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Omega.Routines.HighLoad
{
    public class ApportionedRoutine : Routine, IProgressRoutineProvider
    {
        private Stopwatch _stopwatch;

        private ApportionedRoutineDelegate _actionWork;
        private int _workCount;
        private int _currentWork;
        private int _workPerMeasure;
        private int _currentFrame;
        private float _limitPerUpdate;

        public ApportionedRoutine(ApportionedRoutineDelegate actionWork, int workCount, float limitPerUpdate,
            int workPerMeasure)
        {
            _stopwatch = new Stopwatch();
            _actionWork = actionWork;
            _workCount = workCount;
            _workPerMeasure = workPerMeasure;
            _limitPerUpdate = limitPerUpdate;
        }

        protected override IEnumerator RoutineUpdate()
        {
            for (_currentWork = 0; _currentWork < _workCount;)
            {
                _currentFrame = Time.frameCount;

                _stopwatch.Restart();
                while (_stopwatch.Elapsed.TotalSeconds < _limitPerUpdate)
                {
                    var iteration = Mathf.Min(_workPerMeasure, (_workCount - _currentWork));
                    if (iteration <= 0)
                        yield break;

                    for (int i = 0; i < iteration; i++)
                        _actionWork.Invoke(_currentWork++);
                }

                _stopwatch.Stop();

                yield return new WaitUntil(() => _currentFrame != Time.frameCount || IsForcedProcessing);
            }
        }

        public float GetProgress()
        {
            return _currentWork / (float) _workCount;
        }
    }
}