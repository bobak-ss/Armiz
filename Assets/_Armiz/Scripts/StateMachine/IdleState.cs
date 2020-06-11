using System;
using System.Collections;
using UnityEngine;

namespace Armiz
{
	public class IdleState : State
    {
        public IdleState(GameController _gameController) : base(_gameController) { }

        public override IEnumerator Start()
        {
            Debug.Log("Idle State Started!");
            gameController.ChangeUIState();
            yield break;
        }
    }
}