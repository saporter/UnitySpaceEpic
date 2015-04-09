using UnityEngine;
using System.Collections;

public class ShipSystems1 : MonoBehaviour {
	// Private variables
	private float _powerLevel;
	private float _engineUse;
	private NavMeshAgent navAgent;

	// Public Properties
	public float PowerCapacity 
	{
		get 
		{
			//if(this == null) return 0f;
			PowerGenerator[] generators = GetComponentsInChildren<PowerGenerator>();
			float totalPower = 0f;
			foreach(PowerGenerator g in generators)
			{
				totalPower += g.PowerCapacity;
			}
			return totalPower;
		}
	}

	public float CurrentPowerLevel
	{
		get { return _powerLevel; }
		// set { _powerLevel = value; }
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

	public float RotationSpeed
	{
		get 
		{
			EngineController[] engines = GetComponentsInChildren<EngineController>();
			float totalSpeed = 0f;
			foreach(EngineController e in engines)
			{
				totalSpeed += e.RotationSpeed;
			}
			return totalSpeed;
		}
	}

	public float CurrentEngineUse
	{
		get { return _engineUse; }
		// set { _engineUse = value; }
	}

	// How much enegery the ship can devote to movement
	public float KineticPower
	{
		get { return Mathf.Min(CurrentPowerLevel, EngineCapacity - CurrentEngineUse); }
	}

	void Start()
	{
		_powerLevel = PowerCapacity;
		_engineUse = 0f;
		navAgent = GetComponent<NavMeshAgent> ();
		if (navAgent == null)
			throw new UnityException ("navAgent can not be null in ShipSystems.  Must attached NavMeshAgent to GameObject.");

		Events.instance.AddListener<TransitionToPlayerTurnEvent> (NewTurn);
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<TransitionToPlayerTurnEvent> (NewTurn);
	}

	void NewTurn(TransitionToPlayerTurnEvent e)
	{
		_powerLevel = PowerCapacity;
		_engineUse = 0f;
	}

	/// <summary>
	/// Moves to point.
	/// </summary>
	/// <param name="point">Point.</param>
	void MoveToPoint(Vector3 point)
	{
		float distance = Vector3.Distance (transform.position, point);
		Vector3 newPos = point;
		
		if (distance > _powerLevel) {
			newPos = Vector3.MoveTowards (transform.position, point, _powerLevel);
			_powerLevel = 0f;
			_engineUse += _powerLevel;
		} 
		else {
			_powerLevel -= distance;
			_engineUse += distance;
		}
		
		navAgent.SetDestination (newPos);
	}

	/// <summary>
	/// Move the specified point.
	/// </summary>
	/// <param name="point">Point.</param>
	public void Move(Vector3 point)
	{
		MoveToPoint (point);
	}

	/// <summary>
	/// Move to the specified point and wait.
	/// </summary>
	/// <returns>IEnumerator for use in Coroutine.</returns>
	/// <param name="point">Point.</param>
	public IEnumerator MoveAndWait(Vector3 point)
	{
		MoveToPoint (point);
		yield return StartCoroutine(WaitForDestination());
	}

	/// <summary>
	/// Finishes movement to destination before return from started Coroutine.
	/// </summary>
	/// <returns>IEnumerator for use in Coroutine.</returns>
	public IEnumerator WaitForDestination()
	{
		yield return new WaitForSeconds (.5f);		// Seriously has to be a better way than this...
		while (navAgent.pathPending) 
		{
			yield return null;
		}
		
		while (!(navAgent.pathStatus == NavMeshPathStatus.PathComplete && navAgent.remainingDistance <= float.Epsilon) )
		{
			yield return new WaitForSeconds(.2f);
		}
	}

	public IEnumerator Rotate (float degrees)
	{
		if (_powerLevel > degrees * EngineController.RotationCost) {
			//Quaternion newRotation = Quaternion.AngleAxis (degrees, transform.up);
			float speed = RotationSpeed;
			float direction = degrees > 0 ? 1f : -1f;
			degrees *= direction;
			while (degrees > 0) {
				float step = speed * Time.deltaTime * direction;
				transform.Rotate(new Vector3(0f, step, 0f));
				degrees -= step * direction;
				yield return null;
			}

			_powerLevel -= degrees * EngineController.RotationCost;
		}
	}

	public void Fire(GameObject target, Vector3 hit, PhaserWeapon weapon)
	{
		if (weapon.Owner != this.gameObject) 
		{
			Debug.Log("ShipSystems could not fire.  Weapon does not belong to this ShipSystem.");
			return;
		}
		if (!weapon.CanFire (hit)) 
		{
			Debug.Log("Weapon could not fire.  CanFire() returned false.");
			return;
		}
		if (weapon.powerCost > _powerLevel) 
		{
			Debug.Log("ShipSystems could not fire.  Not enough power.");
			return;
		}

		_powerLevel -= weapon.powerCost;
		weapon.Fire (target, hit);
	}

}
