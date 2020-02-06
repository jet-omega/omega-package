using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Omega.Routines
{
    public sealed class DelayRoutine : Routine, IProgressRoutineProvider
    {
        private const int ForceCompleteLatency = 20;
        
        private readonly TimeSpan _delayInterval;
        private DateTime _releaseTimeSeconds;

        public TimeSpan DelayInterval => _delayInterval;

        internal DelayRoutine(TimeSpan delayInterval)
        {
            _delayInterval = delayInterval;
        }

        protected override IEnumerator RoutineUpdate()
        {
            _releaseTimeSeconds = DateTime.UtcNow + _delayInterval;
            // Если рутину завершают синхронно то ждать в while`е пока пройдет заданный интервал не эффективно
            // Так как это полностью занимает поток на все время задержки
            // Поэтому чтобы не занимать поток мы отправляем его в состояние сна
            // Если интервал короче ForceCompleteLatency мс то точности встроенного таймера может не хватить
            if (IsForcedProcessing)
            {
                // Спим частями по ForceCompleteLatency, чтобы дать возможность вызывающему коду прервать 
                // операцию по тайм ауту
                var preReleaseTimeSeconds = _releaseTimeSeconds.Subtract(TimeSpan.FromMilliseconds(ForceCompleteLatency));
                while (DateTime.UtcNow < preReleaseTimeSeconds)
                    Thread.Sleep(ForceCompleteLatency);
                
                // Однако Thread.Sleep может усыпить поток на чуть меньшее время чем было указано
                // Поэтому после Thread.Sleep имеет смысл не долго подолбиться в while`е
            }

            while (DateTime.UtcNow < _releaseTimeSeconds)
                yield return null;
        }

        public float GetProgress()
        {
            var startedIn = _releaseTimeSeconds - _delayInterval;
            var delta = DateTime.UtcNow - startedIn;

            var unclampedProgress = (float) (delta.TotalSeconds / _delayInterval.TotalSeconds);

            return Mathf.Clamp01(unclampedProgress);
        }

        protected override void OnForcedComplete()
        {
            Debug.LogWarning("You force complete delay routine, its lock-thread operation");
            base.OnForcedComplete();
        }
    }
}