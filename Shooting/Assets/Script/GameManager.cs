using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioClip[] clip;
    AudioSource source;

    public SpriteRenderer bg;
    public AudioSource bgSource;
    public Sprite bgSpr;
    public Text currentStage;
    public int stage;
    public Text scoreT, timeT;
    public int time;
    public InputField inputField;
    public GameObject continueButton;
    public GameObject result;

    public int playerScore;
    public Text currentScore;

    public GameObject shotParticle;

    bool bossSpawned = false;
    public float bossSpawnTime;
    public GameObject boss;
    public GameObject bossWarning;
    public Slider bossHp;

    public int invCount;

    Vector3 min, max;
    public static GameManager instance;
    public GameObject enemy;

    public Slider hp, fuel;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1));
        max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
    }
    void Start()
    {
        currentStage.text = "스테이지 " + (stage + 1);
        currentStage.gameObject.SetActive(true);
        Time.timeScale = 1;
        StartCoroutine(spawnEnemy());
        StartCoroutine(useFuel());
        StartCoroutine(bossCount());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) { Cheat(0); }
        if (Input.GetKeyDown(KeyCode.F2)) { Cheat(1); }
        if (Input.GetKeyDown(KeyCode.F3)) { Cheat(2); }
        if (Input.GetKeyDown(KeyCode.F4)) { Cheat(3); }
        if (Input.GetKeyDown(KeyCode.F5)) { Cheat(4); }
        if (Input.GetKeyDown(KeyCode.F6)) { Cheat(5); }
    }

    public void Cheat(int n)
    {
        switch (n)
        {
            case 0:
                GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject obj1 in enemy)
                {
                    Enemy enim = obj1.GetComponent<Enemy>();
                    enim.Die();
                }
                GameObject[] shot = GameObject.FindGameObjectsWithTag("eShot");
                foreach (GameObject obj2 in shot)
                {
                    Instantiate(shotParticle, obj2.transform.position, Quaternion.identity);
                    Destroy(obj2);
                }
                break;
            case 1:
                Player.instance.shotLv = 3;
                break;
            case 2:
                Player.instance.skillA.value = 0;
                Player.instance.skillB.value = 0;
                Player.instance.count1 = 2;
                Player.instance.count2 = 2;
                Player.instance.skill1.text = Player.instance.count1.ToString();
                Player.instance.skill2.text = Player.instance.count2.ToString();
                break;
            case 3:
                hp.value = hp.maxValue; break;
            case 4:
                fuel.value = fuel.maxValue; break;
            case 5:
                if(stage != 1)
                {
                    ResetStage();
                }
                break;
        }
    }
    public void AddScore()
    {
        currentScore.text = playerScore.ToString() + "P";
    }

    public void ResetStage()
    {
        Time.timeScale = 1;
        if (boss.activeSelf)
        {
            Boss.instance.resetBoss();
        }
        
        bossHp.gameObject.SetActive(false);
        stage++;
        bg.sprite = bgSpr;
        result.SetActive(false);
        currentStage.text = "스테이지 " + (stage+1);
        currentStage.gameObject.SetActive(false);
        currentStage.gameObject.SetActive(true);
        StartCoroutine(spawnEnemy());
        bossSpawned = false;
        StartCoroutine(bossCount());
    }

    public void useItem(int type)
    {
        switch (type) 
        {
            case 0:
                if(Player.instance.shotLv < 3)
                {
                    Player.instance.shotLv++;
                }
                break;
            case 1:
            case 2:
                StartCoroutine(itemInv());
                break;
            case 3:
            case 4:
                Heal();
                break;
            default:
                fuel.value += 40;
                source.clip = clip[1];
                source.Play();
                break;
        }

    }

    public void ShowResult(bool isDie)
    {
        scoreT.text = "스코어 : " + playerScore;
        timeT.text = "소요시간 : " + time / 60 + "분 " + time % 60 + "초";
        if (!isDie)
        {
            continueButton.SetActive(true);
            source.clip = clip[3];
            source.Play();
        }
        if(stage == 1)
        {
            continueButton.SetActive(false);
        }
        StartCoroutine(timeStop());
        result.SetActive(true);
    }
    IEnumerator timeStop()
    {
        yield return new WaitForSeconds(0.6f);
        Time.timeScale = 0f;
    }

    public IEnumerator itemInv()
    {
        StopCoroutine(Player.instance.pInvisible());
        if(Player.instance.isInvisible == true)
        {
            invCount = 0;
            yield break;
        }
        invCount = 0;
        Player.instance.isInvisible = true;
        Player.instance.spr.color = new Color(0.4f, 0.4f, 0.8f, 0.6f);
        while (invCount != 4)
        {
            yield return new WaitForSeconds(1f);
            invCount++;
        }
        Player.instance.isInvisible = false;
        Player.instance.spr.color = new Color(1, 1, 1, 1f);
    }

    public void Heal()
    {
        hp.value += 5;
        source.clip = clip[0];
        source.Play();
    }
    public void Bomb()
    {
        source.clip = clip[2];
        source.Play();
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj1 in enemy)
        {
            Enemy enim = obj1.GetComponent<Enemy>();
            enim.Die();
        }
        GameObject[] shot = GameObject.FindGameObjectsWithTag("eShot");
        foreach (GameObject obj2 in shot)
        {
            Instantiate(shotParticle, obj2.transform.position, Quaternion.identity);
            Destroy(obj2);
        }
    }

    IEnumerator bossCount()
    {
        if(stage == 1)
        {
            bossHp.maxValue *= 2;
            bossHp.value = bossHp.maxValue;
            yield return new WaitForSeconds(bossSpawnTime - stage*20);
        }
        yield return new WaitForSeconds(bossSpawnTime);
        bossSpawned = true;
        StartCoroutine(spawnBoss());
    }

    IEnumerator spawnBoss()
    {
        if (bossSpawned)
        {
            bgSource.clip = clip[4];
            bgSource.Play();
            bossHp.gameObject.SetActive(true);
            bossWarning.SetActive(true);
            yield return new WaitForSeconds(7f);
            bossWarning.SetActive(false);
            boss.SetActive(true);
        }
    }

    IEnumerator useFuel()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            time++;
            fuel.value--;
            if(fuel.value <= 0)
            {
                Player.instance.Die();
            }
        }
    }
    IEnumerator spawnEnemy()
    {
        while (true)
        {
            if (bossSpawned)
            {
                yield break;
            }
            yield return new WaitForSeconds(2.5f);
            Instantiate(enemy, new Vector3(UnityEngine.Random.Range(min.x+1, max.x-1), min.y+2, 0), Quaternion.identity);
        }
    }
}
