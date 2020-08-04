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
        public float alliesAttackTime;
        public float enemiesAttackTime;
        public Vector3 enemyPos;

        [Header("ScriptRefrences")]
        //[SerializeField] public List<EnemyController> enemyControllers;
        //[SerializeField] public List<AllyController> allyControllers;
        [SerializeField] public List<FighterController> allyFighterControllers;
        [SerializeField] public List<FighterController> enemyFighterControllers;

        [Header("Editor Parameters")]
        //public GameObject uiPanel;
        public Image changeStateBtn;
        public Button addOneBtn;
        public Button upgradeBtn;
        public Text coinCountTxt;

        [Header("Prefabs")]
        public GameObject allyPrefab;
        public GameObject enemyPrefab;
        public GameObject allyBulletPrefab;
        public GameObject enemyBulletPrefab;

        [Header("Scriptable Objects")]
        public Fighter enemy;
        public Fighter ally;

        [HideInInspector] public TimerTool alliesAttackTimer;
        [HideInInspector] public TimerTool enemyAttackTimer;

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
                    if (enemyAttackTimer.isFinished(Time.time))
                    {
                        enemyAttackTimer = new TimerTool(Time.time, enemiesAttackTime);
                        StartCoroutine(state.EnemiesAttack());
                    }
                    if (alliesAttackTimer.isFinished(Time.time))
                    {
                        alliesAttackTimer = new TimerTool(Time.time, alliesAttackTime);
                        StartCoroutine(state.AlliesAttack());
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(state.AlliesAttack());
                    }

                    //if (Input.GetKey(KeyCode.F))
                    //    StartCoroutine(state.EnemiesAttack());
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
                SetState(new AttackState(this));
                alliesAttackTimer = new TimerTool(Time.time, alliesAttackTime);
                enemyAttackTimer = new TimerTool(Time.time, enemiesAttackTime);
                StartCoroutine(state.AlliesAttack());
            }
            else if (gameState == GameState.Attack)
            {
                gameState = GameState.Idle;
                SetState(new IdleState(this));
            }
        }
        public void OnAddAllyBtnClick()
        {
            //if (GameData.Coin < ally.GetCost()) return;

            GameData.Coin -= ally.GetCost();
            GameData.AllyCount++;

            allyFighterControllers.Add(ObjectPool.Spawn(allyPrefab, Vector3.zero).GetComponent<FighterController>());
            allyFighterControllers[GameData.AllyCount - 1].Initialize(this, ally, allyBulletPrefab);

            SetAlliesPositions(GameData.AllyCount);
        }
        public void OnLevelUpAllyBtnClick()
        {
            if (allyFighterControllers.Count <= 0) return;
            if (GameData.Coin < ally.GetUpgradeCost()) return;

            int minLevel = 10000;
            for (int i = 0; i < allyFighterControllers.Count; i++)
            {
                if (allyFighterControllers[i].GetLevel() < minLevel)
                {
                    minLevel = allyFighterControllers[i].GetLevel();
                }
            }

            List<FighterController> minLevelAllies = new List<FighterController>();
            for (int i = 0; i < allyFighterControllers.Count; i++)
            {
                if (allyFighterControllers[i].GetLevel() == minLevel)
                {
                    minLevelAllies.Add(allyFighterControllers[i]);
                }
            }

            var allyToLevelUp = minLevelAllies[UnityEngine.Random.Range(0, minLevelAllies.Count - 1)];
            allyToLevelUp.LevelUp();
        }
        public void OnResetProgressBtnClick()
        {
            GameData.AllyCount = 1;
            GameData.Coin = 0;
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
            GameData.Coin += enemy.GetBountyValue();
            enemy.LevelUp();
            // enemy coin get animation
            SpawnEnemies();
        }

        private void DespawnAllAllies()
        {
            for (int i = 0; i < allyFighterControllers.Count; i++)
            {
                ObjectPool.Despawn(allyFighterControllers[i].gameObject);
            }
            allyFighterControllers = new List<FighterController>();
        }
        private void SpawnNewAllies()
        {
            DespawnAllAllies();

            int allyCount = GameData.AllyCount;

            if (allyCount <= 0)
            {
                GameData.AllyCount = 1;
                allyCount = 1;
            }

            for (int i = 0; i < allyCount; i++)
            {
                allyFighterControllers.Add(ObjectPool.Spawn(allyPrefab, Vector3.zero).GetComponent<FighterController>());
            }

            SetAlliesPositions(allyCount);
        }
        private void SetAlliesPositions(int allyCount)
        {
            float segmentDegree = 360 / allyCount;
            int radius = (allyCount < 20) ? 2 : 5;
            for (int i = 0; i < allyCount; i++)
            {
                allyFighterControllers[i].transform.position = new Vector3(enemyPos.x + Utility.rCos(radius, i * segmentDegree),
                                                                    enemyPos.y * allyPrefab.transform.localScale.y,
                                                                    enemyPos.z + Utility.rSin(radius, i * segmentDegree));
                allyFighterControllers[i].Initialize(this, ally, allyBulletPrefab);
            }
        }

        public void SpawnEnemies()
        {
            //TODO: spawn multiple Enemies!
            for (int i = 0; i < enemyFighterControllers.Count; i++)
            {
                ObjectPool.Despawn(enemyFighterControllers[i].gameObject);
            }
            enemyFighterControllers = new List<FighterController>();

            GameObject enemyGO = ObjectPool.Spawn(enemyPrefab, enemyPos);
            enemyFighterControllers.Add(enemyGO.GetComponent<FighterController>());
            enemyFighterControllers[enemyFighterControllers.Count - 1].Initialize(this, enemy, enemyBulletPrefab);
            enemyFighterControllers[enemyFighterControllers.Count - 1].SetHealthBar();
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