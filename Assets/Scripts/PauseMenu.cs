using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            isGameOver = value;
        }
    }

    Animator anim;
    //Animator camAnim;

    bool isGameOver = false;

    bool isShown = false;

    [SerializeField] AnimationClip quitClip;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //camAnim = Camera.main.GetComponent<Animator>();
    }

    public void OnQuitButton()
    {
        buttonSound.Play();
        anim.SetTrigger("Quit");
        StartCoroutine(QuitAfterSeconds(quitClip.length));
    }

    IEnumerator QuitAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }

    public void OnRestartButton()
    {
        buttonSound.Play();
        anim.SetTrigger("Quit");
        StartCoroutine(LoadSceneAfterSeconds(quitClip.length));
    }

    IEnumerator LoadSceneAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Show()
    {
        anim.SetTrigger("Show");
        //camAnim.enabled = true;
        //camAnim.SetTrigger("Pause");
        Cursor.visible = true;
    }

    void Hide()
    {
        anim.SetTrigger("Hide");
        //camAnim.enabled = true;
        //camAnim.SetTrigger("Play");
        Cursor.visible = false;
    }

}
