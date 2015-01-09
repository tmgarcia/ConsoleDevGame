using UnityEngine;
using System.Collections;

public class VillagerMovement : MonoBehaviour {

    private PhotonView photonView;
    private float speed = 5;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
        {
            Vector3 forward = gameObject.transform.forward;
            Vector3 right = Vector3.Cross(forward,Vector3.up);
            Vector3 accelaration = new Vector3(0,0,0);
            if(Input.GetKey(KeyCode.W)) accelaration += forward;
            if(Input.GetKey(KeyCode.A)) accelaration += right;
            if(Input.GetKey(KeyCode.S)) accelaration -= forward;
            if(Input.GetKey(KeyCode.D)) accelaration -= right;
            accelaration.Normalize();
            gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime * speed;
        }
	}
}
