using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    public void loadSingleplayer()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void loadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
