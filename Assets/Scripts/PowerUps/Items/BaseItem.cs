using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This gets attached to the player item slot
/// </summary>
public class BaseItem : PowerUpFloating {

    [SerializeField] protected int usageTimes = 1;

    // Activate the item and make use of its benefits
    public virtual void Use()
    {

    }

}
