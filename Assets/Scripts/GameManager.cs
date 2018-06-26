using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameManager script, which controls several aspects of the game unaffected by loading scenes etc.
/// </summary>
public class GameManager : Singleton<GameManager> {

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

    public bool IsBossSpawned
    {
        get
        {
            return isBossSpawned;
        }
        set
        {
            isBossSpawned = value;
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>().OnBossDefeated += SettleHighscore;
        }
    }

    #region Fields

    [SerializeField] Canvas pauseMenu;

    bool isControllerInput = false;
    int controllerCount = 0;

    int highscore = 0;
    int highscoreAddition = 6000;
    int finalHighscore;

    bool isBossSpawned = false;

    #endregion

    #region Unity Messages

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        }
        GetControllerCount();
        if(controllerCount > 0 && !isControllerInput)
        {
            isControllerInput = true;
        }
        else if(controllerCount < 1 && isControllerInput)
        {
            isControllerInput = false;
        }
        if(highscoreAddition > 0)
        {
            highscoreAddition -= (int)(Time.deltaTime * 100f);
        }
        else
        {
            highscoreAddition = 0;
        }
        print(highscoreAddition);
    }

    #endregion

    #region Helper Methods

    void SettleHighscore()
    {
        finalHighscore = highscore + highscoreAddition;
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
