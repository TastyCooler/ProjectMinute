using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Shield : BaseItem {

    [SerializeField] int usages = 1;
    [SerializeField] GameObject Shield;

    public override void Use()
    {
        if (usageTimes <= usages)
        {
            usages--;
            //Debug.Log("Shield used");
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        GameObject shield = Instantiate(Shield, player.transform);
        
    }
}
