﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

/// <summary>
/// The GameManager script, which controls several aspects of the game unaffected by loading scenes etc.
/// </summary>
public class GameManager : Singleton<GameManager>
{

    public bool IsControllerInput
    {
        get
        {
            return isControllerInput;
        }
    }

    public int Highscore
    {
        get
        {
            return highscore;
        }
        set
        {
            if (value > highscore)
            {
                highscore = value;
            }
            else
            {
                Debug.LogWarning("Highscore cannot be decreased");
            }
        }
    }

    public bool IsStarted
    {
        get
        {
            return isStarted;
        }
    }

    #region Fields

    public event System.Action<int> OnTimerChanged;

    public event System.Action<int> OnWinScreen;

    [SerializeField] int maxStack = 50;

    [SerializeField] GameObject arrow;
    [SerializeField] GameObject arrowParent;
    Stack<GameObject> arrowStack = new Stack<GameObject>();

    [SerializeField] GameObject laser;
    [SerializeField] GameObject laserParent;
    Stack<GameObject> laserStack = new Stack<GameObject>();

    [SerializeField] GameObject hook;
    [SerializeField] GameObject hookParent;
    Stack<GameObject> hookStack = new Stack<GameObject>();

    GameObject player;

    [SerializeField] PostProcessingProfile bossPost;

    [SerializeField] int preparationTime = 60;
    int timer;
    bool isPreparing = false;

    [SerializeField] AudioSource preparationMusic;

    [SerializeField] GameObject boss;

    [SerializeField] PauseMenu pauseMenu;

    [SerializeField] ParticleSystem flashUpParticle;

    [SerializeField] Vector2 destructionSizeOnBossSpawn;

    [SerializeField] GameObject impactCrater;

    [SerializeField] Animator overlayAnim;

    bool isControllerInput = false;
    int controllerCount = 0;

    int highscore = 0;
    int highscoreAddition = 6000;
    int finalHighscore;

    bool isBossSpawned = false;

    bool isAlreadyDead = false;

    CameraShake camShake;

    [SerializeField] float camShakeDurationBossSpawn = 1f;
    [SerializeField] float camShakeAmountBossSpawn = 0.5f;

    bool isStarted = false; // Stores if the counter is started or not

    #endregion

    #region Unity Messages

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
        for (int i = 0; i < maxStack; i++)
        {
            GameObject newArrow = Instantiate(arrow, transform.position, transform.rotation);
            if (arrowParent)
            {
                newArrow.transform.parent = arrowParent.transform;
            }
            newArrow.SetActive(false);
            arrowStack.Push(newArrow);

            GameObject newLaser = Instantiate(laser, transform.position, transform.rotation);
            if (laserParent)
            {
                newLaser.transform.parent = laserParent.transform;
            }
            newLaser.SetActive(false);
            laserStack.Push(newLaser);

            GameObject newHook = Instantiate(hook, transform.position, transform.rotation);
            if (hookParent)
            {
                newHook.transform.parent = hookParent.transform;
            }
            newHook.SetActive(false);
            hookStack.Push(newHook);
        }
        timer = preparationTime;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().OnPlayerDied += OnPlayerDied;
        player.GetComponent<PlayerController>().OnSummonBossEarly += CallBossEarly;
        camShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Start()
    {
        if (OnTimerChanged != null)
        {
            OnTimerChanged(preparationTime);
        }
    }

    private void Update()
    {
        if (timer <= 0 && isPreparing)
        {
            CallBoss();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            pauseMenu.IsShown = !pauseMenu.IsShown;
        }
        GetControllerCount();
        if (controllerCount > 0 && !isControllerInput)
        {
            isControllerInput = true;
        }
        else if (controllerCount < 1 && isControllerInput)
        {
            isControllerInput = false;
        }
        if (highscoreAddition > 0)
        {
            highscoreAddition -= (int)(Time.deltaTime * 100f);
        }
        else
        {
            highscoreAddition = 0;
        }

        //Debug.LogFormat("arrowStack {0}, laserStack {1}", arrowStack.Count, laserStack.Count);
    }

