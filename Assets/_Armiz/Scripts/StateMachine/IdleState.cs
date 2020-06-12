using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
	public class IdleState : State
    {
        public IdleState(GameController _gameController) : base(_gameController) { }

        public override IEnumerator Start()
        {
            Debug.Log("Idle State Started!");
            //gameController.ChangeUIState();
            gameController.changeStateBtn.color = Color.yellow;
            gameController.changeStateBtn.transform.GetChild(0).GetComponent<Text>().text = "ATTACK!";
            gameController.addOneBtn.interactable = true;
            gameController.upgradeBtn.interactable = true;
            yield break;
        }
    }
}