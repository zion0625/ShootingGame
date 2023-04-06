using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Enemy : MonoBehaviour
{
    public AudioClip[] clip;
    AudioSource source;

    public GameObject item;
    public int dropRate;
    public float speed;
    public int hp;
    public int score;
    public GameObject[] shots;
    public GameObject dieParticle;
    public int enemyType;
    public Sprite[] enemySprite;
    public Sprite[] hitSpr;
    SpriteRenderer spr;

    GameObject player;
    Vector3 dir;
    // Start is called before the first frame update
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyType = Random.Range(0, enemySprite.Length);
        dropRate = Random.Range(0, 10);

        switch (enemyType) 
        { 
            case 0:
                spr.sprite = enemySprite[enemyType];
                hp = 3;
                speed = 1.3f;
                score = 30;
                StartCoroutine(shot(2.8f));
                break;
            case 1:
                spr.sprite = enemySprite[enemyType];
                hp = 2;
                speed = 1.5f;
                score = 20;
                StartCoroutine(shot(1.5f));
                break;
            case 2:
                transform.localScale = new Vector3(2,2);
                spr.sprite = enemySprite[enemyType];
                spr.color = new Color(0.25f, 0.1f, 0f);
                hp = 1;
                speed = 3f;
                score = 10;
                break;
            case 3:
                spr.sprite = enemySprite[enemyType];
                hp = 1;
                score = 5;
                speed = 4f;
                break;
        }
        if(enemyType != 2 || enemyType != 3)
        {
            hp += GameManager.instance.stage;
        }
        score += GameManager.instance.stage;
        speed += GameManager.instance.stage/2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "pShot")
        {
            hp--;
            Instantiate(dieParticle, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            StartCoroutine(hit());
            if(hp <= 0)
            {
                Die();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyType == 2) {
            transform.Rotate(new Vector3(0, 0, 3), Space.Self);
        }
        if(enemyType == 3)
        {
            if(player != null)
            {
                dir = transform.position - player.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion quat = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, quat, 1.5f * Time.deltaTime);

                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        }
    }

    public void Die()
    {
        if (dropRate > 5)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
        GameManager.instance.playerScore += score;
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        GameManager.instance.AddScore();
        Destroy(gameObject);
    }

    IEnumerator hit()
    {
        source.clip = clip[0];
        source.Play();
        spr.sprite = hitSpr[enemyType];
        yield return new WaitForSeconds(0.05f);
        spr.sprite = enemySprite[enemyType];
    }

    IEnumerator shot(float t)
    {
        yield return new WaitForSeconds(t - GameManager.instance.stage * 0.5f);
        source.clip = clip[1];
        source.Play();
        Instantiate(shots[enemyType], transform.position, Quaternion.identity);
    }
}
