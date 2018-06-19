using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour {

    [SerializeField] float cooldown = 0.3f;

    // Activate the skill and make use of its benefits
	public virtual void Use()
    {

    }

}
