using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Arimz;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        [SerializeField] public List<EnemyController> enemyControllers;
        [SerializeField] public List<AllyController> allyControllers;

        [Header("Editor Parameters")]
        //public GameObject uiPanel;
        public Image changeStateBtn;
        public Button addOneBtn;
        public Button upgradeBtn;
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

        void Awake()
        {
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
                    //if (attackTimer.isFinished(Time.time))
                    //{
                    //    OnAttackTimerFinished();
                    //}
                    //else
                    //{
                    //    if (shootTimer.isFinished(Time.time))
                    //    {
                    //        shootTimer = new TimerTool(Time.time, shootTime);
                    //        StartCoroutine(state.AlliesAttack());
                    //    }

                    //    if (Input.GetMouseButtonDown(0))
                    //    {
                    //        StartCoroutine(state.AlliesAttack());
                    //    }
                    //}
                    if (shootTimer.isFinished(Time.time))
                    {
                        shootTimer = new TimerTool(Time.time, shootTime);
                        StartCoroutine(state.AlliesAttack());
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(state.AlliesAttack());
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnStateChangeBtnClick()
        {
            if (gameState == GameState.Idle)
            {
                gameState = GameState.Attack;
                attackTimer = new TimerTool(Time.time, attackTime);
                SetState(new AttackState(this));
                shootTimer = new TimerTool(Time.time, shootTime);
                StartCoroutine(state.AlliesAttack());
            }
            else if (gameState == GameState.Attack)
            {
                if (gameState != GameState.Attack) return;
                gameState = GameState.Idle;
                SetState(new IdleState(this));
            }
        }
        public void OnAddAllyBtnClick()
        {
            GameData.AllyCount++;
            SpawnNewAllies();
        }
        public void OnResetProgressBtnClick()
        {
            GameData.AllyCount = 1;
            SceneManager.LoadScene(0);
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
            enemyControllers[enemyControllers.Count - 1].Hit();
        }
        private void SpawnNewAllies()
        {
            for (int i = 0; i < allyControllers.Count; i++)
            {
                ObjectPool.Despawn(allyControllers[i].gameObject);
            }
            allyControllers = new List<AllyController>();

            int allyCount = GameData.AllyCount;
            float segmentDegree = 360 / allyCount;
            int radius = (allyCount < 20) ? 2 : 5;
            for (int i = 0; i < allyCount; i++)
            {
                Vector3 newPos = new Vector3(enemyPos.x + Utility.rCos(radius, i * segmentDegree),
                                                enemyPos.y * allyPrefab.transform.localScale.y,
                                                enemyPos.z + Utility.rSin(radius, i * segmentDegree));
                allyControllers.Add(ObjectPool.Spawn(allyPrefab, newPos).GetComponent<AllyController>());
                allyControllers[i].Initialize(this, ally, bulletPrefab);
            }
        }

        public void SpawnEnemies()
        {
            //TODO: spawn multiple Enemies!
            for (int i = 0; i < enemyControllers.Count; i++)
            {
                ObjectPool.Despawn(enemyControllers[i].gameObject);
            }
            enemyControllers = new List<EnemyController>();

            GameObject enemyGO = ObjectPool.Spawn(enemyPrefab, enemyPos);
            enemyHealthBar = enemyGO.transform.GetChild(0).GetChild(1).GetComponent<Image>();
            enemyControllers.Add(enemyGO.GetComponent<EnemyController>());
            enemyControllers[enemyControllers.Count - 1].Initialize(enemy, enemyHealthBar);
            enemyControllers[enemyControllers.Count - 1].SetEnemyHealthBar();
        }

        //public void ChangeUIState()
        //{
        //    if (gameState == GameState.Idle)
        //    {
        //        changeStateBtn.color = Color.yellow;
        //        changeStateBtn.transform.GetChild(0).GetComponent<Text>().text = "ATTACK!";
        //        addOneBtn.interactable = true;
        //        upgradeBtn.interactable = true;
        //    }
        //    else if (gameState == GameState.Attack)
        //    {
        //        changeStateBtn.color = Color.red;
        //        changeStateBtn.transform.GetChild(0).GetComponent<Text>().text = "STOP!";
        //        addOneBtn.interactable = false;
        //        upgradeBtn.interactable = false;
        //    }
        //}
    }
}