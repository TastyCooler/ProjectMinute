using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAppearance : PowerUpFloating
{

    public BaseItem Item
    {
        get
        {
            return itemToAttach;
        }
    }

    [SerializeField] BaseItem itemToAttach;
    
}
