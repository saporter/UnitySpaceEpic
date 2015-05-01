using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleEnemyAI : MonoBehaviour { 
	/// <summary>
	/// Angle, in degrees, this AI will scan for ship modules
	/// </summary>
	public float ScanningScope = 45f;

	// Test variables
	private Vector3 debugHeading;

	private GameObject target;
	private int shipMask;

	// private NavMeshAgent navMeshAgent;
	public ShipSystems1 systems;

	public float FiringRange = 5f;	// How close I'd like to be to fire


	void Start () {
		shipMask = LayerMask.GetMask ("Ship");
		systems = GetComponent<ShipSystems1> ();
		if (systems == null) 
		{
			systems = gameObject.AddComponent<ShipSystems1>();
			Debug.Log("systems was null in enemeyship.  Added ShipSystems Component manually.");
		}


		Events.instance.AddListener<TransitionToEnemyTurnEvent> (StartTurn);
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<TransitionToEnemyTurnEvent>(StartTurn);
	}

	void Update()
	{
		// Testing scan...
		//Debug.DrawRay (transform.position, debugHeading, Color.blue);
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
		Debug.Log ("Finding ship modules");
		yield return null;
		List<ShipModule1> modules = ScanForModules ();
		foreach (ShipModule1 module in modules) {
			Debug.Log("I found: " + module.gameObject.name);
		}
		ShipModule1 targetModule = ChooseTargetModule (modules);
		Debug.Log("I'm targeting: " + targetModule);
		yield return StartCoroutine(FireWithClosestWeaponOn (targetModule));
	}

	IEnumerator Retreat()
	{
		Debug.Log ("------------ Retreating");
		if (systems.CurrentPowerLevel > 0 && target != null) 
		{
			Debug.Log ("Have " + systems.CurrentPowerLevel + " power units left. Moving as far away from target as possible");
			//Vector3 vEnd = transform.position + ((Quaternion.AngleAxis(Random.Range(90f, 270f), transform.up) * (transform.forward)) * systems.CurrentPowerLevel);
			Vector3 vEnd = transform.position + ((Quaternion.AngleAxis(Random.Range(90f, 270f), transform.up) * (target.transform.position - transform.position).normalized) * systems.CurrentPowerLevel);
			Vector3 newPos = Vector3.MoveTowards(transform.position, vEnd, systems.CurrentPowerLevel);

			yield return StartCoroutine(systems.MoveAndWait(newPos));

			Debug.Log ("Arrived at retreating destination");
		}
	}

	List<ShipModule1> ScanForModules()
	{
		List<ShipModule1> modules = new List<ShipModule1> ();
		Vector3 heading =  Quaternion.AngleAxis (-(ScanningScope / 2f), transform.up) * (target.transform.position - transform.position);
		for (int i = 0; i < (int) ScanningScope; ++i) {
			Ray ray = new Ray (transform.position, heading); 
			RaycastHit moduleSelected;
			if (Physics.Raycast (ray, out moduleSelected, 100f, shipMask)) {
				if(moduleSelected.collider.gameObject.GetComponent<ShipModule1>() && !modules.Contains(moduleSelected.collider.gameObject.GetComponent<ShipModule1>())){
					modules.Add(moduleSelected.collider.gameObject.GetComponent<ShipModule1>());
				}
			}
			heading =  Quaternion.AngleAxis (1f, transform.up) * heading;
		}
		return modules;
	}

	ShipModule1 ChooseTargetModule(List<ShipModule1> modules)
	{
		if (modules == null || modules.Count == 0)
			return null;
		ShipModule1 killThis = null;
		float highestScore = -1f;
		float heuristic;
		foreach (ShipModule1 module in modules) {
			heuristic = 0f;
			if(module.GetComponent<PhaserWeapon1>() != null)
				heuristic += 1f;
			if(module.GetComponent<EngineController>() != null)
				heuristic += 1f;
			if(module.GetComponent<PowerGenerator>() != null)
				heuristic += 1.2f;
			heuristic /= module.currentHealth;
			if(heuristic > highestScore){
				highestScore = heuristic;
				killThis = module;
			}
		}

		return killThis;
	}

	IEnumerator FireWithClosestWeaponOn (ShipModule1 targetModule)
	{
		PhaserWeapon1[] weapons = GetComponentsInChildren<PhaserWeapon1> ();
		if (weapons != null && weapons.Length > 0) {
			PhaserWeapon1 firingWith = null;
			float degreesToRotate = 180f;
			foreach (PhaserWeapon1 weapon in weapons){
				Vector3 heading = targetModule.transform.position - weapon.exitAperture.transform.position;
				float offset = weapon.InFieldOfFire(heading);
				Debug.Log("Degree offset for weapon " + weapon.name + " is " + offset);
				if(Mathf.Abs(offset) <= Mathf.Abs(degreesToRotate)){
					degreesToRotate = offset;
					firingWith = weapon;
				}
			}

			while(degreesToRotate != 0f){
				Debug.Log("Rotating " + degreesToRotate + " degrees for weapon " + firingWith.name);
				yield return StartCoroutine (systems.Rotate (degreesToRotate));
				degreesToRotate = firingWith.InFieldOfFire(targetModule.transform.position - firingWith.exitAperture.transform.position);
			}

			if(!firingWith.CanFire(targetModule.transform.position))
			{
				Debug.LogError("ERROR in EnemyAI.  Could not fire weapon after algorithm executed");
			}
			else{
				firingWith.Fire(targetModule.gameObject, targetModule.transform.position);
				yield return new WaitForSeconds(firingWith.effectDuration);
			}
		}
	}



	float MinPowerNeededToFire()
	{
		return 6f;
	}
}

public class EnemyFinishedTurnEvent : GameEvent
{
}
