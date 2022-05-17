using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions.Patterns
{
    public class SimpleStateMachine<Enum> where Enum : IFormattable, IConvertible, IComparable
    {
        public delegate int ConvertTypeToInt(Enum _toConvert);
        private Action[] m_statesMethods;
        private Action<Enum> m_onStateEnter;
        private Action<Enum> m_onStateExit;
        private ConvertTypeToInt m_typeToIntConverter;

        private Enum m_currentState;
        private bool m_callOnEnterAndOnExit;
        
        public Enum CurrentState
        {
            get { return m_currentState; }
            set
            {
                if (!m_callOnEnterAndOnExit)
                {
                    m_currentState = value;
                    return;
                }
                m_onStateExit(m_currentState);
                m_currentState = value;
                m_onStateEnter(m_currentState);
            }
        }

        public SimpleStateMachine(Action[] _statesMethods, ConvertTypeToInt _typeIntConverter)
        {
            m_statesMethods = _statesMethods;
            m_typeToIntConverter = _typeIntConverter;
            m_callOnEnterAndOnExit = false;
        }
        public SimpleStateMachine(Action[] _statesMethods, Action<Enum> _onStateEnter, Action<Enum> _onStateExit, ConvertTypeToInt _typeIntConverter)
        {
            m_statesMethods = _statesMethods;
            m_typeToIntConverter = _typeIntConverter;
            m_onStateEnter = _onStateEnter;
            m_onStateExit = _onStateExit;

            m_callOnEnterAndOnExit = true;
        }

        public void CallCurrentState()
        {
            m_statesMethods[m_typeToIntConverter(CurrentState)]();
        }
    }
}

