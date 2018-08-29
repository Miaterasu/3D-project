using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject MainMenuPopup;
    public GameObject QuitGamePopup;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                MainMenuPopup.SetActive(true);
                QuitGamePopup.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                MainMenuPopup.SetActive(false);
                QuitGamePopup.SetActive(false);
            }
        }
    }

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("Gane");
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
