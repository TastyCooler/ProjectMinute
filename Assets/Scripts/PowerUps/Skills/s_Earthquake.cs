using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Earthquake : BaseSkill {

    //[SerializeField]GameObject medal;
    BoxCollider2D boxColl;
    CircleCollider2D circColl;
    [SerializeField] float riseSpeed;
    [SerializeField] float radiusMax;
    [SerializeField] float knockbackDuration = 1f;
    float radiusOrigin;

    LayerMask enemyLayer;
    
    float waitTime = 0.000001f;

    protected override void Awake()
    {
        base.Awake();
        int layer = LayerMask.NameToLayer("Enemy");
        enemyLayer = 1 << layer;
    }

    protected override void RunFunctionalityOfItem()
    {
        base.RunFunctionalityOfItem();
        Debug.Log("USED");
        //TODO ADD COOLDOWN
        IncreaseTheRadius();
    }

    void IncreaseTheRadius()
    {
        circColl = GetComponent<CircleCollider2D>();
        circColl.radius = radiusOrigin;
        circColl.enabled = true;
        
        if (circColl.radius <= radiusMax)
        {
            Debug.Log("Rise() started");
            StartCoroutine(Rise());
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// This IEnumerator increases the CircleColliders Radius till it reaches radiusMax; After that the Item Destroys itself
    /// </summary>
    /// <returns></returns>
    IEnumerator Rise()
    {
        while (circColl.radius <= radiusMax)
        {
            circColl.radius += riseSpeed * Time.deltaTime;
            Debug.Log(circColl.radius);

            if (circColl.radius >= radiusMax)
            {
                Debug.Log("Rise() stopped");
                StopCoroutine(Rise());
              
                circColl.enabled = false;
            }
            
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO STUN AND DAMAGE ENEMIES

        if(collision.gameObject.layer == enemyLayer)
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage(player.Attack / 2, collision.gameObject.transform.position - player.transform.position, knockbackDuration);
        }
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
