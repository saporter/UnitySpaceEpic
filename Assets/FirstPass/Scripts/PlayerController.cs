using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	private float _engineCharge;
	private List<GameObject> flags;	// From System.Collections.Generic
	private GameController gameController;
	//private NavMeshAgent navMeshAgent;
	private bool currentlyMoving;
	private PhaserWeapon[] phasers;

	public float EngineCharge
	{
		get { return _engineCharge; }
	}

	public ShipSystems systems;
	public GameObject flagObject;

	void Start () {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController>();
		//navMeshAgent = GetComponent<NavMeshAgent> ();
		flags = new List<GameObject> ();
		currentlyMoving = false;

		systems = GetComponent<ShipSystems> ();
		if (systems == null) 
		{
			systems = gameObject.AddComponent<ShipSystems>();
			Debug.Log("systems was null on playership.  Added ShipSystems Component manually.");
		}

		Events.instance.AddListener<RightMouseUpOnBackgroundEvent> (OnRightMouseUpOnBackground);
		Events.instance.AddListener<CommitToMovementKeyPressEvent> (OnCommitToMovement);
		Events.instance.AddListener<ClearSelectionKeyPressEvent> (ClearSelection);
		Events.instance.AddListener<TransitionToEnemyTurnEvent> (ClearSelection);
	}

	void OnRightMouseUpOnBackground(RightMouseUpOnBackgroundEvent e) 
	{
		if(gameController.Phase == TurnPhase.PlayerTurn)
		{
			// Determine if I'm adding aditional waypoints or reseting and starting with the first
			if(!e.holdingShift)
			{
				ClearSelection (null);
			}

			// Determine if I calculate distance from the previous waypoint or the player
			Vector3 measureFrom;
			Vector3 newWaypoint = e.point;
			if(flags.Count > 0){
				measureFrom = flags[flags.Count - 1].transform.position;
			}else{
				measureFrom = transform.position;
			}

			// If I have enough power to move, add a waypoint as close as I'm able.
			float kineticPower = systems.KineticPower - _engineCharge;
			if(kineticPower > 0f){
				Vector3 actualNew = Vector3.MoveTowards(measureFrom, newWaypoint, kineticPower);
				AddWaypoint(actualNew);
				_engineCharge += Vector3.Distance(measureFrom, actualNew);;
			}

		}
	}

	void OnCommitToMovement(CommitToMovementKeyPressEvent e)
	{
		currentlyMoving = true;
		StartCoroutine (FollowPath());
	}

	void ClearSelection( GameEvent e)
	{
		if (!currentlyMoving && flags.Count > 0) {
			ClearWaypoints();
			_engineCharge = 0f;
		}
	}

	IEnumerator FollowPath()
	{
		_engineCharge = 0f;
		foreach (GameObject gameObject in flags) {
			yield return StartCoroutine(systems.MoveAndWait(gameObject.transform.position));
			Destroy(gameObject);
		}
		currentlyMoving = false;
		ClearWaypoints ();
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
}
