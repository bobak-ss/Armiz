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

        [Header("ScriptRefrences")]
        [SerializeField] private EnemyController enemyController;

        [Header("Editor Parameters")]
        public GameObject uiPanel;
        public Image enemyHealthBar;

        [Header("Prefabs")]
        public GameObject allyPrefab;
        public GameObject enemyPrefab;
        public GameObject bulletPrefab;

        [Header("Scriptable Objects")]
        public Fighter enemy;
        public Fighter ally;

        [HideInInspector] public TimerTool attackTimer;
        [HideInInspector] public TimerTool shootTimer;

        [HideInInspector] public List<GameObject> alliesGOList = new List<GameObject>();
        [HideInInspector] public List<GameObject> enemiesGOList = new List<GameObject>();

        void Awake()
        {
            GameData.AllyCount = 1;
            gameState = GameState.Idle;
            SetState(new IdleState(this));
            SpawnNewAllies();
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
                            shootTimer = new TimerTool(Time.time, shootTime);
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
            attackTimer = new TimerTool(Time.time, attackTime);
            SetState(new AttackState(this));
            shootTimer = new TimerTool(Time.time, shootTime);
            StartCoroutine(state.AlliesAttack());
        }
        public void OnAddAllyBtnClick()
        {
            GameData.AllyCount++;
            SpawnNewAllies();
        }
        public void OnAttackTimerFinished()
        {
            if (gameState != GameState.Attack) return;
            gameState = GameState.Idle;
            SetState(new IdleState(this));
        }

        public void EnemyDied()
        {
            Debug.Log("Enemy Died!");
            SpawnEnemies();
        }

        public void EnemyHit()
        {
            enemyController.Hit();
        }
        private void SpawnNewAllies()
        {
            for (int i = 0; i < alliesGOList.Count; i++)
            {
                ObjectPool.Despawn(alliesGOList[i]);
            }
            alliesGOList = new List<GameObject>();

            int allyCount = GameData.AllyCount;
            float segmentDegree = 360 / allyCount;
            int radius = (allyCount < 20) ? 2 : 5;
            for (int i = 0; i < allyCount; i++)
            {
                Vector3 newPos = new Vector3(enemyPos.x + Utility.rCos(radius, i * segmentDegree),
                                                enemyPos.y * allyPrefab.transform.localScale.y,
                                                enemyPos.z + Utility.rSin(radius, i * segmentDegree));
                alliesGOList.Add(ObjectPool.Spawn(allyPrefab, newPos));
            }
        }

        public void SpawnEnemies()
        {
            //TODO: spawn multiple Enemies!
            for (int i = 0; i < enemiesGOList.Count; i++)
            {
                ObjectPool.Despawn(enemiesGOList[i]);
            }
            enemiesGOList = new List<GameObject>();

            GameObject enemyGO = ObjectPool.Spawn(enemyPrefab, enemyPos);
            enemyHealthBar = enemyGO.transform.GetChild(0).GetChild(1).GetComponent<Image>();
            enemyController = enemyGO.GetComponent<EnemyController>();
            enemyController.Initialize(enemy, enemyHealthBar);
            enemyController.SetEnemyHealthBar();
            enemiesGOList.Add(enemyGO);
        }
    }
}