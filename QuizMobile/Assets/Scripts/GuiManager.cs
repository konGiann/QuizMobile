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
        public Text categoryLevel;
        public GameObject[] livesIndicator;
        public GameObject StatsScreenCanvas;

        public Text percent;
        public Text percentNeeded;

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
            PlayerScore.text = PlayerPrefmanager.GetScore().ToString();
            TopScore.text = PlayerPrefmanager.GetHighScore().ToString();
            
            //Lifes.text = GameManager._instance.player.lives.ToString();
        }

        private void Start()
        {
            UpdateCategoryLevelText();
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

            // calculate stars
            float p = (float)qm._instance.totalCorrectAnswers / qm._instance.answersGiven * 100;
            float pNeeded = (float)qm._instance.answersNeededToPassLevel / qm._instance.totalQuestions * 100;
            percent.text = p.ToString("n2");
            percentNeeded.text = pNeeded.ToString("n2");

            if(p < pNeeded)
            {
                TotalQuetionsFiller.fillAmount = 0;
            }
            // one star
            else if(p >= pNeeded && p <= 90)
            {
                TotalQuetionsFiller.fillAmount = 0.4f;
            }
            // two stars
            else if(p >= pNeeded && p > 90 && p < 100)
            {
                TotalQuetionsFiller.fillAmount = 0.8f;
            }
            // 3 stars
            else
            {
                TotalQuetionsFiller.fillAmount = 1;
            }
            //TotalQuetionsFiller.fillAmount = (float)qm._instance.totalCorrectAnswers / (float)qm._instance.totalQuestions;
        }

        public void UpdatePlayerScore(int score)
        {
            PlayerScore.text = score.ToString();
        }

        public void UpdatePlayerLifes(int lifes)
        {
            //Lifes.text = lifes.ToString();
            for (int i = 0; i < livesIndicator.Length; i++)
            {
                if(i <= GameManager._instance.player.lives - 1)
                {
                    livesIndicator[i].SetActive(true);
                }
                else
                {
                    livesIndicator[i].SetActive(false);
                }
            }
        }

        public void UpdateCategoryLevelText()
        {
            categoryLevel.text = PlayerPrefmanager.GetCategoryLevel(qm._instance.currentCategory.categoryName).ToString();
        }
    }
}
