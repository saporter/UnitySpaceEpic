using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

	private int floorMask;
	private float camRayLength;

	// Use this for initialization
	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		camRayLength = 300.0f;
	}

	void Update()
	{
		if(Input.GetMouseButtonUp (1))
		{
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) 
			{
				Events.instance.Raise(new RightMouseUpOnBackgroundEvent(floorHit.point, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));
			}
		}
	}

//	void OnMouseUp()
//	{
//		Events.instance.Raise (new BackgroundMouseUpEvent());
//	}
}

public class RightMouseUpOnBackgroundEvent : GameEvent
{
	public Vector3 point;
	public bool holdingShift;
	
	public RightMouseUpOnBackgroundEvent (Vector3 _point, bool _isHoldingShift)
	{
		point = _point;
		holdingShift = _isHoldingShift;
	}
}

//public class BackgroundMouseUpEvent: GameEvent
//{
//}


