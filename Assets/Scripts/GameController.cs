using System;
using System.Diagnostics;

public class GameController
{
    public GameModel gameModel;

    public virtual void Setup(GameModel _gameModel)
    {
        gameModel = _gameModel;
    }

    public void StartGame()
    {
        // start game with idle state
        gameModel.state = GameModel.GameState.Idle;
    }

    public void StartAttack()
    {
        // attack
        gameModel.state = GameModel.GameState.Attack;
    }

    public void EndAttack()
    {
        // end attack
        gameModel.state = GameModel.GameState.Idle;
    }
}
