using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	private float _powerLevel;
	private float _engineUse;
	private float engineCharge;
	private List<GameObject> flags;	// From System.Collections.Generic
	private GameController gameController;
	private NavMeshAgent navMeshAgent;
	private bool currentlyMoving;
	private PhaserWeapon[] phasers;
	private ShipSystems systems;

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

	public float PowerLevel
	{
		get { return _powerLevel;}
	}

	public float EngineUse		// Power transferred to movement
	{
		get { return _engineUse; }
	}

	public GameObject flagObject;

	void Start () {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController>();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		flags = new List<GameObject> ();
		_powerLevel = PowerCapacity;
		currentlyMoving = false;

		systems = GetComponent<ShipSystems> ();
		if (systems == null) 
		{
			systems = gameObject.AddComponent<ShipSystems>();
			Debug.Log("systems was null in enemeyship.  Added ShipSystems Component manually.");
		}

		Events.instance.AddListener<RightMouseUpOnBackgroundEvent> (OnRightMouseUpOnBackground);
		Events.instance.AddListener<CommitToMovementKeyPressEvent> (OnCommitToMovement);
		Events.instance.AddListener<TransitionToPlayerTurnEvent> (OnNewTurn);
		Events.instance.AddListener<ClearSelectionKeyPressEvent> (ClearSelection);
	}

	void OnRightMouseUpOnBackground(RightMouseUpOnBackgroundEvent e) 
	{
		if(gameController.Phase == TurnPhase.PlayerTurn)
		{
			if(!e.holdingShift)
			{
				ClearSelection (null);
			}

			// Calculate distance between the previous waypoint (or player) and the new point
			Vector3 measureFrom;
			Vector3 newWaypoint = e.point;
			float distance;
			if(flags.Count > 0){
				measureFrom = flags[flags.Count - 1].transform.position;
			}else{
				measureFrom = transform.position;
			}
			distance = Vector3.Distance(measureFrom, newWaypoint);

			// If we have enough enginePower, add the next point
			float kineticPower = _powerLevel <= EngineCapacity - _engineUse ? _powerLevel : EngineCapacity - _engineUse;
			if(kineticPower > 0f){
				Vector3 actualNew = Vector3.MoveTowards(measureFrom, newWaypoint, kineticPower);
				AddWaypoint(actualNew);
				distance = Vector3.Distance(measureFrom, actualNew);
				_powerLevel -= distance;
				engineCharge += distance;
				_engineUse += distance;
			}

		}
	}

	void OnCommitToMovement(CommitToMovementKeyPressEvent e)
	{
		currentlyMoving = true;
		StartCoroutine (FollowPath());
	}

	void OnNewTurn(TransitionToPlayerTurnEvent e)
	{
		_powerLevel = PowerCapacity;
		_engineUse = 0f;
	}

	void ClearSelection( ClearSelectionKeyPressEvent e)
	{
		if (!currentlyMoving && flags.Count > 0) {
			ClearWaypoints();
			_powerLevel += engineCharge;
			_engineUse -= engineCharge;
			engineCharge = 0f;
		}
	}

	IEnumerator FollowPath()
	{

		foreach (GameObject gameObject in flags) {
			Move(gameObject.transform.position); 

			yield return new WaitForFixedUpdate();
			yield return new WaitForFixedUpdate();	// There has to be a better way to determine when I've reached my position

			while (!(navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance <= float.Epsilon) )
			{
				yield return null;
			}
			// Debug.Log ("Path status: " + navMeshAgent.pathStatus + " remainingDistance: " + navMeshAgent.remainingDistance);
			Destroy(gameObject);
		}
		currentlyMoving = false;
		engineCharge = 0f;
		ClearWaypoints ();
	}

	void Move(Vector3 position)
	{
		navMeshAgent.SetDestination (position);
	}

	void ClearWaypoints()
	{
		foreach (GameObject gameObject in flags) {
			if(gameObject != null){
				Destroy(gameObject);
			}
		}
		flags.Clear ();
	}

	void AddWaypoint(Vector3 point)
	{
		GameObject flag = Instantiate(flagObject, point, new Quaternion()) as GameObject;
		flags.Add (flag);
	}

	public void FireWeapon(PhaserWeapon p, GameObject target, Vector3 hit)
	{
		if (_powerLevel >= p.powerCost) {
			_powerLevel -= p.powerCost;
			p.Fire (target, hit);
		}
	}

}
