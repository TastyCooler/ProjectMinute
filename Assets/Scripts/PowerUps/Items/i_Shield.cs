using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Shield : BaseItem {
    
    [SerializeField] GameObject Shield;

    protected override void RunFunctionalityOfItem()
    {
        if (usageTimes > 0)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        GameObject shield = Instantiate(Shield, player.transform);
    }
}
