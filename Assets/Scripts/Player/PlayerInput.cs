using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The implementation of all the inputs, the player can give
/// </summary>
public class PlayerInput : MonoBehaviour, IInput {

    #region Fields

    #endregion

    #region Input Properties

    // The input for horizontal movement
    public float Horizontal
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }

    // The input for vertical movement
    public float Vertical
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }

    // Button for player attack
    public bool Attack
    {
        get
        {
            return Input.GetButtonDown("Attack");
        }
    }

    // Use the item, the player hast equipped
    public bool UseItem
    {
        get
        {
            return Input.GetButtonDown("UseItem");
        }
    }

    // Use the skill, the player has equipped
    public bool UseSkill
    {
        get
        {
            return Input.GetButtonDown("UseSkill");
        }
    }

    public bool SwitchPowerup
    {
        get
        {
            return Input.GetButtonDown("SwitchPowerup");
        }
    }

    #endregion

}
