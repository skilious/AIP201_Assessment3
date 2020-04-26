using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInteraction : MonoBehaviour
{
    public void QuitGame() // when person presses this button it closes the game
    {
        Debug.Log(" Quit ");
        Application.Quit();
    }

    public void PlayGame() // when person presses this button, it loads up the indexed scene
    {
        // in this case there is only one scene so it will be 0.
        SceneManager.LoadScene(0);
    }

    public void StartRace(GameObject StartScreen)
    {
        Time.timeScale = 1f;
        StartScreen.SetActive(false);
    }

}
