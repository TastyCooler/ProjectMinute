using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PauseMenu : BaseMenu {

    public bool IsShown
    {
        get
        {
            return isShown;
        }
        set
        {
            if(value)
            {
                Show();
            }
            else
            {
                Hide();
            }
            isShown = value;
        }
    }

    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
        set
        {
            anim.SetBool("GameOver", value);
            if(value && gameOverSound)
            {
                gameOverSound.Play();
            }
            isGameOver = value;
        }
    }

    Animator anim;
    //Animator camAnim;

    bool isGameOver = false;

    bool isShown = false;

    [SerializeField] Text highscoreText;

    [SerializeField] AudioSource gameOverSound;

    [SerializeField] AnimationClip quitClip;

    [SerializeField] Button restartButton;

    int playerScore = 0;
    [SerializeField] string playerName = "";

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //camAnim = Camera.main.GetComponent<Animator>();
        GameManager.Instance.OnWinScreen += ShowWinScreen;

    }

    public void OnQuitButton()
    {
        //Time.timeScale = 1f;
        buttonSound.Play();
        StartCoroutine(QuitAfterSeconds(quitClip.length));
    }

    IEnumerator QuitAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetTrigger("Quit");
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }

    void ShowWinScreen(int highscore)
    {
        playerScore = highscore;
        if(highscoreText)
        {
            highscoreText.text = "Score: " + highscore;
        }
        Cursor.visible = true;
        StartCoroutine(WinScreenAfterSeconds(2f));
        //StartCoroutine(EnterHighscore());
    }

    IEnumerator EnterHighscore()
    {
        string url = NetworkScript.Instance.HighscoreURL + "?player=" + playerName + "&newScore=" + playerScore;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        NetworkScript.Instance.UpdateHighscoreList();
        yield return new WaitForSeconds(1f);
        ShowTopTen();
    }

    void ShowTopTen()
    {
        // Show highscore list
        for (int i = 0; i < NetworkScript.Instance.HighscoreList.Length; i++)
        {
            Debug.Log(NetworkScript.Instance.HighscoreList[i]);
        }
    }

    IEnumerator WinScreenAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.SetTrigger("Win");
        restartButton.Select();
    }

    public void OnRestartButton()
    {
        //Time.timeScale = 1f;
        IsGameOver = false;
        buttonSound.Play();
        StartCoroutine(LoadSceneAfterSeconds(quitClip.length));
    }

    IEnumerator LoadSceneAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetTrigger("Quit");
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Show()
    {
        anim.SetTrigger("Show");
        //camAnim.enabled = true;
        //camAnim.SetTrigger("Pause");
        Cursor.visible = true;
        restartButton.Select();
    }

    void Hide()
    {
        anim.SetTrigger("Hide");
        //camAnim.enabled = true;
        //camAnim.SetTrigger("Play");
        Cursor.visible = false;
    }

}
