using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Button LoadGameButton;

    public GameObject playerName;

    public GameObject displayName;

    public Text playerInput;

    public Text playerDisplay;

    private void Awake()
    {
        LoadGameButton.gameObject.SetActive(false);

        if (string.IsNullOrEmpty(PlayerPrefmanager.GetPlayerName()))
        {
            playerName.SetActive(true);
        }
        else
        {
            displayName.SetActive(true);
            playerDisplay.text = PlayerPrefmanager.GetPlayerName();
        }
    }

    private void Start()
    {
        StartCoroutine(ShowButton());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene(1);
        //}
    }

    public void SetName()
    {
        PlayerPrefmanager.SetPlayerName(playerInput.text);
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
