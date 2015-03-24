using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour {
	private GameObject target;
	private NavMeshAgent navMeshAgent;
	private float _powerLevel;
	private float _engineUse;

	public float PowerCapacity 
	{
		get 
		{
			PowerGenerator[] generators = GetComponentsInChildren<PowerGenerator>();
			float totalPower = 0f;
			foreach(PowerGenerator g in generators)
			{
				totalPower += g.PowerCapacity;
			}
			return totalPower;
		}
	}
	
	public float EngineCapacity
	{
		get 
		{
			EngineController[] engines = GetComponentsInChildren<EngineController>();
			float totalEngine = 0f;
			foreach(EngineController e in engines)
			{
				totalEngine += e.EngineCapacity;
			}
			return totalEngine;
		}
	}
	public float FiringRange = 5f;	// How close I'd like to be to fire


	void Start () {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		_powerLevel = PowerCapacity;

		Events.instance.AddListener<TransitionToEnemyTurnEvent> (StartTurn);
	}

	void StartTurn(TransitionToEnemyTurnEvent e)
	{
		StartCoroutine (EnemyTurn ());

	}

	IEnumerator EnemyTurn()
	{
		_powerLevel = PowerCapacity;
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
			if(_powerLevel - travel >= MinPowerNeededToFire()){
				Debug.Log ("Need to be " + FiringRange + " units from target. Moving " + travel + " units closer");
				_powerLevel -= travel;
				Vector3 newPos = Vector3.MoveTowards(transform.position, target.transform.position, travel);

				navMeshAgent.SetDestination(newPos); 
				yield return StartCoroutine(WaitForDestination());

				Debug.Log ("Arrived at firing range destination");
			}
		}
		//yield return null;
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
		if (_powerLevel > 0) 
		{
			Debug.Log ("Have " + _powerLevel + " power units left. Moving as far away from target as possible");
			Vector3 vEnd = transform.position + ((Quaternion.AngleAxis(Random.Range(90f, 270f), transform.up) * transform.forward) * _powerLevel);
			Vector3 newPos = Vector3.MoveTowards(transform.position, vEnd, _powerLevel);

			navMeshAgent.SetDestination(newPos); 
			yield return StartCoroutine(WaitForDestination());

			Debug.Log ("Arrived at retreating destination");
		}
	}

	IEnumerator WaitForDestination()
	{
		yield return new WaitForSeconds (.5f);		// Seriously has to be a better way than this...
		while (navMeshAgent.pathPending) 
		{
			yield return null;
		}

		while (!(navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance <= float.Epsilon) )
		{
			Debug.Log ("Moving to destination");
			yield return new WaitForSeconds(.2f);
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
