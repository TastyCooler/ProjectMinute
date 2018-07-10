using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlescreenController : MonoBehaviour {

    Animator anim;
    [SerializeField] Animator overlayAnim;
    public event System.Action OnGameStarted;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void DisableTItlescreen()
    {
        gameObject.SetActive(false);
    }

    public void OnPlayButton()
    {
        Cursor.visible = false;
        anim.SetTrigger("Hide");
        overlayAnim.SetTrigger("FadeIn");
        if(OnGameStarted != null)
        {
            OnGameStarted();
        }
    }
}
