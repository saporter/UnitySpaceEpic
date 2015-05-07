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

	public LayerMask floorMask;


	private Coroutine p_firing;	// used to stop coroutines
	private Coroutine s_firing; // used to stop coroutines

	
	// Update is called once per frame
	void Update () {
		// Primary weapon
		if (Input.GetMouseButtonDown (0) && _primary != null) {
			Vector3? target = MousePointOnFloor();
			if (target.HasValue) 
			{
				if(p_firing != null) StopCoroutine(p_firing);
				_primary.FireButtonDown(target.Value, null);
				p_firing = StartCoroutine(Firing(_primary));
			}
		}else if(Input.GetMouseButtonUp(0) && _primary != null){
			if(p_firing != null) StopCoroutine(p_firing);
			_primary.FireButtonUp();
		}

		// Secondary weapon
		if (Input.GetMouseButtonDown (1) && _secondary != null) {
			Vector3? target = MousePointOnFloor();
			if (target.HasValue) 
			{
				if(s_firing != null) StopCoroutine(s_firing);
				_secondary.FireButtonDown(target.Value, null);
				s_firing = StartCoroutine(Firing(_secondary));
			}
		}else if(Input.GetMouseButtonUp(1) && _secondary != null){
			if(s_firing != null) StopCoroutine(s_firing);
			_secondary.FireButtonUp();
		}
	}

	IEnumerator Firing(IWeapon weapon)
	{
		while (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
			Vector3? target = MousePointOnFloor();
			if(target.HasValue)
				weapon.UpdateWeapon(target.Value, null);

			yield return new WaitForFixedUpdate();
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
