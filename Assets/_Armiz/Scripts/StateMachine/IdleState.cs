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

            //UI Changes
            gameController.changeStateBtn.color = Color.yellow;
            gameController.changeStateBtn.transform.GetChild(0).GetComponent<Text>().text = "ATTACK!";
            gameController.addOneBtn.interactable = (GameData.Coin > gameController.ally.GetCost());
            gameController.upgradeBtn.interactable = (GameData.Coin > gameController.ally.GetUpgradeCost());
            gameController.coinCountTxt.text = GameData.Coin.ToString();
            yield break;
        }
    }
}