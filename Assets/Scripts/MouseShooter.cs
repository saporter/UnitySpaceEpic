using UnityEngine;
using System.Collections;

public class MouseShooter : MonoBehaviour, IShooter {
	#region IShooter implementation
	private IWeapon _primary;
	private IWeapon _secondary;

	public IWeapon Primary {
		get { return _primary; }
		set { _primary = value; }
	}

	public IWeapon Secondary {
		get { return _secondary; }
		set { _secondary = value; }
	}

	#endregion

	[SerializeField] LayerMask floorMask;

	private bool primaryUp = false;		// when primary button is released
	private bool secondaryUp = false;	// when secondary button is released

	void Awake()
	{
		if(floorMask.value == 0)
			floorMask  = LayerMask.GetMask("Floor");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Primary weapon
		if (Input.GetAxisRaw("Fire1") > 0f && _primary != null) {
			Vector3? target = MousePointOnFloor();
			if (target.HasValue) 
			{
				primaryUp = true;
				_primary.FireButtonDown(target.Value);
			}
		}else if(primaryUp && _primary != null){
			primaryUp = false;
			_primary.FireButtonUp();
		}

		// Secondary weapon
		if (Input.GetAxisRaw("Fire2") > 0f && _secondary != null) {
			Vector3? target = MousePointOnFloor();
			if (target.HasValue) 
			{
				secondaryUp = true;
				_secondary.FireButtonDown(target.Value);
			}
		}else if(secondaryUp && _secondary != null){
			secondaryUp = false;
			_secondary.FireButtonUp();
		}
	}

	// ? makes it a nullable type. Will return y = 0.0f
	private Vector3? MousePointOnFloor()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast (camRay, out floorHit, 300f, floorMask)) 
		{
			return new Vector3?(new Vector3(floorHit.point.x, 0f, floorHit.point.z));
		}
		return null;
	}
}