    private void CallBoss()
    {
        isPreparing = false;
        CancelInvoke();
        if (bossPost)
        {
            Camera.main.GetComponent<PostProcessingBehaviour>().profile = bossPost;
        }
        InvokeRepeating("IncreaseTimer", 1f, 1f);
        StartCoroutine(SummonBoss());
    }

    void CallBossEarly()
    {
        timer = 0;
        if (OnTimerChanged != null)
        {
            OnTimerChanged(timer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!isStarted && collision.gameObject.tag == "Player")
        {
            InvokeRepeating("DecreaseTimer", 0f, 1.03f); // Yes this causes the game to run longer than one minute. I'm a ninja
            isPreparing = true;
            if (preparationMusic)
            {
                preparationMusic.Play();
            }
            isStarted = true;
        }
    }

    #endregion

    #region Helper Methods

    public void PushHook(GameObject newObject)
    {
        newObject.SetActive(false);
        hookStack.Push(newObject);
    }

    public GameObject GetHook(Vector3 pos)
    {
        GameObject hookReturned = hookStack.Pop();
        hookReturned.transform.position = pos;
        hookReturned.SetActive(true);
        return hookReturned;
    }

    IEnumerator SummonBoss()
    {
        flashUpParticle.transform.position = new Vector3(player.transform.position.x + 6, player.transform.position.y);
        flashUpParticle.Play();
        yield return new WaitForSeconds(1.5f);
        if(impactCrater)
        {
            Instantiate(impactCrater, new Vector3(player.transform.position.x + 6, player.transform.position.y), transform.rotation);
        }
        camShake.shakeAmount = camShakeAmountBossSpawn;
        camShake.shakeDuration = camShakeDurationBossSpawn;
        Collider2D[] colls = Physics2D.OverlapBoxAll(player.transform.position, destructionSizeOnBossSpawn, 0f);
        foreach (Collider2D coll in colls)
        {
            if (coll.gameObject.tag != "Player" && coll.gameObject.tag != "GameManager" && coll.gameObject.tag != "Crater")
            {
                Destroy(coll.gameObject);
            }
        }
        GameObject newBoss = Instantiate(boss, new Vector3(player.transform.position.x + 6, player.transform.position.y), transform.rotation);
        newBoss.GetComponent<BossController>().OnBossDefeated += OnBossDefeated;
    }

    void DecreaseTimer()
    {
        timer--;
        if (OnTimerChanged != null)
        {
            OnTimerChanged(timer);
        }
    }

    void OnPlayerDied()
    {
        if (!isAlreadyDead)
        {
            overlayAnim.SetTrigger("FadeOut");
            pauseMenu.IsShown = true;
            pauseMenu.IsGameOver = true;
            //Time.timeScale = 0f;
            isAlreadyDead = true;
        }
    }

    void IncreaseTimer()
    {
        timer++;
        if (OnTimerChanged != null)
        {
            OnTimerChanged(timer);
        }
    }

    public void PushArrow(GameObject newObject)
    {
        newObject.SetActive(false);
        arrowStack.Push(newObject);
    }

    public GameObject GetArrow(Vector3 pos)
    {
        GameObject arrowReturned = arrowStack.Pop();
        arrowReturned.transform.position = pos;
        arrowReturned.SetActive(true);
        return arrowReturned;
    }

    public void PushLaser(GameObject newObject)
    {
        newObject.SetActive(false);
        laserStack.Push(newObject);
    }

    public GameObject GetLaser(Vector3 pos)
    {
        GameObject laserReturned = laserStack.Pop();
        laserReturned.transform.position = pos;
        laserReturned.SetActive(true);
        return laserReturned;
    }

    void OnBossDefeated()
    {
        if(overlayAnim)
        {
            overlayAnim.SetTrigger("FadeOut");
        }
        finalHighscore = highscore + highscoreAddition;
        if(OnWinScreen != null)
        {
            OnWinScreen(finalHighscore);
        }
    }

    // Looks for any connected controller and updates the counter for every connected controller
    void GetControllerCount()
    {
        string[] names = Input.GetJoystickNames();
        controllerCount = 0;
        for (int i = 0; i < names.Length; i++)
        {
            if (!string.IsNullOrEmpty(names[i]))
            {
                controllerCount++;
            }
        }
    }

    #endregion

}