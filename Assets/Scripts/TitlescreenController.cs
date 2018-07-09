using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlescreenController : MonoBehaviour {

    Animator anim;

    public event System.Action OnGameStarted;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnPlayButton()
    {
        anim.SetTrigger("Hide");
        if(OnGameStarted != null)
        {
            OnGameStarted();
        }
    }
}
