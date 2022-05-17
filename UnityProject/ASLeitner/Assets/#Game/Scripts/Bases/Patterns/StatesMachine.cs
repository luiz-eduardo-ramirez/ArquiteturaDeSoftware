using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions.Patterns
{
    public class StatesMachine<Enum, StatesType> where StatesType : StateBase where Enum : IFormattable, IConvertible, IComparable //todo enum implementa essas interfaces;
    {
        public delegate int ConvertTypeToInt(Enum _toConvert);
        
        private StatesType[] m_states;
        private ConvertTypeToInt m_typeToIntConverter;

        private Enum m_currentState;
        
        public Enum CurrentState {
            get { return m_currentState; }
            set {
                m_states[m_typeToIntConverter(m_currentState)].OnStateExit();
                m_currentState = value;
                m_states[m_typeToIntConverter(m_currentState)].OnStateEnter();
            }
        }
        public StatesType GetState { get { return m_states[m_typeToIntConverter(CurrentState)]; } }
        
        public StatesMachine(StatesType[] _states, ConvertTypeToInt _typeIntConverter)
        {
            m_states = _states;
            m_typeToIntConverter = _typeIntConverter;
        }
    }

    public abstract class StateBase
    {
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
    }
}