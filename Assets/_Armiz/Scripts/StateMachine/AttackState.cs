using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Armiz
{
    public class AttackState : State
    {
        public AttackState(GameController _gameController) : base(_gameController) { }

        public override IEnumerator Start()
        {
            Debug.Log("Attack State Started!");

            gameController.uiPanel.SetActive(false);

            yield break;
        }

        public override IEnumerator AlliesAttack()
        {
            Debug.Log("Allies Attack!");
            gameController.shootTimer = new Timer(Time.time, gameController.shootTime);
            for (int i = 0; i < gameController.alliesPosList.Count; i++)
            {
                GameObject bulletGO = ObjectPool.Spawn(gameController.bulletPrefab, gameController.alliesPosList[i]);
                Tween thisTween = bulletGO.transform.DOMove(gameController.enemyPos, 0.7f);
                thisTween.OnComplete(() => ObjectPool.Despawn(bulletGO));
            }
            if (gameController.enemy.Damage(GameData.AllyCount * gameController.ally.GetDamage()))
            {
                gameController.EnemyDied();
            }
            gameController.SetEnemyHealthBar();
            Debug.Log("Enemy Damaged! \nEnemy health:" + gameController.enemy.GetCurrentHealth());
            yield break;
        }

        public override IEnumerator EnemiesAttack()
        {
            Debug.Log("Enemies Attack!");
            yield break;
        }
    }
}