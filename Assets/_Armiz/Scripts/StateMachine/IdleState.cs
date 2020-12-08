using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
	public class IdleState : State
    {
        public IdleState() : base() { }

        public override IEnumerator Start()
        {
            Debug.Log("Idle State Started!");
            EventManager.FireEventIdleStateStart();
            yield break;
        }
    }
}