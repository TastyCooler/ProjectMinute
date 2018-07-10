using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlescreenController : BaseMenu {

    Animator anim;
    [SerializeField] Animator overlayAnim;
    public event System.Action OnGameStarted;

    //[SerializeField] AudioSource menuSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnQuitButton()
    {
        buttonSound.Play();
        Application.Quit();
    }

    public void DisableTItlescreen()
    {
        gameObject.SetActive(false);
    }

    public void OnPlayButton()
    {
        buttonSound.Play();
        Cursor.visible = false;
        anim.SetTrigger("Hide");
        overlayAnim.SetTrigger("FadeIn");
        if(OnGameStarted != null)
        {
            OnGameStarted();
        }
    }
}
