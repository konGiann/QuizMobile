using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using gui = Managers.GuiManager;
using menu = Managers.MenuManager;
using qm = Managers.QuestionManager;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        //public static GameManager _instance ;

        #region inspector fields

        [Header("Default Εικόνες κατηγοριών")]
        public Sprite religionImage;
        public Sprite cultureImage;
        public Sprite natureImage;
        public Sprite covidImage;

        [Header("Game Info")]
        public int highScore;
        public int lives;

        public QuestionDifficulty currentDifficulty;

        public PlayerProfile player;        

        public GameState currentState;

        #endregion

        #region fields        

        [HideInInspector]
        public Sprite defaultCategorySprite;

        [HideInInspector]
        public bool hasNewHighscore;

        #endregion

        protected override void Awake()
        {
            base.Awake();
                        
            Init();
        }

        /// <summary>
        /// Init fields and assign delegates
        /// </summary>
        private void Init()
        {
            
                                    
            #region events to look at

            // increase score on correct answer
            qm._instance.onCorrectAnswer += IncreaseScore;

            // reduce player lifes on wrong answer
            qm._instance.onWrongAnswer += ReduceLifes;

            // start question set again on failure
            DisplayStats.TrySameQuestionSetAgain += RetryQuestionSet;

            #endregion
            
            // load player's statistics
            LoadPlayerStats();
        }

        private void Start()
        {
            //CheckDifficulty();

            // load question set of chosen category
            qm._instance.SetSelectedCategory();           
            RetryQuestionSet();
        }
        
        private void Update()
        {        
            // check for ESC key to pause game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }

        /// <summary>
        /// start question set based on difficulty
        /// </summary>
        private void RetryQuestionSet()
        {
            TimeManager.timerIsPaused = false;
            player.lives = lives;
            gui._instance.UpdatePlayerLifes(player.lives);
            qm._instance.SelectRandomQuestion(qm._instance.currentDifficulty);
        }

        //private void CheckGameState()
        //{
        //    switch (currentState)
        //    {
        //        case GameState.Running:

        //            break;
        //        case GameState.Paused:
        //            PauseGame();
        //            break;
        //        case GameState.GameOver:
        //            //LoadGameOverScreen();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1;
            currentState = GameState.Running;
        }
        
        private void ReduceLifes()
        {
            player.lives--;
            gui._instance.UpdatePlayerLifes(player.lives);            
        }
        
        private void IncreaseScore()
        {
            switch (currentDifficulty)
            {
                case QuestionDifficulty.EASY:
                    player.score += 10;
                    gui._instance.UpdatePlayerScore(player.score);
                    break;
                case QuestionDifficulty.NORMAL:
                    player.score += 15;
                    gui._instance.UpdatePlayerScore(player.score);
                    break;
                case QuestionDifficulty.HARD:
                    player.score += 20;
                    gui._instance.UpdatePlayerScore(player.score);
                    break;
                default:
                    break;
            }
            PlayerPrefmanager.SetScore(player.score);

        }

        private void LoadPlayerStats()
        {
            // init player
            player = new PlayerProfile();

            player.score = PlayerPrefmanager.GetScore();

            player.level = 1;
            //highScore = PlayerPrefmanager.GetHighScore();

            player.lives = lives;
        }
    }

    public enum GameState
    {
        Running,
        Paused,
        GameOver
    }
}
