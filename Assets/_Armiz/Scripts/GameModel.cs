using System;
using UnityEngine;

[System.Serializable]
public class GameModel
{
    public GameState state;
    public int allyCount;
    public enum GameState
    {
        Attack,
        Idle
    }
}
