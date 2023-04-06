using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBoard : MonoBehaviour
{
    public static SetBoard instance;

    public Text[] nameText, scoreText;
    private void Awake()
    {
        instance = this;
        ScoreBoard.instance.showBoard();
    }
}
