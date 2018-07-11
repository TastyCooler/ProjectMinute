using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : BaseEnemy {

    public event System.Action OnBossDefeated;

    bool isDead = false;

    [SerializeField] BoxCollider2D coll;

    private void OnDestroy()
    {
        if(OnBossDefeated != null)
        {
            OnBossDefeated();
        }
    }

    protected override void Update()
    {
        if(isDead) { return; }
        base.Update();
    }

    protected override void Die()
    {
        if(anim)
        {
            anim.SetTrigger("Death");
        }
        if(OnBossDefeated != null)
        {
            OnBossDefeated();
        }
        GameManager.Instance.Highscore += highscoreValue;
        player.GainExp(expToGive);
        isDead = true;
        coll.enabled = false;
    }

}
