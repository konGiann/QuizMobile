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
            // reset score and set highscore
            PlayerPrefmanager.ResetStats();
            highScore = PlayerPrefmanager.GetHighScore();
            
            

            #region events to look at

            // increase score on correct answer
            qm._instance.onCorrectAnswer += IncreaseScore;

            // reduce player lifes on wrong answer
            qm._instance.onWrongAnswer += ReduceLifes;

            // start question set again on failure
            DisplayStats.TrySameQuestionSetAgain += StartQuestionSet;

            #endregion

            // init player
            player = new PlayerProfile
            {
                level = 1,
                score = 0
            };
        }

        private void Start()
        {
            CheckDifficulty();

            // load question set of chosen category
            qm._instance.SetSelectedCategory(menu._instance.selectedCategory, currentDifficulty);
            Debug.Log("hi");
            StartQuestionSet();
        }
        
        private void Update()
        {
            //CheckGameState();
            CheckDifficulty();

            // check for ESC key to pause game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }

        /// <summary>
        /// start question set based on difficulty
        /// </summary>
        private void StartQuestionSet()
        {
            TimeManager.timerIsPaused = false;
            qm._instance.SelectRandomQuestion(currentDifficulty);
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
     
        /// <summary>
        /// Check game difficulty based on player level
        /// we use that to assign the proper question set based on difficulty in the QuestionManager
        /// </summary>
        private void CheckDifficulty()
        {
            if (player.level <= 5)
            {
                currentDifficulty = QuestionDifficulty.EASY;
            }
            else if (player.level > 5 && player.level <= 10)
            {
                currentDifficulty = QuestionDifficulty.NORMAL;
            }
            else
            {
                currentDifficulty = QuestionDifficulty.HARD;
            }
        }

        private void ReduceLifes()
        {
            //
        }
        
        void IncreaseScore()
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
        }
    }

    public enum GameState
    {
        Running,
        Paused,
        GameOver
    }
}
