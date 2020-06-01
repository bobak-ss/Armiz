using System;
using UnityEngine;

namespace Armiz
{
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
}
