using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Input interface for the Player Inputs
/// </summary>
public interface IInput
{
    // Axis for the movement
    float Horizontal { get; }
    float Vertical { get; }

    // Button for player attack
    bool Attack { get; }

    // Use the item, the player hast equipped
    bool UseItem { get; }

    // Use the skill, the player has equipped
    bool UseSkill { get; }

    bool SwitchPowerup { get; }
}
