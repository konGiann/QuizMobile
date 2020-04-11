using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using gui = Managers.GuiManager;
using gm = Managers.GameManager;
using sm = Managers.SoundManager;

namespace Managers
{
    public class QuestionManager : MonoBehaviour
    {
        public static QuestionManager _instance;

        #region inspector fields

        [Header("Religion Questions")]
        public QuestionList religionQuestions;

        [Header("Culture Questions")]
        public QuestionList cultureQuestions;

        [Header("Nature Questions")]
        public QuestionList natureQuestions;

        [Header("COVID Questions")]
        public QuestionList covidQuestions;

        [Header("User selected Questions")]
        public List<Question> selectedQuestions;

        public int totalCorrectAnswers;

        public int totalQuestions;

        #endregion

        #region fields        

        private bool isQuestionAnswered;

        private TimeManager tm;

        private Question currentQuestion;

        private Sprite defaultCategorySprite;

        private List<Question> easyQuestions;
        private List<Question> normalQuestions;
        private List<Question> hardQuestions;

        public delegate void OnAnswer();
        public event OnAnswer onCorrectAnswer;
        public event OnAnswer onWrongAnswer;

        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<QuestionManager>();
            }

            tm = GetComponent<TimeManager>();

            tm.onTimeEnded += LostDueToTimer;

            isQuestionAnswered = false;


        }

        private void Update()
        {
            tm.AnswerCountDown(isQuestionAnswered);
        }

        public void SetSelectedCategory(string selectedCategory)
        {
            switch (selectedCategory)
            {
                case "Religion":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = religionQuestions.questionList.ToList();
                        defaultCategorySprite = gm._instance.religionImage;
                    }
                    break;
                case "Culture":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = cultureQuestions.questionList.ToList();
                        defaultCategorySprite = gm._instance.cultureImage;
                    }
                    break;
                case "Nature":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = natureQuestions.questionList.ToList();
                        defaultCategorySprite = gm._instance.natureImage;
                    }
                    break;
                case "COVID-19":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = covidQuestions.questionList.ToList();
                        defaultCategorySprite = gm._instance.covidImage;
                    }
                    break;

                default:
                    break;
            }
        }

        private void LostDueToTimer()
        {
            tm.timer = tm.timeForAnswer;
            selectedQuestions.Remove(currentQuestion);
            int randomIndex = Random.Range(0, sm._instance.WrongAnswers.Length);
            sm._instance.audioController.PlayOneShot(sm._instance.WrongAnswers[randomIndex]);
            gui._instance.ResetButtonColors();
            totalQuestions += 1;
            gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);
            SelectRandomQuestion(gm._instance.currentDifficulty);
        }

        public void SelectRandomQuestion(QuestionDifficulty diff)
        {
            if (selectedQuestions.Count(x => x.Difficulty == diff) != 0)
            {
                // filter questions by difficulty
                var diffQuestions = selectedQuestions.Where(x => x.Difficulty == diff).ToList();

                int randomIndex = Random.Range(0, diffQuestions.Count);

                currentQuestion = diffQuestions[randomIndex];
                
                gui._instance.QuestionText.text = currentQuestion.Text;

                // assign question image
                // else assign default image
                if (currentQuestion.Image == null)
                {
                    gui._instance.QuestionImage.sprite = defaultCategorySprite;
                }
                else
                {
                    gui._instance.QuestionImage.sprite = currentQuestion.Image;
                }

                // assign question text to button text
                int numberOfAnswers = 0;
                for (int i = 0; i < gui._instance.Answers.Length; i++)
                {
                    gui._instance.Answers[i].gameObject.SetActive(true);
                    gui._instance.Answers[i].GetComponentInChildren<Text>().text = currentQuestion.Answers[i].text;
                    gui._instance.Answers[i].interactable = true;

                    // count number of answers
                    if (!string.IsNullOrWhiteSpace(currentQuestion.Answers[i].text))
                    {
                        numberOfAnswers++;
                    }
                }

                // hide buttons that are not needed
                if (numberOfAnswers != gui._instance.Answers.Length)
                {
                    for (int i = numberOfAnswers; i <= gui._instance.Answers.Length -1; i++)
                    {
                        gui._instance.Answers[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // load game over screen
                SceneManager.LoadScene(3);
            }
        }

        public void CheckAnswer()
        {
            int correctIndex = -1;

            string name = EventSystem.current.currentSelectedGameObject.name;

            // find index of correct answer
            for (int i = 0; i < currentQuestion.Answers.Length; i++)
            {
                if (currentQuestion.Answers[i].isCorrect)
                {
                    correctIndex = i;
                }
            }

            // check user's answer
            if (name == correctIndex.ToString()) // correct
            {
                gui._instance.Answers[correctIndex].GetComponent<Image>().color = Color.green;

                // play random effect
                int randomIndex = UnityEngine.Random.Range(0, sm._instance.CorrectAnswers.Length);

                sm._instance.audioController.PlayOneShot(sm._instance.CorrectAnswers[randomIndex]);

                totalCorrectAnswers++;

                // check highscore
                if (totalCorrectAnswers >= gm._instance.highScore)
                {
                    gm._instance.hasNewHighscore = true;
                    gm._instance.highScore = totalCorrectAnswers;
                    gui._instance.TopScore.text = gm._instance.highScore.ToString();
                    PlayerPrefmanager.SetHighScore(gm._instance.highScore);
                }

                onCorrectAnswer();
                // PlayerPrefmanager.SetScore(totalCorrectAnswers);
            }

            else // wrong
            {
                gui._instance.Answers[int.Parse(name)].GetComponent<Image>().color = Color.red;

                // play random effect
                int randomIndex = UnityEngine.Random.Range(0, sm._instance.WrongAnswers.Length);
                sm._instance.audioController.PlayOneShot(sm._instance.WrongAnswers[randomIndex]);
                onWrongAnswer();
            }
            totalQuestions++;

            gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);

            StartCoroutine(GotoNextQuestionWithDelay(tm.nextQuestionDelay));
        }

        IEnumerator GotoNextQuestionWithDelay(float delay)
        {
            isQuestionAnswered = true;
            selectedQuestions.Remove(currentQuestion);

            foreach (var button in gui._instance.Answers)
            {
                button.interactable = false;
            }

            yield return new WaitForSeconds(delay);

            isQuestionAnswered = false;
            tm.timer = tm.timeForAnswer;
            gui._instance.ResetButtonColors();
            SelectRandomQuestion(gm._instance.currentDifficulty);
        }

        private void OnDisable()
        {
            tm.onTimeEnded -= LostDueToTimer;
        }

    }
}
