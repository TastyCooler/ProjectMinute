using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : BaseEnemy {

    public event System.Action OnBossDefeated;

    private void OnDestroy()
    {
        if(OnBossDefeated != null)
        {
            OnBossDefeated();
        }
    }

    protected override void Die()
    {
        if(OnBossDefeated != null)
        {
            OnBossDefeated();
        }
        GameManager.Instance.Highscore += highscoreValue;
        player.GainExp(expToGive);
        Destroy(gameObject);
    }

}
