using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //camAnim = Camera.main.GetComponent<Animator>();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnRestartButton()
    {
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
