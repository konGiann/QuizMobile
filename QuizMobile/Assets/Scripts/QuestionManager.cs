using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using gm = Managers.GameManager;
using gui = Managers.GuiManager;
using sm = Managers.SoundManager;
using menu = Managers.MenuManager;

namespace Managers
{
    public class QuestionManager : MonoBehaviour
    {
        public static QuestionManager _instance;

        #region inspector fields
     
        [Header("User selected Questions")]
        public List<Question> selectedQuestions;

        [Header("Religion")]
        public Category religion;

        [Header("Culture")]
        public Category culture;

        [Header("Nature")]
        public Category nature;

        [Header("Covid")]
        public Category covid;

        [Header("History")]
        public Category history;

        [Header("Geography")]
        public Category geography;

        public Category currentCategory;

        public QuestionDifficulty currentDifficulty;
        
        // temp public
        public List<Question>  questionPool;
        
        public int totalCorrectAnswers;

        public int totalQuestions;

        public int answersGiven;

        public int answersNeededToPassLevel;

        #endregion

        #region fields        

        [HideInInspector]
        public bool isQuestionAnswered;

        public bool passedLevel;

        private TimeManager tm;

        private Question currentQuestion;        

        private Sprite defaultCategorySprite;                      

        #region delegates
        public delegate void OnAnswer();
        public event OnAnswer OnCorrectAnswer;
        public event OnAnswer OnWrongAnswer; 
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
        /// Assigns the current category, loads its level and then loads questions based on the difficulty
        /// </summary>
        public void LoadCategory()
        {
            switch (menu._instance.selectedCategory)
            {
                case "Religion":
                    currentCategory = religion;
                    currentCategory.level = PlayerPrefmanager.GetCategoryLevel(currentCategory.categoryName);
                    currentDifficulty = currentCategory.CalculateDifficulty();
                    selectedQuestions = religion.questions.questionList.ToList();
                    questionPool = selectedQuestions.Where(x => x.Difficulty == currentDifficulty).ToList();
                    defaultCategorySprite = gm._instance.religionImage;
                    break;
                case "Culture":
                    currentCategory = culture;
                    currentCategory.level = PlayerPrefmanager.GetCategoryLevel(currentCategory.categoryName);
                    currentDifficulty = currentCategory.CalculateDifficulty();
                    selectedQuestions = culture.questions.questionList.ToList();
                    questionPool = selectedQuestions.Where(x => x.Difficulty == currentDifficulty).ToList();
                    defaultCategorySprite = gm._instance.cultureImage;
                    break;
                case "Nature":
                    currentCategory = nature;
                    currentCategory.level = PlayerPrefmanager.GetCategoryLevel(currentCategory.categoryName);
                    currentDifficulty = currentCategory.CalculateDifficulty();
                    selectedQuestions = nature.questions.questionList.ToList();
                    questionPool = selectedQuestions.Where(x => x.Difficulty == currentDifficulty).ToList();
                    defaultCategorySprite = gm._instance.natureImage;
                    break;
                case "History":
                    currentCategory = history;
                    currentCategory.level = PlayerPrefmanager.GetCategoryLevel(currentCategory.categoryName);
                    currentDifficulty = currentCategory.CalculateDifficulty();
                    selectedQuestions = history.questions.questionList.ToList();
                    questionPool = selectedQuestions.Where(x => x.Difficulty == currentDifficulty).ToList();

                    //TODO: Find history image
                    defaultCategorySprite = gm._instance.defaultCategorySprite;
                    break;
                case "COVID-19":
                    currentCategory = covid;
                    currentCategory.level = PlayerPrefmanager.GetCategoryLevel(currentCategory.categoryName);
                    selectedQuestions = covid.questions.questionList.ToList();
                    questionPool = selectedQuestions.Where(x => x.Difficulty == currentDifficulty).ToList();
                    defaultCategorySprite = gm._instance.covidImage;
                    break;
                case "Geography":
                    currentCategory = geography;
                    currentCategory.level = PlayerPrefmanager.GetCategoryLevel(currentCategory.categoryName);
                    currentDifficulty = currentCategory.CalculateDifficulty();
                    selectedQuestions = geography.questions.questionList.ToList();
                    questionPool = selectedQuestions.Where(x => x.Difficulty == currentDifficulty).ToList();
                    //TODO: Find history image
                    defaultCategorySprite = gm._instance.defaultCategorySprite;
                    break;
                default:
                    break;
            }
            
        }

