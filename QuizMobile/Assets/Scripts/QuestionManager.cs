using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using gm = Managers.GameManager;
using gui = Managers.GuiManager;
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

        #region delegates
        public delegate void OnAnswer();
        public event OnAnswer onCorrectAnswer;
        public event OnAnswer onWrongAnswer; 
        #endregion

        #endregion

        private void Awake()
        {

            Init();
        }

        /// <summary>
        /// Init fields
        /// </summary>
        private void Init()
        {
            if (_instance == null)
            {

                _instance = this;
            }

            tm = FindObjectOfType<TimeManager>();

            tm.onTimeEnded += LostDueToTimer;

            isQuestionAnswered = false;

            answersGiven = 0;
        }

        /// <summary>
        /// Check for wich category this question set is about
        /// and load the appropriate questions based on the difficulty
        /// </summary>
        /// <param name="selectedCategory"></param>
        public void SetSelectedCategory(string selectedCategory, QuestionDifficulty diff)
        {
            switch (selectedCategory)
            {
                case "Religion":

                    selectedQuestions = religionQuestions.questionList.ToList();
                    questionPool = selectedQuestions.Where(x => x.Difficulty == diff).ToList();
                    defaultCategorySprite = gm._instance.religionImage;

                    break;
                case "Culture":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = cultureQuestions.questionList.ToList();
                        questionPool = selectedQuestions.Where(x => x.Difficulty == diff).ToList();
                        defaultCategorySprite = gm._instance.cultureImage;
                    }
                    break;
                case "Nature":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = natureQuestions.questionList.ToList();
                        questionPool = selectedQuestions.Where(x => x.Difficulty == diff).ToList();
                        defaultCategorySprite = gm._instance.natureImage;
                    }
                    break;
                case "COVID-19":
                    if (selectedQuestions == null || selectedQuestions.Count == 0)
                    {
                        selectedQuestions = covidQuestions.questionList.ToList();
                        questionPool = selectedQuestions.Where(x => x.Difficulty == diff).ToList();
                        defaultCategorySprite = gm._instance.covidImage;
                    }
                    break;

                default:
                    break;
            }
        }

        
        /// <summary>
        /// Select a random question from the set question list based on the game difficulty
        /// </summary>
        /// <param name="diff">The set game difficulty</param>
        public void SelectRandomQuestion(QuestionDifficulty diff)
        {            
            StatsCanvas.SetActive(false);            
            
            // check the total questions of the level
            TotalQuestionsToAnswer();
            
            // if there are still questions to be answered and player has lifes, choose one question
            // else end question set and show results
            if (answersGiven < totalQuestions && gm._instance.player.lifes > 0)
            {
                if (questionPool.Count() < (totalQuestions - answersGiven))
                {                                      
                    questionPool = selectedQuestions.Where(x => x.Difficulty == diff).ToList();
                }
                int randomIndex = Random.Range(0, questionPool.Count);

                currentQuestion = questionPool[randomIndex];

                // assign question text to button text
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

                // count the number of possible answers the question has (max 6)
                // assign their text to buttons' text and make the buttons active                
                int numberOfAnswers = 0;
                for (int i = 0; i < gui._instance.Answers.Length; i++)
                {
                    gui._instance.Answers[i].gameObject.SetActive(true);
                    gui._instance.Answers[i].GetComponentInChildren<Text>().text = currentQuestion.Answers[i].text;
                    gui._instance.Answers[i].interactable = true;

                    
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
                QuestionSetEnded();                                        
            }
        }

        /// <summary>
        /// Stops current set of questions and timer
        /// </summary>
        private void QuestionSetEnded()
        {
            // stop timer
            TimeManager.timerIsPaused = true;

            // reset question stats
            answersGiven = 0;
            totalCorrectAnswers = 0;

            gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);

            // display stats
            StatsCanvas.SetActive(true);
        }

        /// <summary>
        /// Checks if the answer is correct and loads next question
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private void LostDueToTimer()
        {
            answersGiven++;

            // reset timer
            tm.ResetTimer();
            
            // play sound
            int randomIndex = Random.Range(0, sm._instance.WrongAnswers.Length);
            sm._instance.audioController.PlayOneShot(sm._instance.WrongAnswers[randomIndex]);
            gui._instance.ResetButtonColors();

            gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);

            // go to next question if question set is not over
            if (answersGiven < totalQuestions)
            {
                SelectRandomQuestion(gm._instance.currentDifficulty);
            }

        }

        /// <summary>
        /// Loads next question after some seconds
        /// </summary>
        /// <param name="delay">the delay in seconds</param>
        /// <returns></returns>
        IEnumerator GotoNextQuestionWithDelay(float delay)
        {
            // stop question timer for as long as is the question delay
            isQuestionAnswered = true;
            
            // make all buttons non interactable
            foreach (var button in gui._instance.Answers)
            {
                button.interactable = false;
            }

            yield return new WaitForSeconds(delay);

            // start timer again
            isQuestionAnswered = false;

            tm.ResetTimer();
            
            gui._instance.ResetButtonColors();

            // load next question
            SelectRandomQuestion(gm._instance.currentDifficulty);
        }

        /// <summary>
        /// Checks how many questions will the question set have based on the player level
        /// </summary>
        private void TotalQuestionsToAnswer()
        {
            switch (gm._instance.player.playerLevel)
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
    }
}
