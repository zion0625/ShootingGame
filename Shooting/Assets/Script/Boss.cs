using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Boss : MonoBehaviour
{
    public static Boss instance;

    public AudioClip[] clip;
    AudioSource source;

    public GameObject item;
    public int dropRate;
    public GameObject dieParticle;
    public GameObject shotParticle;

    SpriteRenderer spr;
    public Sprite hitSpr;

    Vector3 min, max;
    public int nextMove;
    public float nextVec;
    public float speed;
    public GameObject[] shots;
    public GameObject[] upgradeShots;
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        min = Camera.main.ViewportToWorldPoint(new Vector3(0,1));
        max = Camera.main.ViewportToWorldPoint(new Vector3(1,1));
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(nextVec, transform.position.y, 0), speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "pShot")
        {
            GameManager.instance.bossHp.value--;
            Instantiate(shotParticle, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            hit();
            if (GameManager.instance.bossHp.value <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        if (dropRate > 5)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
        GameManager.instance.playerScore += 200;
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        GameManager.instance.playerScore += (int)GameManager.instance.hp.value * 10;
        GameManager.instance.ShowResult(false);
        GameManager.instance.AddScore();
    }
    public void resetBoss() 
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(0, min.y - 1);
    }

    void hit()
    {
        source.clip = clip[0];
        source.Play();
        spr.sprite = hitSpr;
    }

    IEnumerator Act()
    {
        while (true)
        {
            nextMove = Random.Range(0, shots.Length);
            Shot();
            yield return new WaitForSeconds(2.5f);
            nextVec = Random.Range(min.x+3, max.x-3);
            source.clip = clip[1];
            source.Play();
        }
    }
    void Shot()
    {
        switch (nextMove)
        {
            case 0:
            case 1:
                CreateBullet();
                break;
            case 2:
                CreateArea();
                break;
        }
    }
    void CreateArea()
    {
        source.clip = clip[1];
        source.Play();
        if (GameManager.instance.stage == 1)
        {
            Instantiate(upgradeShots[nextMove], new Vector3(Random.Range((int)min.x+1, (int)max.x-1), 0), Quaternion.identity);
            Instantiate(upgradeShots[nextMove], new Vector3(Random.Range((int)min.x+1, (int)max.x-1), 0), Quaternion.identity);
            Instantiate(upgradeShots[nextMove], new Vector3(Random.Range((int)min.x+1, (int)max.x-1), 0), Quaternion.identity);
        }
        else
        {
            Instantiate(upgradeShots[nextMove], new Vector3(Random.Range((int)min.x+1, (int)max.x-1), 0), Quaternion.identity);
        }
    }
    void CreateBullet()
    {
        source.clip = clip[1];
        source.Play();
        if (GameManager.instance.stage == 1)
        {
            Instantiate(upgradeShots[nextMove], transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(shots[nextMove], transform.position, Quaternion.identity);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(Act());
        Debug.Log("a");
    }
}
