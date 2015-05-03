using UnityEngine;
using System.Collections;

public class OrbitMover : MonoBehaviour {
	private bool stillRotating = false;

	public float speed = 6f;
	public float rotationSpeed = 180f;
	public IShipSystems systems;			
	public LayerMask floorMask;

	void Awake()
	{
		systems = GetComponent<IShipSystems> ();
		GetComponent<Rigidbody> ().isKinematic = true;
	}

	public void Move(Vector3 toPoint)
	{
		Vector3 around = systems.Target == null ? transform.position : systems.Target.transform.position;
		toPoint.Set (toPoint.x, transform.position.y, toPoint.z);

		StartCoroutine(Orbit(around, toPoint));
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
				Move(floorHit.point);
			}
		}
	}

	IEnumerator Orbit(Vector3 around, Vector3 toPoint)
	{


		// Move radius away from "around" point
		float radius = Vector3.Distance (around, toPoint);
		Vector3 startDir = transform.position - around;
		Vector3 start = (new Ray(around, startDir)).GetPoint (radius);
		// Rotate before moving
		StartCoroutine (RotateToward (start - transform.position));
		while (Vector3.Distance(transform.position, start) > Mathf.Epsilon || stillRotating) {
			transform.position = Vector3.MoveTowards (transform.position, start, speed * Time.deltaTime);
			yield return new WaitForFixedUpdate ();
		}

		// Determine which way around is shortest
		Vector3 newDir = toPoint - around;
		float angle = Vector3.Angle (startDir, newDir);
		float rotDir = Vector3.Dot (Vector3.Cross (startDir, newDir), transform.up);
		float direction = rotDir < 0 ? -1f : 1f;
		// And rotate in that direction
		StartCoroutine (RotateToward (Vector3.Cross(transform.up, startDir) * direction)); 

		// start rotation
		float rot = 0f;
		float degrees = 0f;
		float angularSpeed = speed / (radius);
		while (rot < angle) {
			degrees = angularSpeed * direction;
			transform.RotateAround (around, transform.up, degrees);
			rot += Mathf.Abs(degrees);
			yield return new WaitForFixedUpdate();
		}

		while (stillRotating)
			yield return new WaitForFixedUpdate();
	}

	IEnumerator RotateToward(Vector3 newDirection)
	{
		stillRotating = true;
		float degrees = Vector3.Angle (transform.forward, newDirection); 
		float direction = Vector3.Dot (Vector3.Cross (transform.forward, newDirection), transform.up) < 0 ? -1f : 1f;
		float step = 0f;
		while (degrees > 0) {
			step = rotationSpeed * Time.deltaTime * direction;
			transform.Rotate(new Vector3(0f, step, 0f));
			degrees -= step * direction;
			yield return new WaitForFixedUpdate();
		}
		stillRotating = false;
	}
}
