using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The implementation of all the inputs, the player can give
/// </summary>
public class PlayerInput : MonoBehaviour, IInput {

    #region Fields

    bool bShootInUse = false;
    bool bReloadInUse = false;
    bool bFlashlightInUse = false;
    bool bCancelInUse = false;
    bool bResetCamInUse = false;
    bool bItem1InUse = false;
    bool bItem2InUse = false;
    bool bItem3InUse = false;

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

    // The input for horizontal aiming
    public float R_Horizontal
    {
        get
        {
            return Input.GetAxis("RHorizontal");
        }
    }

    // The input for vertical aiming
    public float R_Vertical
    {
        get
        {
            return Input.GetAxis("RVertical");
        }
    }

    public bool Shoot
    {
        get
        {
            if (Input.GetAxisRaw("Shoot") != 0)
            {
                if (bShootInUse == false)
                {
                    bShootInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("Shoot") == 0)
            {
                bShootInUse = false;
            }
            return false;
        }
    }

    public bool Reload
    {
        get
        {
            if (Input.GetAxisRaw("Reload") != 0)
            {
                if (bReloadInUse == false)
                {
                    bReloadInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("Reload") == 0)
            {
                bReloadInUse = false;
            }
            return false;
        }
    }

    public bool ToggleFlashlight
    {
        get
        {
            if (Input.GetAxisRaw("ToggleFlashlight") != 0)
            {
                if (bFlashlightInUse == false)
                {
                    bFlashlightInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("ToggleFlashlight") == 0)
            {
                bFlashlightInUse = false;
            }
            return false;
        }
    }

    public bool UseItem1
    {
        get
        {
            if (Input.GetAxisRaw("UseItem1") != 0)
            {
                if (bItem1InUse == false)
                {
                    bItem1InUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("UseItem1") == 0)
            {
                bItem1InUse = false;
            }
            return false;
        }
    }

    public bool UseItem2
    {
        get
        {
            if (Input.GetAxisRaw("UseItem2") != 0)
            {
                if (bItem2InUse == false)
                {
                    bItem2InUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("UseItem2") == 0)
            {
                bItem2InUse = false;
            }
            return false;
        }
    }

    public bool UseItem3
    {
        get
        {
            if (Input.GetAxisRaw("UseItem3") != 0)
            {
                if (bItem3InUse == false)
                {
                    bItem3InUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("UseItem3") == 0)
            {
                bItem3InUse = false;
            }
            return false;
        }
    }

    public bool ResetCam
    {
        get
        {
            if (Input.GetAxisRaw("ResetCam") != 0)
            {
                if (bResetCamInUse == false)
                {
                    bResetCamInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("ResetCam") == 0)
            {
                bResetCamInUse = false;
            }
            return false;
        }
    }

    public bool Cancel
    {
        get
        {
            if (Input.GetAxisRaw("Cancel") != 0)
            {
                if (bCancelInUse == false)
                {
                    bCancelInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("Cancel") == 0)
            {
                bCancelInUse = false;
            }
            return false;
        }
    }

    #endregion

}
