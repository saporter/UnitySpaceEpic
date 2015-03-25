using UnityEngine;
using System.Collections;

public class ShipSystems : MonoBehaviour {
	// Private variables
	private float _powerLevel;
	private float _engineUse;
	private NavMeshAgent navAgent;

	// Public Properties
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

	public float CurrentEngineUse
	{
		get { return _engineUse; }
		// set { _engineUse = value; }
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

	void NewTurn(TransitionToPlayerTurnEvent e)
	{
		_powerLevel = PowerCapacity;
		_engineUse = 0f;
	}

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

	public void Move(Vector3 point)
	{
		MoveToPoint (point);
	}

	public IEnumerator MoveAndWait(Vector3 point)
	{
		MoveToPoint (point);
		yield return StartCoroutine(WaitForDestination());
	}

	public IEnumerator WaitForDestination()
	{
		yield return new WaitForSeconds (.5f);		// Seriously has to be a better way than this...
		while (navAgent.pathPending) 
		{
			yield return null;
		}
		
		while (!(navAgent.pathStatus == NavMeshPathStatus.PathComplete && navAgent.remainingDistance <= float.Epsilon) )
		{
			Debug.Log ("*Moving to destination");
			yield return new WaitForSeconds(.2f);
		}
	}

}
