using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringBoard : MonoBehaviour
{
    public GameObject board;
    public static bool spawnBoard;
    // Start is called before the first frame update
    void Awake()
    {
        if (!spawnBoard)
        {
            Instantiate(board);
            spawnBoard = true;
        }
    }
}
