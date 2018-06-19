using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAppearance : PowerUpFloating
{
    public BaseSkill Skill
    {
        get
        {
            return skillToAttach;
        }
    }

    [SerializeField] BaseSkill skillToAttach;

	
}
