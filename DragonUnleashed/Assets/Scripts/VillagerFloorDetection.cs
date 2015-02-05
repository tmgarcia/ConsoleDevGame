using UnityEngine;
using System.Collections;

public class VillagerFloorDetection : MonoBehaviour 
{
    private VillagerMovement movement;

	void Start () 
    {
        movement = transform.parent.GetComponent<VillagerMovement>();
        GameObject villager = transform.parent.gameObject;
        Physics.IgnoreCollision(collider, villager.collider);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            movement.isGrounded = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
        {
            movement.isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            movement.isGrounded = false;
        }
    }

}
