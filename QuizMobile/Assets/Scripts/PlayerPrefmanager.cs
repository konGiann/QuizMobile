using UnityEngine;

namespace Managers
{
    public class PlayerPrefmanager : MonoBehaviour
    {
        #region player score

        public static int GetScore()
        {
            if (PlayerPrefs.HasKey("Score"))
                return PlayerPrefs.GetInt("Score");

            else return 0;
        }

        public static void SetScore(int score)
        {
            PlayerPrefs.SetInt("Score", score);
        }

        public static int GetPlayerLevel()
        {
            if (PlayerPrefs.HasKey("PlayerLevel"))
                return PlayerPrefs.GetInt("PlayerLevel");
            else
                return 0;
        }

        public static void SetPlayerLevel(int playerLevel)
        {
            PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        }

        public static int GetCategoryLevel(Category category)
        {
            if (PlayerPrefs.HasKey(category.categoryName))
                return PlayerPrefs.GetInt(category.categoryName);
            else return 0;
        }

        public static void SetCategoryLevel(Category category, int level)
        {
            PlayerPrefs.SetInt(category.categoryName, level);
        }
            

        #endregion

        #region player highscore

        public static int GetHighScore()
        {
            if (PlayerPrefs.HasKey("HighScore"))
                return PlayerPrefs.GetInt("HighScore");

            else return 0;
        }

        public static void SetHighScore(int highScore)
        {
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        #endregion

        #region resets

        public static void ResetStats()
        {
            PlayerPrefs.SetInt("Score", 0);
        }

        #endregion
    }
}
