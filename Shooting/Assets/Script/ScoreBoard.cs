using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;

    public List<string> userName;
    public List<int> userScore;

    private void Awake()
    {
        instance = this;
        userName = new List<string>();
        userScore = new List<int>();
    }

    public void menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void showBoard()
    {
        userName.Sort();
        userScore.Sort();
        userName.Reverse();
        userScore.Reverse();

        int i = 0;
        foreach (string n in userName)
        {
            SetBoard.instance.nameText[i].text = n;
            i++;
        }
        int j = 0;
        foreach (int n in userScore)
        {
            SetBoard.instance.scoreText[j].text = n.ToString();
            j++;
        }
    }
}