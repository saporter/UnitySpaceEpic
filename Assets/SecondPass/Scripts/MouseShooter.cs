using UnityEngine;
using System.Collections;

public class MouseShooter : MonoBehaviour {
	public LayerMask floorMask;

	private IWeapon primary;
	private IWeapon secondary;
	private Coroutine p_firing;	// used to stop coroutines
	private Coroutine s_firing; // used to stop coroutines
	
	// Use this for initialization
	void Awake () {
		/* Change how this works... */
		IWeapon[] weapons = GetComponentsInChildren<IWeapon>();
		if (weapons != null && weapons.Length > 0) {
			primary = weapons[0];
			secondary = weapons.Length > 1 ? weapons[1] : null;
		}
		/* End Change*/
	}
	
	// Update is called once per frame
	void Update () {
		// Primary weapon
		if (Input.GetMouseButtonDown (0) && primary != null) {
			Vector3? target = MousePointOnFloor();
			if (target.HasValue) 
			{
				if(p_firing != null) StopCoroutine(p_firing);
				primary.FireButtonDown(target.Value, null);
				p_firing = StartCoroutine(Firing(primary));
			}
		}else if(Input.GetMouseButtonUp(0) && primary != null){
			if(p_firing != null) StopCoroutine(p_firing);
			primary.FireButtonUp();
		}

		// Secondary weapon
		if (Input.GetMouseButtonDown (1) && secondary != null) {
			Vector3? target = MousePointOnFloor();
			if (target.HasValue) 
			{
				if(s_firing != null) StopCoroutine(s_firing);
				secondary.FireButtonDown(target.Value, null);
				s_firing = StartCoroutine(Firing(secondary));
			}
		}else if(Input.GetMouseButtonUp(1) && secondary != null){
			if(s_firing != null) StopCoroutine(s_firing);
			secondary.FireButtonUp();
		}
	}

	IEnumerator Firing(IWeapon weapon)
	{
		Debug.Log ("Consider a backup shutoff for Firing Coroutine in MouseShooter");
		while (true) {
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
