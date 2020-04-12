using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using qm = Managers.QuestionManager;
using gm = Managers.GameManager;

public class DisplayStats : MonoBehaviour
{
    public Text FinalScore;
    public Text HighScore;
    public Text HasHighscore;
    public Text SuccessPercentage;
    public Text Grade;

    float percentage;

    public delegate void OnCategoryRestart();
    public static event OnCategoryRestart CategoryReplay;
    
    void Awake()
    {
        FinalScore.text = PlayerPrefmanager.GetScore().ToString();
        HighScore.text = PlayerPrefmanager.GetHighScore().ToString();
        if (!GameManager._instance.hasNewHighscore)
        {
            HasHighscore.gameObject.SetActive(false);
        }

        CalculatePercentage();
        AssignGrade();
    }

    public void AssignGrade()
    {
        if (percentage == 100)
        {
            Grade.text = "Φώστήρας!";
        }
        else if (percentage > 75 && percentage < 100)
        {
            Grade.text = "Όχι κι άσχημα!";
        }
        else if (percentage > 50 && percentage <= 75)
        {
            Grade.text = "Θέλεις δουλίτσα!";
        }
        else if (percentage > 25 && percentage <= 50)
        {
            Grade.text = "Οι εγγραφές στο Δημοτικό ξεκίνησαν!";
        }
        else
        {
            Grade.text = "Τί λέει Σταύρο, καλά;";
        }
    }

    public void CalculatePercentage()
    {
        //percentage = ((float)GameManager._instance.totalCorrectAnswers / (float)GameManager._instance.totalQuestions) * 100;
        SuccessPercentage.text = percentage.ToString("n2") + "%";
    }

    public void TryCategoryAgain()
    {        
        SceneManager.LoadScene(2);        
        gm._instance.currentState = GameState.Running;
        CategoryReplay();
    }

    public void QuitApp()
    {
        Application.Quit();
    }


}
