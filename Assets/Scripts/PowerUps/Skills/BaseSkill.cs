using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This gets attached to the player skill slot
/// </summary>
public class BaseSkill : PowerUpFloating {

    [SerializeField] protected float cooldown = 0.3f;

    // Activate the skill and make use of its benefits
	public virtual void Use()
    {

    }

}
