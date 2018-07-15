using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_instakill : BaseItem {

    //[SerializeField]GameObject medal;
    BoxCollider2D boxColl;
    CircleCollider2D circColl;
    [SerializeField] float riseSpeed;
    [SerializeField] float radiusMax;
    float waitTime = 0.000001f;

    [SerializeField] ParticleSystem shine;
    ParticleSystem.EmissionModule shineEmission;

    public override void Use()
    {
        
        if(usageTimes > 0)
        {
            shine.transform.position = player.transform.position;
            shineEmission = shine.emission;
            shine.Play();
            //shine.Stop();
           // Debug.Log("Instakill used");
            IncreaseTheRadius();
        }
        
    }

    void IncreaseTheRadius()
    {
        usageTimes--;

       circColl = GetComponent<CircleCollider2D>();
        if(circColl.radius <= radiusMax)
        {
           // Debug.Log("Rise() started");
            StartCoroutine(Rise());
        }
      
        
    }
    /// <summary>
    /// This IEnumerator increases the CircleColliders Radius till it reaches radiusMax; After that the Item Destroys itself
    /// </summary>
    /// <returns></returns>
    IEnumerator Rise()
    {
        while(circColl.radius <= radiusMax)
        {
        circColl.radius += riseSpeed * Time.deltaTime;
        Debug.Log(circColl.radius);

            if (circColl.radius >= radiusMax)
            {
                Debug.Log("Rise() stopped");
                StopCoroutine(Rise());
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(waitTime);

        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(FindObjectOfType<BaseEnemy>().gameObject);
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        
    }

}
