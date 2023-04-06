using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scence : MonoBehaviour
{
    public void nextStage()
    {
        GameManager.instance.ResetStage();
    }

    public void record_n_menu()
    {
        if (GameManager.instance.inputField.text.Length > 0)
        {
            ScoreBoard.instance.userName.Add(GameManager.instance.inputField.text);
            ScoreBoard.instance.userScore.Add(GameManager.instance.playerScore);
            SceneManager.LoadScene("Menu");
        }
    }

    public void menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void game()
    {
        SceneManager.LoadScene("Game");
    }
    public void help()
    {
        SceneManager.LoadScene("Help");
    }
    public void rank()
    {
        SceneManager.LoadScene("Rank");
    }
}
