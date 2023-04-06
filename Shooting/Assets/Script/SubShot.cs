using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubShot : MonoBehaviour
{
    public static SubShot instance;

    private void Awake()
    {
        instance = this;
    }

    public void shot(GameObject obj)
    {
        Instantiate(obj, transform.position, Quaternion.identity);
    }
}
