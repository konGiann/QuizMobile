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

        public GameObject StatsCanvas;

        // temp public
        public List<Question>  questionPool;

        public List<Question> easyQuestions;

        public int totalCorrectAnswers;

        public int totalQuestions;

        public int answersGiven;

        #endregion

        #region fields        

        [HideInInspector]
        public bool isQuestionAnswered;

        private TimeManager tm;

        private Question currentQuestion;

        private Sprite defaultCategorySprite;
        
        private List<Question> normalQuestions;
        private List<Question> hardQuestions;
                
        #region delegates
        public delegate void OnAnswer();
        public event OnAnswer onCorrectAnswer;
        public event OnAnswer onWrongAnswer; 
        #endregion

        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
               
                _instance = this;
            }            

            tm = FindObjectOfType<TimeManager>();

            tm.onTimeEnded += LostDueToTimer;
            

            isQuestionAnswered = false;
            answersGiven = 0;
            //DisplayStats.CategoryReplay += SelectRandomQuestion;
        }

        private void Start()
        {
            
        }        

        public void SetSelectedCategory(string selectedCategory)
        {
            switch (selectedCategory)
            {
                case "Religion":

                    selectedQuestions = religionQuestions.questionList.ToList();
                    
                    if (gm._instance.currentDifficulty == QuestionDifficulty.EASY)
                        questionPool = easyQuestions.ToList();
                    easyQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.EASY).ToList();
                    normalQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.NORMAL).ToList();
                    hardQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.HARD).ToList();
                    defaultCategorySprite = gm._instance.religionImage;

                    break;
                case "Culture":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = cultureQuestions.questionList.ToList();
                        easyQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.EASY).ToList();
                        normalQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.NORMAL).ToList();
                        hardQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.HARD).ToList();
                        defaultCategorySprite = gm._instance.cultureImage;
                    }
                    break;
                case "Nature":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = natureQuestions.questionList.ToList();
                        easyQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.EASY).ToList();
                        normalQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.NORMAL).ToList();
                        hardQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.HARD).ToList();
                        defaultCategorySprite = gm._instance.natureImage;
                    }
                    break;
                case "COVID-19":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = covidQuestions.questionList.ToList();
                        easyQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.EASY).ToList();
                        normalQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.NORMAL).ToList();
                        hardQuestions = selectedQuestions.Where(x => x.Difficulty == QuestionDifficulty.HARD).ToList();
                        defaultCategorySprite = gm._instance.covidImage;
                    }
                    break;

                default:
                    break;
            }
        }

        private void LostDueToTimer()
        {
            Debug.Log("lost");
            tm.timer = tm.timeForAnswer;
            answersGiven++;

            int randomIndex = Random.Range(0, sm._instance.WrongAnswers.Length);
            sm._instance.audioController.PlayOneShot(sm._instance.WrongAnswers[randomIndex]);
            gui._instance.ResetButtonColors();
            
            gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);
            SelectRandomQuestion(gm._instance.currentDifficulty);
        }

        public void SelectRandomQuestion(QuestionDifficulty diff)
        {
            StatsCanvas.SetActive(false);
            // check the total questions of the level
            NumberOfQuestionsToAnswer();
            
            // if we can provide questions choose one
            // else game over
            if (answersGiven < totalQuestions)
            {
                if (questionPool.Count() < (totalQuestions - answersGiven))
                {                    
                    questionPool = easyQuestions.ToList();                    
                }
                int randomIndex = Random.Range(0, questionPool.Count);

                currentQuestion = questionPool[randomIndex];                           

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
                    for (int i = numberOfAnswers; i <= gui._instance.Answers.Length - 1; i++)
                    {
                        gui._instance.Answers[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                answersGiven = 0;
                totalCorrectAnswers = 0;
                gm._instance.currentState = GameState.GameOver;

                StatsCanvas.SetActive(true);
                //SceneManager.LoadScene(3);
            }
        }

        public void CheckAnswer()
        {
            ++answersGiven;

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
                questionPool.Remove(currentQuestion);
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

            gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);


            StartCoroutine(GotoNextQuestionWithDelay(tm.nextQuestionDelay));
        }

        IEnumerator GotoNextQuestionWithDelay(float delay)
        {
            isQuestionAnswered = true;
            
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

        private void NumberOfQuestionsToAnswer()
        {
            switch (gm._instance.player.level)
            {
                case 1:
                case 2:
                    totalQuestions = 8;
                    break;
                case 3:
                case 4:
                    totalQuestions = 10;
                    break;
                case 5:
                case 6:
                    totalQuestions = 12;
                    break;
                case 7:
                    totalQuestions = 14;
                    break;
                case 8:
                case 9:
                    totalQuestions = 16;
                    break;
                case 10:
                case 11:
                    totalQuestions = 18;
                    break;
                case 12:
                case 13:
                    totalQuestions = 20;
                    break;
                case 14:
                    totalQuestions = 22;
                    break;
                case 15:
                    totalQuestions = 25;
                    break;                                    
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            //DisplayStats.CategoryReplay -= SelectRandomQuestion;
        }
    }
}
