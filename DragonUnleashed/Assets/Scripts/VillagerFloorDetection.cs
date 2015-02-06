using UnityEngine;
using System.Collections;

public class VillagerFloorDetection : MonoBehaviour 
{
    public bool isGrounded { get; set; }
    private const float Y_POSITION = 0.4f;

	void Start () 
    {
        isGrounded = true;
	}

    void Update()
    {
        Vector3[] rayStartPoints = new Vector3[5];
        MeshRenderer render = GetComponent<MeshRenderer>();
        Vector3 temp = render.bounds.size;
        Vector3 bounds = new Vector3(temp.x / 2.0f, temp.y / 2.0f, temp.z / 2.0f);

        rayStartPoints[0] = transform.position + new Vector3(bounds.x, Y_POSITION, bounds.z); //Forward-right
        rayStartPoints[1] = transform.position + new Vector3(-bounds.x, Y_POSITION, bounds.z); //Forward-left
        rayStartPoints[2] = transform.position + new Vector3(bounds.x, Y_POSITION, -bounds.z); //Back-right
        rayStartPoints[3] = transform.position + new Vector3(-bounds.x, Y_POSITION, -bounds.z); //Back-left
        rayStartPoints[4] = transform.position + new Vector3(0.0f, Y_POSITION, 0.0f);

        Debug.DrawRay(rayStartPoints[0], -Vector3.up * temp.y, Color.red);
        Debug.DrawRay(rayStartPoints[1], -Vector3.up * temp.y, Color.black);
        Debug.DrawRay(rayStartPoints[2], -Vector3.up * temp.y, Color.blue);
        Debug.DrawRay(rayStartPoints[3], -Vector3.up * temp.y, Color.green);
        Debug.DrawRay(rayStartPoints[4], -Vector3.up * temp.y, Color.yellow);
    }

    public void CheckIfGrounded()
    {
        Vector3[] rayStartPoints = new Vector3[5];
        MeshRenderer render = GetComponent<MeshRenderer>();
        Vector3 temp = render.bounds.size;
        Vector3 bounds = new Vector3(temp.x/2.0f, temp.y/2.0f, temp.z/2.0f);

        rayStartPoints[0] = transform.position + new Vector3(bounds.x, Y_POSITION, bounds.z); //Forward-right
        rayStartPoints[1] = transform.position + new Vector3(-bounds.x, Y_POSITION, bounds.z); //Forward-left
        rayStartPoints[2] = transform.position + new Vector3(bounds.x, Y_POSITION, -bounds.z); //Back-right
        rayStartPoints[3] = transform.position + new Vector3(-bounds.x, Y_POSITION, -bounds.z); //Back-left
        rayStartPoints[4] = transform.position + new Vector3(0.0f, Y_POSITION, 0.0f);

        isGrounded = false;
        int layermask = 1 << 8;
        layermask = ~layermask;
        Collider playerCollider = collider;

        for (int i = 0; i < rayStartPoints.Length && !isGrounded; i++)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(rayStartPoints[i], -transform.up, out hit, temp.y, layermask))
            {
                if (hit.collider != playerCollider)
                {
                    isGrounded = true;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CheckIfGrounded();
    }

}
