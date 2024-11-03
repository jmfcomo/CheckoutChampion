using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public TMP_Text creditsButtonText;
    public GameObject credits;
    private bool creditsShown = false;

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GameManager.LOAD_STORE();
    }

    public void QuitGame ()
    {
       // Debug.Log("Quit!");
        Application.Quit();
    }

    public void ToggleCredits()
    {
        if (!creditsShown)
        { 
            creditsShown = true;
            credits.SetActive(true);
            creditsButtonText.text = "CLOSE";
        }

        else
        {
            creditsShown = false;
            credits.SetActive(false);
            creditsButtonText.text = "CREDITS";
        }
    }
}
