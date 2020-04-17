using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayStats : MonoBehaviour
{    

    public delegate void OnCategoryStart();
    public static event OnCategoryStart StartQuestionsSet;        

    public void PlayAgain()
    {                        
        StartQuestionsSet();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitApp()
    {
        Application.Quit();
    }


}
