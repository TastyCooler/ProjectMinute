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

    // Axis for the aiming
    float R_Horizontal { get; }
    float R_Vertical { get; }

    // Shooting buttons
    bool Shoot { get; }

    // The reload axis
    bool Reload { get; }

    // Toggles the flashlight on or off
    bool ToggleFlashlight { get; }

    // The buttons for the items, the player has equipped
    bool UseItem1 { get; }
    bool UseItem2 { get; }
    bool UseItem3 { get; }

    // Reset cam to look forward with those buttons
    bool ResetCam { get; }

    // Back Button to get out or into menu
    bool Cancel { get; }
}
