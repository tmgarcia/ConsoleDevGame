using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterMovement : MonoBehaviour
{
	//SpawnerObject
	public GameObject anchor;
	public Vector3 anchorPosition;
	//public AudioSource footstep;
	//public AudioSource screaming;

	GameObject player;
	NavMeshAgent monsterMesh;
	public float minMoveDisance = 1.0f;
	public float maxMoveDistance = 5.0f;
	public float maxAnchorDistance = 5.0f;
	public float NormalSpeed = 3.5f;
	public float BurningSpeed = 100.0f;
	bool inRange = false;

	//for dragon
	private static Vector2 herdDirection;
	private static float herdX;
	private static float herdY;
	private static bool isHerdSelectingDirection;

	void Start()
	{
		monsterMesh = GetComponent<NavMeshAgent>();
		if (anchor == null)
		{
			anchorPosition = transform.position;
		}
		else
		{
			anchorPosition = anchor.transform.position;
		}
	}

	void Update()
	{
		//if (inRange)
		//{
		//	SeekPlayer();
		//}
		//else
		//{

		if (GetComponent<Flammable>().BurninationLevel <= GetComponent<Flammable>().ignitionThreshold)
		{
			GetComponent<NavMeshAgent>().speed = NormalSpeed;
			//if (screaming.isPlaying)
			//{
			//	screaming.Stop();
			//}
		}
		else
		{
			GetComponent<NavMeshAgent>().speed = BurningSpeed;
			//if (!screaming.isPlaying)
			//{
			//	screaming.Play();
			//}
		}

		if (monsterMesh.enabled && monsterMesh.remainingDistance <= float.Epsilon)
		{
			//if(footstep != null && footstep.isPlaying)
			//{
			//	footstep.Stop();
			//}
			PickTarget();
		}
		else
		{
		//	if(footstep != null && !footstep.isPlaying)
		//	{
		//		footstep.Play();
		//		footstep.pitch = monsterMesh.speed / 2;
		//	}
		}
	}

	void PickTarget()
	{
		Vector2 direction = Random.insideUnitCircle;

		Vector3 potentialTarget = transform.position + new Vector3(direction.x * Random.Range(minMoveDisance, maxMoveDistance), transform.position.y, direction.y * Random.Range(minMoveDisance, maxMoveDistance));

//		print(potentialTarget + " D " + anchorPosition + " = " + Vector3.Distance(potentialTarget, anchorPosition) + " > " + maxAnchorDistance);
		if (Vector3.Distance(potentialTarget, anchorPosition) > maxAnchorDistance)
		{

			//Debug.LogException(new UnityException("InvalidTarget"));
			//print("Failed to pick valid target");
		}
		else
		{
			monsterMesh.SetDestination(new Vector3(potentialTarget.x, transform.position.y, potentialTarget.z));
		}
	}

	//void SeekPlayer()
	//{
	//	if (Vector3.Distance(player.transform.position, anchor.transform.position) <= maxAnchorDistance)
	//	{
	//		monsterMesh.SetDestination(player.transform.position);
	//	}
	//}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject != null && c.gameObject == player)
		{
			inRange = true;
		}
	}

	void OnTriggerExit(Collider c)
	{
		if (c.gameObject != null && c.gameObject == player)
		{
			inRange = false;
		}
	}

	void OnDestroy()
	{
		if (anchor != null && anchor.GetComponent<Spawner>() != null && transform.parent != null)
		{
			anchor.GetComponent<Spawner>().currentSpawned--;
			Destroy(transform.parent.gameObject);
		}
	}
}