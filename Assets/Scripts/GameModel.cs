using System;
[System.Serializable]
public class GameModel
{
    public GameState state;
    public enum GameState
    {
        Attack,
        Idle
    }


}
