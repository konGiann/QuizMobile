using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Button LoadGameButton;

    private void Awake()
    {
        LoadGameButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(ShowButton());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void LoadGame()
    {        
        SceneManager.LoadScene(1);        
    }

    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(10f);
        LoadGameButton.gameObject.SetActive(true);
    }
}
