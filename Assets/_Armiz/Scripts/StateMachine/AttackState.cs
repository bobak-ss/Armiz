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
            gameController.ChangeUIState();

            yield break;
        }

        public override IEnumerator AlliesAttack()
        {
            Debug.Log("Allies Attack!");
            gameController.shootTimer = new TimerTool(Time.time, gameController.shootTime);
            for (int i = 0; i < gameController.alliesGOList.Count; i++)
            {
                GameObject bulletGO = ObjectPool.Spawn(gameController.bulletPrefab, gameController.alliesGOList[i].transform.position);
                Tween thisTween = bulletGO.transform.DOMove(gameController.enemyPos, 0.7f);
                thisTween.OnComplete(() => {
                    ObjectPool.Despawn(bulletGO);
                    //gameController.EnemyHit();
                });
            }
            if (gameController.enemy.Damage(GameData.AllyCount * gameController.ally.GetDamage()))
            {
                gameController.EnemyDied();
            }
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