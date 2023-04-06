using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    public float size;
    public GameObject attacker;
    public float time;
    Vector3 min, max;
    void Awake()
    {
        size = GetComponent<BoxCollider2D>().size.x;
        min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        if (GameManager.instance.stage == 1)
        {
            transform.localScale = new Vector3((Mathf.Abs(min.x) + max.x) / 7, Mathf.Abs(min.y) + max.y);
        }
        else
        {
            transform.localScale = new Vector3((Mathf.Abs(min.x) + max.x) / 8, Mathf.Abs(min.y) + max.y);
        }
    }

    private void Start()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(time);
        GameObject attack = Instantiate(attacker, new Vector3(transform.position.x, max.y-1), Quaternion.identity);
        Destroy(gameObject);
    }
}
