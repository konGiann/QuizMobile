﻿using UnityEngine;
using menu = Managers.MenuManager;
using qm = Managers.QuestionManager;
using gui = Managers.GuiManager;
using System;
using UnityEngine.SceneManagement;

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

        private void Awake()
        {
            //if (_instance == null)
            //{               
            //    _instance = this;
            //}
            //else
            //{
                
            //}
            
            // reset score and set highscore
            PlayerPrefmanager.ResetStats();
            highScore = PlayerPrefmanager.GetHighScore();

            qm._instance.SetSelectedCategory(menu._instance.selectedCategory);

            qm._instance.onCorrectAnswer += IncreaseScore;

            qm._instance.onWrongAnswer += ReduceLifes;

            // replay
            DisplayStats.CategoryReplay += StartQuestions;

            InitPlayer();
        }

        private void InitPlayer()
        {
            player = new PlayerProfile
            {
                level = 1,
                score = 0
            };
        }

        private void Start()
        {
            CheckDifficulty();
            StartQuestions();
        }

        private void StartQuestions()
        {
            qm._instance.SelectRandomQuestion(currentDifficulty);
        }

        private void Update()
        {
            CheckGameState();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentState = GameState.Paused;
            }
        }

        private void CheckGameState()
        {
            switch (currentState)
            {
                case GameState.Running:
                    CheckDifficulty();
                    break;
                case GameState.Paused:
                    PauseGame();
                    break;
                case GameState.GameOver:
                    //LoadGameOverScreen();
                    break;
                default:
                    break;
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1;
            currentState = GameState.Running;
        }

        private void LoadGameOverScreen()
        {
            SceneManager.LoadScene(3);
        }


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
