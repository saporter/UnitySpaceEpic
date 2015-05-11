using UnityEngine;
using System.Collections;
using System.Linq;

public class LaserWeapon : MonoBehaviour, IWeapon {

	public float maxDistance = 10f;
	public float maxCharge = 1.5f;
	public float damagePerSecond = 7f;
	public LineRenderer beamEffect;
	public GameObject damageEffect;

	//private Collider coll;
	private Transform aperture;
	private LayerMask shipMask;

	void Awake () {
		beamEffect = GetComponent<LineRenderer> ();
		if (beamEffect == null)
			Debug.LogError ("Attach LineRenderer Component to LaserWeapon.  beamEffect is still null");
		if(damageEffect == null)
			Debug.Log ("No damage effect added for " + name + " under parent " + transform.parent != null ? transform.parent.name : "<none>");
		beamEffect.enabled = false;

		//coll = transform.parent.gameObject.GetComponent<Collider> ();
		aperture = transform;
		damageEffect = Instantiate (damageEffect, aperture.position, aperture.rotation) as GameObject;
		damageEffect.transform.parent = this.transform;
		damageEffect.SetActive (false);
		shipMask = 1 << LayerMask.NameToLayer ("Ship");
	}

	void Start()
	{

	}

	#region IWeapon implementation
	public string Name { get { return "Laser"; } }

	public bool FireButtonDown (IDamageable target)
	{
		throw new System.NotImplementedException ();
	}

	public bool FireButtonDown (Vector3 target)
	{
		if(!beamEffect.enabled) beamEffect.enabled = true;

		//Vector3 source = coll.ClosestPointOnBounds (target);
		//Vector3 source = aperture.position;
//		Ray ray = new Ray (source, target - source);
//		RaycastHit edge;
//		if (coll.Raycast (ray, out edge, float.MaxValue)) {
//			source = edge.point;
//		}

		Vector3 source = aperture.position;
		target = source + (target - source).normalized * maxDistance;
		Ray ray = new Ray (source, target - source);
		RaycastHit shipHit;
		if (Physics.Raycast (ray, out shipHit, maxDistance, shipMask)) {
			target = shipHit.point;
			UpdateDamageEffect (target, Quaternion.LookRotation ((source - target).normalized));
			shipHit.collider.GetComponent<IDamageable>().ApplyDamage(damagePerSecond * Time.deltaTime);
		} else {
			StopDamageEffect();
		}

		beamEffect.SetPosition (0, source);
		beamEffect.SetPosition (1, target);

		return true;
	}

	public void FireButtonUp ()
	{
		beamEffect.SetPosition (0, aperture.position);
		beamEffect.SetPosition (1, aperture.position);
		beamEffect.enabled = false;
		StopDamageEffect ();
	}

	#endregion

	private void UpdateDamageEffect (Vector3 target, Quaternion dir)
	{
		if (!damageEffect.activeSelf)
			damageEffect.SetActive (true);

		damageEffect.transform.position = target;
		damageEffect.transform.rotation = dir;
	}

	void StopDamageEffect ()
	{
		damageEffect.transform.position = aperture.position;
		damageEffect.SetActive (false);
	}
}
