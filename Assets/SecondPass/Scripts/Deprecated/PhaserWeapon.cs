using UnityEngine;
using System.Collections;

public class PhaserWeapon : MonoBehaviour, IWeapon {
	#region IWeapon implementation

	bool IWeapon.FireButtonDown (IDamageable target, IShipSystems systems)
	{
		throw new System.NotImplementedException ();
	}

	bool IWeapon.FireButtonDown (Vector3 target, IShipSystems systems)
	{
		throw new System.NotImplementedException ();
	}

	bool IWeapon.UpdateWeapon (Vector3 target, IShipSystems systems)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	private LineRenderer beamEffect;

	public string Name { get { return "Phaser"; } }
	public GameObject damageEffect;
	public GameObject exitAperture;
	public float effectDuration = .5f;
	public float maxDamage = 10f;
	public float minDamage = 2f;
	public float startFalloff = 3f;
	public float endFalloff = 8f;

	// Use this for initialization
	void Awake () {
		beamEffect = GetComponent<LineRenderer> ();
		if (beamEffect == null)
			Debug.LogError ("Attach LineRenderer Component to PhaserWeapon.  beamEffect is still null");
		if( damageEffect == null)
			Debug.Log ("No damage effect added for " + name);

		beamEffect.enabled = false;
	}

	public void FireButtonDown(IDamageable target, IShipSystems systems)
	{	
		// Determine hit location of target
		Collider collider = target.GameObj.GetComponent<Collider> ();
		Ray ray = new Ray (exitAperture.transform.position, collider.transform.position - exitAperture.transform.position);
		RaycastHit hitInfo;
		Vector3 hit = target.GameObj.transform.position;
		if (collider.Raycast (ray, out hitInfo, ray.direction.magnitude + 10f)) {
			hit = hitInfo.point;
		}

		// Start damage effect
		Quaternion rot = Quaternion.LookRotation ((transform.position - hit).normalized);
		if (damageEffect) {
			GameObject effect = Instantiate (damageEffect, hit, rot) as GameObject;
			Destroy (effect, effectDuration);
		}

		// Start weapon beam effect
		beamEffect.enabled = true;
		beamEffect.useWorldSpace = true;
		beamEffect.SetPosition (0, exitAperture.transform.position);
		beamEffect.SetPosition (1, hit);
		
		// Start handler for deactivating beam effect
		StartCoroutine (PhaserOffAndDamage(effectDuration, target));
	}

	public void FireButtonDown(Vector3 target, IShipSystems systems)
	{
		throw new System.NotImplementedException ();
	}

	public void FireButtonUp()
	{
		throw new System.NotImplementedException ();
	}

	public void UpdateWeapon(Vector3 target, IShipSystems systems)
	{
		throw new System.NotImplementedException ();
	}

	IEnumerator PhaserOffAndDamage(float inSeconds, IDamageable toTarget){
		yield return new WaitForSeconds(inSeconds);

		beamEffect.SetPosition (0, exitAperture.transform.position);
		beamEffect.SetPosition (1, exitAperture.transform.position);
		beamEffect.useWorldSpace = false;
		beamEffect.enabled = false;

		float dist = (toTarget.GameObj.transform.position - exitAperture.transform.position).magnitude;
		if (dist <= startFalloff) {
			toTarget.ApplyDamage (maxDamage);
		} else if (dist >= endFalloff) {
			toTarget.ApplyDamage (minDamage);
		} else {
			float modMaxRange = endFalloff - startFalloff;
			float damage = (((modMaxRange - (dist - startFalloff)) / modMaxRange) * (maxDamage - minDamage)) + minDamage;
			toTarget.ApplyDamage(damage);
		}
	}
}
