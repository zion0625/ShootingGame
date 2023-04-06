using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetShot : MonoBehaviour
{
    GameObject player;
    public GameObject particle;
    Vector3 dir;
    public float speed;
    public float rotSpeed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Destroy());
    }
    void Update()
    {
        dir = transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion quat = Quaternion.AngleAxis(angle-90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, quat, rotSpeed * Time.deltaTime);

        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "pShot")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
