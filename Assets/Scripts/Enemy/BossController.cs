using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    public event System.Action OnBossDefeated;

    private void OnDestroy()
    {
        if(OnBossDefeated != null)
        {
            OnBossDefeated();
        }
    }

    private void Awake()
    {
        GameManager.Instance.IsBossSpawned = true;
    }

}
