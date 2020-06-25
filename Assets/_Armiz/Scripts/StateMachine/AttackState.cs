using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
    public class AttackState : State
    {
        public AttackState(GameController _gameController) : base(_gameController) { }

        public override IEnumerator Start()
        {
            Debug.Log("Attack State Started!");
            
            // UI changes
            gameController.changeStateBtn.color = Color.red;
            gameController.changeStateBtn.transform.GetChild(0).GetComponent<Text>().text = "STOP!";
            gameController.addOneBtn.interactable = false;
            gameController.upgradeBtn.interactable = false;
            gameController.coinCountTxt.text = GameData.Coin.ToString();

            yield break;
        }

        public override IEnumerator AlliesAttack()
        {
            // UI changes
            gameController.coinCountTxt.text = GameData.Coin.ToString();

            Debug.Log("Allies Attack!");
            for (int i = 0; i < gameController.allyControllers.Count; i++)
            {
                gameController.allyControllers[i].FireProjectile();
            }

            if (gameController.enemy.Damage(GameData.AllyCount * gameController.ally.GetDamage()))
            {
                gameController.EnemyDied();
            }
            yield break;
        }

        public override IEnumerator EnemiesAttack()
        {
            Debug.Log("Enemies Attack!");
            gameController.enemyControllers[0].FireProjectileTo(gameController.allyControllers[UnityEngine.Random.Range(0, gameController.allyControllers.Count - 1)].transform.position);
            yield break;
        }
    }
}