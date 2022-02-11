using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGuard : MonoBehaviour
{

	public static event System.Action OnGuardHasSpottedPlayer;

	public float speed = 5;
	public float waitTime = .3f;
	public float turnSpeed = 90;
	public float timeToSpotPlayer = .5f;

	public Light spotlight;
	public float viewDistance;
	public LayerMask viewMask;

	float viewAngle;
	float playerVisibleTimer;

	Transform player;
	Color originalSpotlightColour;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		viewAngle = spotlight.spotAngle;
		originalSpotlightColour = spotlight.color;

		StartCoroutine(FollowPath());

	}

	void Update()
	{
		if (CanSeePlayer())
		{
			playerVisibleTimer += Time.deltaTime;
			transform.LookAt(player);
		}
		else
		{
			playerVisibleTimer -= Time.deltaTime;
		}
		playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
		spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);

		if (playerVisibleTimer >= timeToSpotPlayer)
		{
			if (OnGuardHasSpottedPlayer != null)
			{
				OnGuardHasSpottedPlayer();
			}
		}
	}

	bool CanSeePlayer()
	{
		if (Vector3.Distance(transform.position, player.position) < viewDistance)
		{
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
			if (angleBetweenGuardAndPlayer < viewAngle / 2f)
			{
				if (!Physics.Linecast(transform.position, player.position, viewMask))
				{
					return true;
				}
			}
		}
		return false;
	}

	IEnumerator FollowPath()
	{

		while (true)
		{
			transform.position = transform.forward*5*Time.deltaTime;
		}
	}

	IEnumerator TurnToFace(Vector3 lookTarget)
	{
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}

	void OnDrawGizmos()
	{

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
	}
	void OnTriggerEnter(Collider hitCollider)
	{
		if (hitCollider.tag == "Obstacle")
		{
			transform.Rotate(0, 90, 0);
		}

	}
}