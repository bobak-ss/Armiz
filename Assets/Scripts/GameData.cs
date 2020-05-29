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

    private class DefaultValues
    {
        public const int AllyCountInitial = 1;
    }
}