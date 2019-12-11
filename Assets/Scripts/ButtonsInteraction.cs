using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsInteraction : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
