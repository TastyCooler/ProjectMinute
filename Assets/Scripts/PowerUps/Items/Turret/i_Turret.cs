using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Turret : BaseItem {

    [SerializeField] int usages = 1;
    [SerializeField] GameObject turret;
    public override void Use()
    {
        if (usageTimes <= usages)
        {
            usages--;
            Spawn();
        }
    }


    void Spawn()
    {
        // If it should be a portable tower Prototype =>
        // Vector3 offsetter = new Vector3(1f, 1f, 0);   
        // Vector3 offset = gameObject.transform.position - offsetter;

        GameObject tower = Instantiate(turret, gameObject.transform);
        tower.transform.SetParent(null, true);
    }
}
