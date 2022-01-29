using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    public GameObject optionsMenuPanel;
    public GameObject startMenuPanel;
    public GameObject aboutMenuPanel;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Starts a New Game by Calling GameScene
    /// </summary>
    public void OnClickStartNewGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickQuitGame()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }

    public void OnClickOptionsButton()
    {
        startMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }

    public void OnClickBackButton()
    {
        startMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
        aboutMenuPanel.SetActive(false);
    } 

    public void OnClickAboutButton()
    {
        startMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
        aboutMenuPanel.SetActive(true);
    }


}
