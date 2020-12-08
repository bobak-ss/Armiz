using UnityEngine;

namespace Armiz
{
    public class GameData
    {
        public static bool FirstSession
        {
            get
            {
                if (PlayerPrefs.GetInt("FS", 1) == 0)
                    return false;
                else
                    return true;
            }
            set
            {
                PlayerPrefs.SetInt("FS", (value) ? 1 : 0);
            }
        }
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
}