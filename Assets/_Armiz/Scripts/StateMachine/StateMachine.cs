using System;
using UnityEngine;

namespace Armiz
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State state;

        public void SetState(State _state)
        {
            state = _state;
            StartCoroutine(state.Start());
        }
    }
}

