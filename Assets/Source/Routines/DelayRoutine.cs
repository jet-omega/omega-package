using System.Collections;
using UnityEngine;

namespace Omega.Routines
{
    public sealed class DelayRoutine : Routine
    {
        private readonly float _delayInterval;
        public float DelayTime => _delayInterval;

        private float _releaseTime;
        
        public DelayRoutine(float delayInterval)
        {
            _delayInterval = delayInterval;
        }

        protected override IEnumerator RoutineUpdate()
        {
            _releaseTime = Time.unscaledTime + _delayInterval;

            while (Time.unscaledTime < _releaseTime)
                yield return null;
        }
    }
}