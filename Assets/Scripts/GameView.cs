using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameView: MonoBehaviour
{
    GameController gameController;
    GameModel gameModel;

    [Header("Game Prameters")]
    public float attackTime;
    public float shootTime;
    public Vector3 enemyPos;

    [Header("Editor Prameters")]
    public GameObject uiPanel;

    [Header("Prefabs")]
    public GameObject allyPrefab;
    public GameObject enemyPrefab;
    public GameObject bulletPrefab;

    private Timer attackTimer;
    private Timer shootTimer;
    private List<Vector3> alliesPosList;
    
    void Awake()
    {
        gameModel = new GameModel();
        gameController = new GameController();
        gameController.Setup(gameModel);

        gameController.StartGame();
        SpawnAllies();
        SpawnEnemies();
    }

    void Start()
    {

    }

    void Update()
    {
        switch (gameModel.state)
        {
            case GameModel.GameState.Attack:
                if (attackTimer.isFinished(Time.time))
                {
                    OnAttackTimerFinished();
                }
                else
                {
                    if (shootTimer.isFinished(Time.time))
                    {
                        shootTimer = new Timer(Time.time, shootTime);
                        ShootBulletsAllies();
                    }
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        ShootBulletsAllies();
                    }
                }
                break;
            case GameModel.GameState.Idle:
                // upgrade allies!
                break;
            default:
                break;
        }
    }

    public void OnAttackBtnClick()
    {
        uiPanel.SetActive(false);

        attackTimer = new Timer(Time.time, attackTime);
        gameController.StartAttack();

        shootTimer = new Timer(Time.time, shootTime);
        ShootBulletsAllies();
    }

    public void OnAttackTimerFinished()
    {
        uiPanel.SetActive(true);
        gameController.EndAttack();
    }

    public void SpawnAllies()
    {
        alliesPosList = new List<Vector3>();
        float segmentDegree = 360 / gameModel.allyCount;
        int radius = (gameModel.allyCount < 20) ? 2 : 5;
        for (int i = 0; i < gameModel.allyCount; i++)
        {
            alliesPosList.Add(new Vector3(enemyPos.x + Utility.rCos(radius, i * segmentDegree),
                                            enemyPos.y * allyPrefab.transform.localScale.y,
                                            enemyPos.z + Utility.rSin(radius, i * segmentDegree)));
            ObjectPool.Spawn(allyPrefab, alliesPosList[i]);
        }
    }

    public void SpawnEnemies()
    {
        //TODO: spawn multiple enemies!
        ObjectPool.Spawn(enemyPrefab, enemyPos);
    }

    public void ShootBulletsAllies()
    {
        for (int i = 0; i < alliesPosList.Count; i++)
        {
            GameObject bulletGO = ObjectPool.Spawn(bulletPrefab, alliesPosList[i]);
            Tween thisTween = bulletGO.transform.DOMove(enemyPos, 0.7f);
            thisTween.OnComplete(() => ObjectPool.Despawn(bulletGO));
        }
    }
}
