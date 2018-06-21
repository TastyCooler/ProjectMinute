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

    #region Fields

    [SerializeField] Canvas pauseMenu;

    bool isControllerInput = false;
    int controllerCount = 0;

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
    }

    #endregion

    #region Helper Methods

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
