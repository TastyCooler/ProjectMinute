using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_instakill : BaseItem {

    //[SerializeField]GameObject medal;
    BoxCollider2D boxColl;
    CircleCollider2D circColl;
    [SerializeField] float riseSpeed;
    [SerializeField] float radiusMax;

    [SerializeField] ParticleSystem shine;
    ParticleSystem.EmissionModule shineEmission;

    protected override void RunFunctionalityOfItem()
    {
        shine.transform.position = player.transform.position;
        shineEmission = shine.emission;
        shine.Play();
        //shine.Stop();
        IncreaseTheRadius();
    }

    void IncreaseTheRadius()
    {
        circColl = GetComponent<CircleCollider2D>();
        circColl.enabled = true;
        if (circColl.radius <= radiusMax)
        {
            StartCoroutine(Rise());
        }
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

            if (circColl.radius >= radiusMax)
            {
                player.PlayerState = PlayerController.State.freeToMove;
                StopCoroutine(Rise());
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(cooldown);
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
