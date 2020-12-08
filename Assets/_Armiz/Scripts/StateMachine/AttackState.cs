using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
    public class AttackState : State
    {
        public AttackState() : base() { }
        
        public override IEnumerator Start()
        {
            Debug.Log("Attack State Started!");
            EventManager.FireEventAttackStateStart();
            yield break;
        }

        public override IEnumerator AlliesAttack()
        {
            Debug.Log("Allies Attack!");
            EventManager.FireEventAlliesAttack();
            
            // for (int i = 0; i < gameController.allyFighterControllers.Count; i++)
            //     gameController.allyFighterControllers[i].FireProjectileTo(gameController.enemyPos);
            yield break;
        }

        public override IEnumerator EnemiesAttack()
        {
            Debug.Log("Enemies Attack");
            EventManager.FireEventEnemiesAttack();
            
            // if (gameController.allyFighterControllers.Count <= 0) yield break;
            // var targetedAllyToHit = gameController.allyFighterControllers[UnityEngine.Random.Range(0, gameController.allyFighterControllers.Count - 1)];
            // gameController.enemyFighterControllers[0].FireProjectileTo(targetedAllyToHit.transform.position);
            yield break;
        }
    }
}