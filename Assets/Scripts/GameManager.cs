using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameManager script, which controls several aspects of the game unaffected by loading scenes etc.
/// </summary>
public class GameManager : Singleton<GameManager> {

    #region Fields

    [SerializeField] Canvas pauseMenu;

    #endregion

    #region Unity Messages

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        }
    }

    #endregion

    #region Helper Methods

    #endregion

}
