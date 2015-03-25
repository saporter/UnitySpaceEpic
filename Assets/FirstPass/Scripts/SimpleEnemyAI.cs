using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour {
	private GameObject target;
	// private NavMeshAgent navMeshAgent;
	private ShipSystems systems;

	public float FiringRange = 5f;	// How close I'd like to be to fire


	void Start () {
		systems = GetComponent<ShipSystems> ();
		if (systems == null) 
		{
			systems = gameObject.AddComponent<ShipSystems>();
			Debug.Log("systems was null in enemeyship.  Added ShipSystems Component manually.");
		}


		Events.instance.AddListener<TransitionToEnemyTurnEvent> (StartTurn);
	}

	void StartTurn(TransitionToEnemyTurnEvent e)
	{
		StartCoroutine (EnemyTurn ());

	}

	IEnumerator EnemyTurn()
	{
		AquireTarget ();
		yield return StartCoroutine (MoveToFiringRange ());
		yield return StartCoroutine (FireWeapons ());
		yield return StartCoroutine (Retreat ());
		Events.instance.Raise (new EnemyFinishedTurnEvent ());
	}

	void AquireTarget()
	{
		Debug.Log ("------------ Aquiring Target");
		target = GameObject.FindWithTag ("Player");
		Debug.Log ("Found: " + target);
	}

	IEnumerator MoveToFiringRange()
	{
		Debug.Log ("------------ Moving to firing range");
		float distanceToTarget = Vector3.Distance (transform.position, target.transform.position);
		Debug.Log ("target is " + distanceToTarget + " units away from me");
		if (distanceToTarget > FiringRange) 
		{
			float travel = distanceToTarget - FiringRange;
			if(systems.CurrentPowerLevel - travel >= MinPowerNeededToFire()){
				Debug.Log ("Need to be " + FiringRange + " units from target. Trying to move " + travel + " units closer");
				Vector3 newPos = Vector3.MoveTowards(transform.position, target.transform.position, travel);

				yield return StartCoroutine(systems.MoveAndWait(newPos));

				Debug.Log ("Arrived at firing range destination");
			}
		}
	}

	IEnumerator FireWeapons()
	{
		Debug.Log ("------------ Firing weapons");
		Debug.Log ("Doing nothing & Pausing for 1.5 seconds");
		yield return new WaitForSeconds (1.5f);
	}

	IEnumerator Retreat()
	{
		Debug.Log ("------------ Retreating");
		if (systems.CurrentPowerLevel > 0) 
		{
			Debug.Log ("Have " + systems.CurrentPowerLevel + " power units left. Moving as far away from target as possible");
			Vector3 vEnd = transform.position + ((Quaternion.AngleAxis(Random.Range(90f, 270f), transform.up) * transform.forward) * systems.CurrentPowerLevel);
			Vector3 newPos = Vector3.MoveTowards(transform.position, vEnd, systems.CurrentPowerLevel);

			yield return StartCoroutine(systems.MoveAndWait(newPos));

			Debug.Log ("Arrived at retreating destination");
		}
	}

	float MinPowerNeededToFire()
	{
		return 0f;
	}
}

public class EnemyFinishedTurnEvent : GameEvent
{
}
