using UnityEngine;
using UnityEngine.UI;
using qm = Managers.QuestionManager;

namespace Managers
{
    public class GuiManager : MonoBehaviour
    {
        public static GuiManager _instance;

        // Inspector
        [Header("Main Game")]
        public Text QuestionText;
        public Text CorrectAnswersScore;
        public Text TotalAnswersScore;
        public Text PlayerScore;        
        public Text TimeText;
        public Image QuestionImage;
        public Image TotalQuetionsFiller;
        public Text categoryLevel;
        public GameObject[] livesIndicator;
        public Text categoryName;
        public GameObject pauseMenu;

        [Header("Results canvas")]
        public GameObject StatsScreenCanvas;
        public Text percentText;
        public Text percentNeededText;
        public Text finalScore;
        public float percent = 0f;
        public float percentNeeded;
        public GameObject winHeader;
        public GameObject loseHeader;
        public GameObject retryButton;
        public GameObject nextLevelButton;
        public GameObject zeroStarsImage;
        public GameObject oneStarImage;
        public GameObject twoStarsImage;
        public GameObject threeStarsImage;

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
            
            //Lifes.text = GameManager._instance.player.lives.ToString();
        }

        private void Start()
        {
            UpdateCategoryText();
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
            percent = (float)qm._instance.totalCorrectAnswers / qm._instance.answersGiven * 100;
            percentNeeded = (float)qm._instance.answersNeededToPassLevel / qm._instance.totalQuestions * 100;
            percentText.text = percent.ToString("n2");
            percentNeededText.text = percentNeeded.ToString("n2");

            if(percent < percentNeeded)
            {
                TotalQuetionsFiller.fillAmount = 0;
                zeroStarsImage.SetActive(true);
                oneStarImage.SetActive(false);
                twoStarsImage.SetActive(false);
                threeStarsImage.SetActive(false);
            }
            // one star
            else if(percent >= percentNeeded && percent <= 90)
            {
                TotalQuetionsFiller.fillAmount = 0.4f;
                zeroStarsImage.SetActive(false);
                oneStarImage.SetActive(true);
                twoStarsImage.SetActive(false);
                threeStarsImage.SetActive(false);
            }
            // two stars
            else if(percent >= percentNeeded && percent > 90 && percent < 100)
            {
                TotalQuetionsFiller.fillAmount = 0.8f;
                zeroStarsImage.SetActive(false);
                oneStarImage.SetActive(false);
                twoStarsImage.SetActive(true);
                threeStarsImage.SetActive(false);
            }
            // 3 stars
            else
            {
                TotalQuetionsFiller.fillAmount = 1;
                zeroStarsImage.SetActive(false);
                oneStarImage.SetActive(false);
                twoStarsImage.SetActive(false);
                threeStarsImage.SetActive(true); 
            }            
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

        public void UpdateCategoryText()
        {
            categoryLevel.text = PlayerPrefmanager.GetCategoryLevel(qm._instance.currentCategory.categoryName).ToString();
            categoryName.text = qm._instance.currentCategory.categoryName;
        }

        public void DisplayStats()
        {
            StatsScreenCanvas.SetActive(true);
            finalScore.text = PlayerScore.text;
            percentText.text = percent.ToString("n1") + "%";

            if (qm._instance.passedLevel)
            {
                winHeader.SetActive(true);
                loseHeader.SetActive(false);
                retryButton.SetActive(false);
                nextLevelButton.SetActive(true);

                
            }
            else
            {
                loseHeader.SetActive(true);
                winHeader.SetActive(false);
                retryButton.SetActive(true);
                nextLevelButton.SetActive(false);
                zeroStarsImage.SetActive(true);
            }
        }
    }
}
