using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView: MonoBehaviour
{
    GameController gameController;
    GameModel gameModel;

    [Header("Game Prameters")]
    public float attackTime;

    [Header("Editor Prameters")]
    public GameObject uiPanel;

    private Timer attackTimer;
    
    void Awake()
    {
        gameModel = new GameModel();
        gameController = new GameController();
        gameController.Setup(gameModel);
        gameController.StartGame();
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

    public void Log(string s)
    {
        Debug.Log(s);
    }
}
