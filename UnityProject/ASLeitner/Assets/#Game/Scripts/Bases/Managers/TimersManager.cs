using Base.Extensions.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Managers
{
    public partial class TimersManager
    {
        private abstract class TimerBase
        {
            protected TimersManager m_parentManager;
                
            protected bool m_done;

            public Action ActionToCall { get; private set; }


            public TimerBase(TimersManager _parentManager, Action _actionToCall)
            {
                m_parentManager = _parentManager;

                ActionToCall = _actionToCall;
                m_done = false;
            }

            public abstract bool TickTimer();
        }

        private class TimeBasedTimer : TimerBase
        {
            private float m_timeToWait;
            private float m_elapsedTime;
            private bool m_useUnscaledDeltaTime;
            private bool m_abortTimer;

            public TimeBasedTimer(TimersManager _parentManager, Action _actionToCall, float _timeToWait, bool _useUnscaledTime = false) : base(_parentManager, _actionToCall)
            {
                m_timeToWait = _timeToWait;
                m_elapsedTime = 0;
                m_useUnscaledDeltaTime = _useUnscaledTime;
                m_abortTimer = false;
            }

            public override bool TickTimer()
            {

                if (m_done) { print("Timer já foi executado uma vez!!!!!"); return m_done; }
                if (m_elapsedTime >= m_timeToWait)
                {
                    if(!m_abortTimer) ActionToCall();
                    m_done = true;
                }

                m_elapsedTime += m_useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;

                return m_done;
            }
            public void CancelTimer() { m_abortTimer = true; }
        }
        private class ConditionBasedTimer : TimerBase
        {
            private ConditionCheck m_conditionCheck;
            public ConditionBasedTimer(TimersManager _parentManager, Action _actionToCall, ConditionCheck _conditionCheck) : base(_parentManager, _actionToCall)
            {
                m_conditionCheck = _conditionCheck;
            }

            public override bool TickTimer()
            {
                if (m_done) { print("Timer já foi executado uma vez!!!!!"); return m_done; }
                if (m_conditionCheck())
                {
                    ActionToCall();
                    m_done = true;
                }
                return m_done;
            }
        }
        private class FrameBasedTimer : TimerBase
        {
            private float m_framesToWait;
            private float m_elapsedFrames;

            public FrameBasedTimer(TimersManager _parentManager, Action _actionToCall, float _framesToWait) : base(_parentManager, _actionToCall)
            {
                m_framesToWait = _framesToWait;
                m_elapsedFrames = 0;
            }

            public override bool TickTimer()
            {

                if (m_done) { print("Timer já foi executado uma vez!!!!!"); return m_done; }
                if (m_elapsedFrames >= m_framesToWait)
                {
                    ActionToCall();
                    m_done = true;
                }
                m_elapsedFrames++;

                return m_done;
            }
        }
    }
    public partial class TimersManager : MonoSingleton<TimersManager>
    {
        public delegate bool ConditionCheck();

        private List<TimerBase> m_timers;
        protected override void Awake()
        {
            base.Awake();
            m_timers = new List<TimerBase>();
        }

        // Update is called once per frame
        void Update()
        {
            List<TimerBase> finishedTimers = new List<TimerBase>();

            for(int i = 0; i < m_timers.Count; i++)
            {
                if (m_timers[i].TickTimer()) finishedTimers.Add(m_timers[i]);
            }

            foreach (TimerBase finishedTimer in finishedTimers)
            {
                m_timers.Remove(finishedTimer);
            }
        }

        public static Action CallAfterTime(Action _actionToCall, float _timeToWait, bool _useUnScaledTime = false)
        {
            TimeBasedTimer timer = new TimeBasedTimer(Instance, _actionToCall, _timeToWait, _useUnScaledTime);
            Instance.m_timers.Add(timer);
            return timer.CancelTimer;
        }
        public static void CallAfterFrames(Action _actionToCall, float _framesToWait)
        {
            Instance.m_timers.Add(new FrameBasedTimer(Instance, _actionToCall, _framesToWait));
        }
        public static void CallAfterConditionIsTrue(Action _actionToCall, ConditionCheck _condition)
        {
            Instance.m_timers.Add(new ConditionBasedTimer(Instance, _actionToCall, _condition));
        }
    }

}
