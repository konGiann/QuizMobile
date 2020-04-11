using UnityEngine;
using gui = Managers.GuiManager;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        #region inspector fields

        public int timeForAnswer;
        public float nextQuestionDelay;

        #endregion

        #region fields

        [HideInInspector]
        public float timer;

        // inform QuestionManager that question time has ended
        public delegate void TimeEnded();

        public event TimeEnded onTimeEnded;

        #endregion

        public void AnswerCountDown(bool isQuestionAnswered)
        {
            if (!isQuestionAnswered)
            {
                timer -= Time.deltaTime;
            }
            int displayAsInt = (int)timer;
            gui._instance.TimeText.text = displayAsInt.ToString();
            if (displayAsInt == 0)
            {
                onTimeEnded();
            }
        }
    }
}
