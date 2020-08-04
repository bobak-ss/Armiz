using UnityEngine;

public class GameData
{
    public static int AllyCount
    {
        get
        {
            return PlayerPrefs.GetInt("AC", DefaultValues.AllyCountInitial);
        }
        set
        {
            PlayerPrefs.SetInt("AC", value);
        }
    }
    public static int Coin
    {
        get
        {
            return PlayerPrefs.GetInt("coin", DefaultValues.CointCountInitial);
        }
        set
        {
            PlayerPrefs.SetInt("coin", value);
        }
    }

    private class DefaultValues
    {
        public const int AllyCountInitial = 1;
        public const int CointCountInitial = 500;
    }
}