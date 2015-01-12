using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    private bool stuck;

	// Use this for initialization
	void Start () {
        stuck = false;
        Destroy(gameObject, 10.00f);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!stuck)
        {
            if (collision.gameObject.name != "Arrow(Clone)")/////////////////////////Sticky check
            {
                gameObject.transform.parent = collision.gameObject.transform;
                Destroy(rigidbody);
            }


             var damcom = collision.gameObject.GetComponent<Damageable>();
           
                //inflict damage with damageable
             if (damcom != null && damcom.damageRole == DamageRole.Dragon)////////////Damage check
             {
                 damcom.CurrentIntegrity -= 10;
             
             }
            
            stuck = true;
        }
    }
}
