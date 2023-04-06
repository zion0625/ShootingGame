using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eShot : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
