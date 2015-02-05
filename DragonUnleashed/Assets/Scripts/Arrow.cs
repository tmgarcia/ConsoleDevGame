using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{

    private bool stuck;

    // Use this for initialization
    void Start()
    {
        stuck = false;
        Destroy(gameObject, 10.00f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stuck)
        {
            if (rigidbody.velocity.normalized != Vector3.zero)
            {
                transform.forward = rigidbody.velocity.normalized;
            }
            else
            {
                transform.forward = Camera.main.transform.forward;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!stuck)
        {

            //var damcom = collision.gameObject.GetComponent<Damageable>();

            //if (collision.gameObject.name != "Arrow(Clone)" && collision.gameObject.name != "Villager(Clone)")////////////Sticky check
            //{
            //    stuck = true;
            //    gameObject.transform.parent = collision.gameObject.transform;
            //    Destroy(rigidbody);
            //    if (damcom != null && damcom.damageRole == DamageRole.Dragon)////////////Damage check //inflict damage with damageable
            //    {
            //        damcom.CurrentLocalIntegrity -= 10;

            //    }
            //}

            stuck = true;
            gameObject.transform.parent = collision.gameObject.transform;
            Destroy(rigidbody);
            if (collision.gameObject.layer == 9)
            {
                bool damageApplied = false;
                GameObject currentSegment = collision.gameObject;
                while (!damageApplied)
                {
                    if (currentSegment.GetComponent<Damageable>() != null)
                    {
                        currentSegment.GetComponent<Damageable>().CurrentLocalIntegrity -= 10;
                        damageApplied = true;
                    }
                    else
                    {
                        currentSegment = currentSegment.transform.parent.gameObject;
                    }
                }
            }


        }
    }
}
