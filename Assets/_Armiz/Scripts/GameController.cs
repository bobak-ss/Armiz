using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Arimz;
using System;
using UnityEngine.UI;

namespace Armiz
{
    public class GameController : StateMachine
    {
        private GameState gameState;

        [Header("Game Prameters")]
        public float attackTime;
        public float shootTime;
        public Vector3 enemyPos;

        [Header("Editor Prameters")]
        public GameObject uiPanel;
        public Image enemyHealthBar;

        [Header("Prefabs")]
        public GameObject allyPrefab;

        public GameObject enemyPrefab;
        public GameObject bulletPrefab;

        [HideInInspector] public Timer attackTimer;
        [HideInInspector] public Timer shootTimer;
        [HideInInspector] public List<Vector3> alliesPosList;

        public Fighter enemy;
        public Fighter ally;

        void Awake()
        {
            GameData.AllyCount = 1;
            gameState = GameState.Idle;
            SetState(new IdleState(this));
            SpawnAllies();
            SpawnEnemies();
        }

        void Start()
        {

        }

        void Update()
        {
            switch (gameState)
            {
                case GameState.Idle:
                    // upgrade allies!
                    break;
                case GameState.Attack:
                    if (attackTimer.isFinished(Time.time))
                    {
                        OnAttackTimerFinished();
                    }
                    else
                    {
                        if (shootTimer.isFinished(Time.time))
                        {
                            shootTimer = new Timer(Time.time, shootTime);
                            StartCoroutine(state.AlliesAttack());
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            StartCoroutine(state.AlliesAttack());
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnAttackBtnClick()
        {
            if (gameState != GameState.Idle) return;
            gameState = GameState.Attack;
            attackTimer = new Timer(Time.time, attackTime);
            SetState(new AttackState(this));
            shootTimer = new Timer(Time.time, shootTime);
            StartCoroutine(state.AlliesAttack());
        }

        public void OnAttackTimerFinished()
        {
            if (gameState != GameState.Attack) return;
            gameState = GameState.Idle;
            SetState(new IdleState(this));
        }

        internal void EnemyDied()
        {
            Debug.Log("Enemy Died!");
            enemy.ResetHealth();
        }

        public void SetEnemyHealthBar()
        {
            if (enemyHealthBar == null) return;
            enemyHealthBar.fillAmount = (enemy.GetCurrentHealth() / enemy.GetTotalHealth());
            Debug.Log("SET HEALTH ENEMY!");
        }

        public void SpawnAllies()
        {
            // TODO: despawn prevoiusly generated Allies
            alliesPosList = new List<Vector3>();
            int allyCount = GameData.AllyCount;
            float segmentDegree = 360 / allyCount;
            int radius = (allyCount < 20) ? 2 : 5;
            for (int i = 0; i < allyCount; i++)
            {
                alliesPosList.Add(new Vector3(enemyPos.x + Utility.rCos(radius, i * segmentDegree),
                                                enemyPos.y * allyPrefab.transform.localScale.y,
                                                enemyPos.z + Utility.rSin(radius, i * segmentDegree)));
                ObjectPool.Spawn(allyPrefab, alliesPosList[i]);
            }
        }

        public void SpawnEnemies()
        {
            //TODO: spawn multiple Enemies!
            //TODO: despawn prevoiusly generated Enemies
            GameObject go = ObjectPool.Spawn(enemyPrefab, enemyPos);

            enemyHealthBar = go.transform.GetChild(0).GetChild(1).GetComponent<Image>();
            SetEnemyHealthBar();
        }
    }
}