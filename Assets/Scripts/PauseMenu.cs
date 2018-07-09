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

    Animator anim;

    bool isShown = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
    }

    void Hide()
    {
        anim.SetTrigger("Hide");
    }

}
