using UnityEngine;
using UnityEngine.UI;
using qm = Managers.QuestionManager;

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
        public Image TotalQuetionsFiller;
        public Text Lifes;

        public Button[] Answers;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
         
            // initialize score values
            CorrectAnswersScore.text = "0";
            TotalAnswersScore.text = "/0";
            TopScore.text = PlayerPrefmanager.GetHighScore().ToString();
            Lifes.text = GameManager._instance.player.lifes.ToString();
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

            Debug.Log(qm._instance.answersGiven);
            Debug.Log(qm._instance.totalQuestions);
            TotalQuetionsFiller.fillAmount = (float)qm._instance.answersGiven / (float)qm._instance.totalQuestions;
        }

        public void UpdatePlayerScore(int score)
        {
            PlayerScore.text = score.ToString();
        }

        public void UpdatePlayerLifes(int lifes)
        {
            Lifes.text = lifes.ToString();
        }
    }
}