        /// <summary>
        /// Selects a random question from the set question list based on the game difficulty
        /// </summary>
        /// <param name="diff">The set game difficulty</param>
        public void SelectRandomQuestion(QuestionDifficulty diff)
        {
            gui._instance.StatsScreenCanvas.SetActive(false);
            
            // check the total questions of the level
            CalculateAnswersStats();
            
            // if there are still questions to be answered and player has lifes, choose one question
            // else end question set and show results
            if (answersGiven < totalQuestions && gm._instance.player.lives > 0)
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
        /// Stops current set of questions and timer and displays the results
        /// </summary>
        private void QuestionSetEnded()
        {
            // stop timer
            TimeManager.timerIsPaused = true;
                        
            // display stats
            
            

            // question set passed!
            if(totalCorrectAnswers >= answersNeededToPassLevel)
            {
                // increase and save category level
                currentCategory.level++;
                PlayerPrefmanager.SetCategoryLevel(currentCategory, currentCategory.level);

                gui._instance.UpdateCategoryText();

                //TODO: show canvas with go to next level button
                passedLevel = true;

            }
            // question set failed
            else
            {
                //TODO: show canvas with retry level button
                passedLevel = false;
            }

            gui._instance.DisplayStats();

            // reset question stats
            answersGiven = 0;
            totalCorrectAnswers = 0;

            //gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);
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

                OnCorrectAnswer();                
            }

            else // wrong
            {
                gui._instance.Answers[int.Parse(name)].GetComponent<Image>().color = Color.red;

                // play random effect
                int randomIndex = UnityEngine.Random.Range(0, sm._instance.WrongAnswers.Length);
                sm._instance.audioController.PlayOneShot(sm._instance.WrongAnswers[randomIndex]);
                OnWrongAnswer();
            }            

            gui._instance.UpdateAnswersScore(totalCorrectAnswers, totalQuestions);


            StartCoroutine(GotoNextQuestionWithDelay(tm.nextQuestionDelay));                       
        }

        /// <summary>
        /// Automatically gives a wrong question when timers is ended without any user input
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

            OnWrongAnswer();

            // go to next question if question set is not over
            if (answersGiven < totalQuestions)
            {
                SelectRandomQuestion(currentDifficulty);
            }
        }

        /// <summary>
        /// Loads next question after some seconds after a user input
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
            SelectRandomQuestion(currentDifficulty);
        }

        /// <summary>
        /// Checks how many questions will the question set have based on the category level
        /// </summary>
        private void CalculateAnswersStats()
        {
            switch (currentCategory.level)
            {
                case 1:
                    totalQuestions = 8;
                    answersNeededToPassLevel = 3;
                    break;
                case 2:
                    totalQuestions = 8;
                    answersNeededToPassLevel = 4;
                    break;
                case 3:
                    totalQuestions = 10;
                    answersNeededToPassLevel = 6;
                    break;
                case 4:
                    totalQuestions = 10;
                    answersNeededToPassLevel = 7;
                    break;                    
                case 5:
                    totalQuestions = 12;
                    answersNeededToPassLevel = 8;
                    break;
                case 6:
                    totalQuestions = 12;
                    answersNeededToPassLevel = 9;                    
                    break;
                case 7:
                    totalQuestions = 14;
                    answersNeededToPassLevel = 10;                    
                    break;
                case 8:
                    totalQuestions = 16;
                    answersNeededToPassLevel = 11;
                    break;
                case 9:
                    totalQuestions = 16;
                    answersNeededToPassLevel = 12;
                    break;
                case 10:
                case 11:
                    totalQuestions = 18;
                    answersNeededToPassLevel = 13;
                    break;
                case 12:
                    totalQuestions = 20;
                    answersNeededToPassLevel = 14;
                    break;
                case 13:
                    totalQuestions = 20;
                    answersNeededToPassLevel = 15;
                    break;
                case 14:
                    totalQuestions = 22;
                    answersNeededToPassLevel = 16;
                    break;
                case 15:
                    totalQuestions = 25;
                    answersNeededToPassLevel = 20;
                    break;                                    
                default:
                    break;
            }
        }        
    }
}
