using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public AudioClip[] clip;
    AudioSource source;

    public static Player instance;
    public GameObject cantUse;
    Animator anim;
    public SpriteRenderer spr;
    Vector3 chrSize;
    bool canShot = true;
    public GameObject[] shot;
    public GameObject dieParticle;
    public GameObject shotParticle;
    public Text skill1, skill2;
    public int count1, count2;
    public int shotLv;

    public Slider skillA, skillB;

    public bool isInvisible = false;

    Vector3 min, max;
    public float speed;
    Vector3 move;

    private void Awake()
    {
        instance = this;
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        chrSize = GetComponent<BoxCollider2D>().size/2;

        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        min = Camera.main.ViewportToWorldPoint(new Vector3(0,0));
        max = Camera.main.ViewportToWorldPoint(new Vector3(1,1));
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.A) && canShot)
        {
            source.clip = clip[0];
            source.Play();
            Instantiate(shot[shotLv], transform.position, Quaternion.identity);
            StartCoroutine(shotCd());
        }
        if (Input.GetKey(KeyCode.S) && skillA.value == 0 && count1 != 0)
        {
            GameManager.instance.Heal();
            count1--;
            skill1.text = count1.ToString();
            StartCoroutine(skillCd(skillA));
        }
        else if (skillA.value != 0 && Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.S) && count1 != 0)
        {
            source.clip = clip[2];
            source.Play();
            Animator cU = cantUse.GetComponent<Animator>();
            cU.SetTrigger("show");
        }
        if (Input.GetKey(KeyCode.D) && skillB.value == 0 && count2 != 0)
        {
            GameManager.instance.Bomb();
            count2--;
            skill2.text = count2.ToString();
            StartCoroutine(skillCd(skillB));
        }
        else if(skillB.value != 0 && Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.D) && count2 != 0)
        {
            source.clip = clip[2];
            source.Play();
            Animator cU = cantUse.GetComponent<Animator>();
            cU.SetTrigger("show");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !isInvisible)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Die();
            StartCoroutine(pInvisible());
            GameManager.instance.hp.value -= 3;
            if (GameManager.instance.hp.value <= 0)
            {
                Die();
            }
        }
        else if(collision.tag == "Boss" && !isInvisible)
        {
            StartCoroutine(pInvisible());
            GameManager.instance.hp.value -= 3;
            if (GameManager.instance.hp.value <= 0)
            {
                Die();
            }
        }
        else if(collision.tag == "eShot" && !isInvisible)
        {
            StartCoroutine(pInvisible());
            GameManager.instance.hp.value -= 2;
            Instantiate(shotParticle, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            if (GameManager.instance.hp.value <= 0)
            {
                Die();
            }
        }
        else if(collision.tag == "Item")
        {
            Item item = collision.GetComponent<Item>();
            GameManager.instance.useItem(item.itemType);
            Destroy(collision.gameObject);
        }
    }

    public void Die()
    {
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        GameManager.instance.playerScore += (int)GameManager.instance.hp.value * 10;
        GameManager.instance.ShowResult(true);
        Destroy(gameObject);
    }

    public IEnumerator pInvisible()
    {
        if (!isInvisible)
        {
            source.clip = clip[1];
            source.Play();
            isInvisible = true;
            spr.color = new Color(0.4f, 0.4f, 0.8f, 0.6f);
            yield return new WaitForSeconds(1f);
            isInvisible = false;
            spr.color = new Color(1, 1, 1, 1f);
        }
    }

    IEnumerator skillCd(Slider slider)
    {
        slider.value = slider.maxValue;
        while (slider.value != 0)
        {
            yield return new WaitForSeconds(1f);
            slider.value--;
        }
    }

    IEnumerator shotCd()
    {
        canShot = false;
        yield return new WaitForSeconds(0.5f);
        canShot = true;
    }

    void Move()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        transform.position += move.normalized * speed * Time.deltaTime;

        Vector3 newVec = transform.position;
        newVec.x = Mathf.Clamp(newVec.x, min.x+chrSize.x, max.x- chrSize.x);
        newVec.y = Mathf.Clamp(newVec.y, min.y+ chrSize.y, max.y- chrSize.y);
        transform.position = newVec;

        anim.SetInteger("xInput", (int)move.x);
    }
}
