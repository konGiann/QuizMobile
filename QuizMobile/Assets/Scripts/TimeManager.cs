using UnityEngine;
using gui = Managers.GuiManager;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        #region inspector fields

        public int timeForAnswer;
        public float nextQuestionDelay;
        public static bool timerIsPaused;

        #endregion

        #region fields

        [HideInInspector]
        public float timer;

        // inform QuestionManager that question time has ended
        public delegate void TimeEnded();

        public event TimeEnded onTimeEnded;

        #endregion

        private void Awake()
        {
            Init();
        }        

        private void Update()
        {
            // start question timer and stop when Question set is over 
            if (!timerIsPaused)
            {
                StartQuestionTimer(QuestionManager._instance.isQuestionAnswered); 
            }
        }


        /// <summary>
        /// Initialize fields
        /// </summary>
        private void Init()
        {
            timer = timeForAnswer;
            timerIsPaused = false;
        }

        /// <summary>
        /// Starts a question countdown 
        /// </summary>
        /// <param name="isQuestionAnswered">Run timer after next question delay</param>
        public void StartQuestionTimer(bool isQuestionAnswered)
        {
            // start timer
            if (!isQuestionAnswered)
            {
                timer -= Time.deltaTime;
            }
            // display timer on GUI
            int displayAsInt = (int)timer;
            gui._instance.TimeText.text = displayAsInt.ToString();

            // if timer reaches zero, notify QuestionManager to mark question as wrong
            if (displayAsInt == 0)
            {
                onTimeEnded();
            }
        }

        /// <summary>
        /// Resets timer back to initial value in the inspector
        /// </summary>
        public void ResetTimer()
        {
            timer = timeForAnswer;
        }
    }
}
