using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterMovement : MonoBehaviour
{
	public GameObject anchor;
	public Vector3 anchorPosition;
	public AudioSource footstep;
	public AudioSource screaming;

	NavMeshAgent monsterMesh;
	public float minMoveDisance = 1.0f;
	public float maxMoveDistance = 5.0f;
	public float maxAnchorDistance = 5.0f;
	public float NormalSpeed = 3.5f;
	public float BurningSpeed = 100.0f;

	private Vector3 lastPosition = new Vector3();
	private Timer t;

	void Start()
	{
		monsterMesh = GetComponent<NavMeshAgent>();
		t = GetComponent<Timer>();
		if (anchor == null)
		{
			anchorPosition = transform.position;
		}
		else
		{
			anchorPosition = anchor.transform.position;
		}

		t.TimerFinishEvent += Explode;
	}

	void Update()
	{
		if (lastPosition != transform.position)
		{
			if (t.isRunning)
			{
				t.StopTimer();
			}
		}
		else
		{
			if (!t.isRunning)
			{
				t.StartTimer();
			}
		}

		lastPosition = transform.position;

		if (GetComponent<Flammable>().BurninationLevel <= GetComponent<Flammable>().ignitionThreshold)
		{
			GetComponent<NavMeshAgent>().speed = NormalSpeed;
			if (screaming != null && screaming.isPlaying)
			{
				screaming.Stop();
			}
		}
		else
		{
			GetComponent<NavMeshAgent>().speed = BurningSpeed;
			if (screaming != null && !screaming.isPlaying)
			{
				screaming.Play();
			}
		}

		if (monsterMesh.enabled && monsterMesh.remainingDistance <= float.Epsilon)
		{
			if (footstep != null && footstep.isPlaying)
			{
				footstep.Stop();
			}
			PickTarget();
		}
		else
		{
			if (footstep != null && !footstep.isPlaying)
			{
				footstep.Play();
			}
		}
	}

	public void Explode()
	{
		GetComponent<Flammable>().BurninationLevel = 100000000;
		t.TimerFinishEvent -= Explode;
	}

	void PickTarget()
	{
		Vector2 direction = Random.insideUnitCircle;

		Vector3 potentialTarget = transform.position + new Vector3(direction.x * Random.Range(minMoveDisance, maxMoveDistance), transform.position.y, direction.y * Random.Range(minMoveDisance, maxMoveDistance));

		if (Vector3.Distance(potentialTarget, anchorPosition) <= maxAnchorDistance)
		{
			monsterMesh.SetDestination(new Vector3(potentialTarget.x, transform.position.y, potentialTarget.z));
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