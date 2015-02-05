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

    void LateUpdate()
    {
        MeshFilter filter = transform.parent.GetComponent<MeshFilter>();
        string meshName = filter.mesh.ToString();
        MeshRenderer parentRenderer = transform.parent.GetComponent<MeshRenderer>();
        if (meshName.Contains("default"))
        {
            float halfHeight = parentRenderer.bounds.size.y / 2.0f;
            transform.localPosition = new Vector3(transform.localPosition.x, -(halfHeight + 0.05f), transform.localPosition.z);
        }
        else
        {
            float distance = parentRenderer.bounds.size.y / 5.0f;
            transform.localPosition = new Vector3(transform.localPosition.x, -(distance - 0.1f), transform.localPosition.z);
        }
    }

    void OnTriggerEnter(Collider other)
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
