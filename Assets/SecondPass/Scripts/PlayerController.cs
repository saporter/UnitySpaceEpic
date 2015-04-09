using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public IShipSystems systems;	// another component or gameobject must register itself in Awake()
	public IMovable engines;		// another component or gameobject must register itself in Awake()
	public LayerMask floorMask;


	void Start(){
		if (systems == null) {
			Debug.Log("IShipSystems is null.  Adding a default component.");
			systems = gameObject.AddComponent<ShipSystems>();
		}
		if (engines == null) {
			Debug.Log("IMovable is null.  Adding a default component.");
			engines = gameObject.AddComponent<OrbitMover>();
		}
	}

	void Update()
	{
		if (Input.GetMouseButton (1)) 
		{
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			if (Physics.Raycast (camRay, out floorHit, 300f, floorMask)) 
			{
				Events.instance.Raise(new PlacingMovementMarkerEvent(floorHit.point));
			}
		}
		else if(Input.GetMouseButtonUp (1))
		{
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			
			if (Physics.Raycast (camRay, out floorHit, 300f, floorMask)) 
			{
				systems.Target = GameObject.FindWithTag("Enemy");
				Events.instance.Raise(new MovementMarkerPlacedEvent(floorHit.point));
				engines.Move(floorHit.point, systems);
				//Events.instance.Raise(new RightMouseUpOnBackgroundEvent(floorHit.point, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));
			}
		}
	}
}
