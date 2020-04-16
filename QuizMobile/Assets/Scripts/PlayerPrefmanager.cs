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
            else return 1;
        }

        public static int GetCategoryLevel(string categoryName)
        {
            if (PlayerPrefs.HasKey(categoryName))
                return PlayerPrefs.GetInt(categoryName);
            else return 1;
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

        public static void ResetAllCategoriesLevel()
        {
            PlayerPrefs.SetInt("Religion", 1);
            PlayerPrefs.SetInt("COVID-19", 1);
            PlayerPrefs.SetInt("Geography", 1);
            PlayerPrefs.SetInt("History", 1);
            PlayerPrefs.SetInt("Nature", 1);
            PlayerPrefs.SetInt("Culture", 1);
        }

        #endregion
    }
}
