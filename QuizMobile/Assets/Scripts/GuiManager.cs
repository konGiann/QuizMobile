using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GuiManager : MonoBehaviour
    {
        public static GuiManager _instance;

        // Inspector
        public Text QuestionText;
        public Text CorrectAnswersScore;
        public Text TotalAnswersScore;
        public Text PlayerScore;
        public Text TopScore;
        public Text TimeText;
        public Image QuestionImage;
        public Button[] Answers;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            //else if (_instance != null)
            //{
            //    Destroy(gameObject);
            //}

            //DontDestroyOnLoad(gameObject);

            // initialize score values
            CorrectAnswersScore.text = "0";
            TotalAnswersScore.text = "/0";
            TopScore.text = PlayerPrefmanager.GetHighScore().ToString();
        }

        public void ResetButtonColors()
        {
            foreach (var button in Answers)
            {
                button.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }

        public void UpdateAnswersScore(int totalCorrectAnswers, int totalQuestions)
        {
            CorrectAnswersScore.text = totalCorrectAnswers.ToString();
            TotalAnswersScore.text = "/" + totalQuestions.ToString();
        }

        public void UpdatePlayerScore(int score)
        {
            PlayerScore.text = score.ToString();
        }
    }
}
