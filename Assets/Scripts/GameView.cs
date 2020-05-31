using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView: MonoBehaviour
{
    GameController gameController;
    GameModel gameModel;

    [Header("Game Prameters")]
    public float attackTime;
    public Vector3 enemyPos;

    [Header("Editor Prameters")]
    public GameObject uiPanel;

    [Header("Prefabs")]
    public GameObject allyPrefab;
    public GameObject enemyPrefab;

    private Timer attackTimer;
    
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
                    // attacking stuff!
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
    }

    public void OnAttackTimerFinished()
    {
        uiPanel.SetActive(true);
        gameController.EndAttack();
    }

    public void SpawnAllies()
    {
        float segmentDegree = 360 / gameModel.allyCount;
        int radius = (gameModel.allyCount < 20) ? 2 : 5;
        for (int i = 0; i < gameModel.allyCount; i++)
        {
            ObjectPool.Spawn(allyPrefab, new Vector3(enemyPos.x + Utility.rCos(radius, i * segmentDegree),
                                                    enemyPos.y * allyPrefab.transform.localScale.y,
                                                    enemyPos.z + Utility.rSin(radius, i * segmentDegree))
                            );
        }
    }

    public void SpawnEnemies()
    {
        //TODO: spawn multiple enemies!
        ObjectPool.Spawn(enemyPrefab, enemyPos);
    }

    public void Log(string s)
    {
        Debug.Log(s);
    }
}
