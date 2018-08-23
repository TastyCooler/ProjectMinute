using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : BaseEnemy {

    public int Attack
    {
        get
        {
            return attack;
        }
    }

    public float PlayerKnockedBackFor
    {
        get
        {
            return playerKnockedBackFor;
        }
    }

    public event System.Action OnBossDefeated;

    [SerializeField] float attackReach;

    [SerializeField] float playerKnockedBackFor = 0.5f;

    bool isDead = false;

    [SerializeField] BoxCollider2D coll;

    [SerializeField] GameObject[] enemies;
    [SerializeField] float spawnDistance = 10f;

    protected override void Awake()
    {
        base.Awake();
        InvokeRepeating("SpawnEnemy", 3f, 3f);
    }

    void SpawnEnemy()
    {
        if(enemies.Length > 0)
        {
            int enemyIndice = Random.Range(0, enemies.Length - 1);
            Instantiate(enemies[enemyIndice], transform.position + (Vector3)Random.insideUnitCircle * spawnDistance, transform.rotation);
        }
    }

    private void OnDestroy()
    {
        if(OnBossDefeated != null)
        {
            OnBossDefeated();
        }
        CancelInvoke();
    }

    protected override void Update()
    {
        if(isDead) { return; }
        base.Update();
        if(toPlayer.magnitude <= attackReach)
        {
            anim.SetTrigger("Attack");
        }

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

    public void CamShakeWhenDead()
    {
        camShake.shakeAmount = camShakeAmountWhenDamaged * 2f;
        camShake.shakeDuration = camShakeDurationWhenDamaged;
    }



}
