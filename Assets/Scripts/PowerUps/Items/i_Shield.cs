using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Shield : BaseItem {
    
    [SerializeField] GameObject Shield;

    protected override void RunFunctionalityOfItem()
    {
        base.RunFunctionalityOfItem();
        ActivateShield();
        player.Invincible = true;
    }

    void ActivateShield()
    {
        
        GameObject shield = Instantiate(Shield, player.transform);
    }
}
