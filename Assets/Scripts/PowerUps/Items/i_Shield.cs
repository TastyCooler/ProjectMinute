using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Shield : BaseItem {
    
    [SerializeField] GameObject Shield;

    public override void Use()
    {
        if (usageTimes > 0)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        usageTimes--;
        GameObject shield = Instantiate(Shield, player.transform);
    }
}
