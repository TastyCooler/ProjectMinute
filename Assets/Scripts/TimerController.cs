using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

    [SerializeField] Text timerText;

    private void Awake()
    {
        GameManager.Instance.OnTimerChanged += ChangeTimer;
    }

    void ChangeTimer(int newTimer)
    {
        if(timerText)
        {
            timerText.text = newTimer.ToString();
        }
    }

}
