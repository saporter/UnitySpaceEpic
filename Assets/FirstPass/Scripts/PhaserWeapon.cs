using UnityEngine;
using System.Collections;

public class PhaserWeapon : MonoBehaviour {
	private int chargesLeftThisTurn;
	private LineRenderer beamEffect;

	public int ChargesLeft
	{
		get { return chargesLeftThisTurn; }
	}
	public GameObject Owner
	{
		get { return transform.parent.gameObject;}
	}

	public GameObject exitAperture;
	public Material phaserMaterial;
	public float fieldOfFireAngle = 180f;		// angle from the forward vector of the exitAperture to include in valid FOF
	public float maxDamage = 10f;
	public float minDamage = 1f;
	public float dropOffEndRange = 7f;
	public float dropOffStartRange = 2f;
	public float effectDuration = .5f;
	public float powerCost = 5f;
	public int chargesPerTurn = 1;

	// Use this for initialization
	void Start () {
		beamEffect = gameObject.AddComponent<LineRenderer>() as LineRenderer;
		beamEffect.enabled = false;
		beamEffect.material = phaserMaterial;
		beamEffect.SetWidth (.1f, .1f);
		beamEffect.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		chargesLeftThisTurn = chargesPerTurn;

		Events.instance.AddListener<TransitionToPlayerTurnEvent> (OnNewTurn);
	}
	
	// Update is called once per frame
	void Update () {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		int floorMask = LayerMask.GetMask ("Floor");
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, 300f, floorMask)) 
		{
			Color headingColor = Color.red;
			Vector3 heading = (floorHit.point - exitAperture.transform.position).normalized;
			Vector3 flatHeading = new Vector3(heading.x, 0f, heading.z);
			float attack = Vector3.Angle (exitAperture.transform.forward, flatHeading);
			float dir = Vector3.Dot (Vector3.Cross (exitAperture.transform.forward, flatHeading), exitAperture.transform.up);
			attack = dir < 0 ? 360f - attack : attack;

			if(attack <= fieldOfFireAngle){
				headingColor = Color.blue;
			}

			Debug.DrawRay (exitAperture.transform.position, flatHeading, headingColor);
			Debug.DrawRay (exitAperture.transform.position, exitAperture.transform.forward);
			Debug.DrawRay (exitAperture.transform.position, Quaternion.AngleAxis(fieldOfFireAngle, transform.up) * exitAperture.transform.forward, Color.green);

//			if(gameObject.name == "Phaser starboard" && transform.parent.tag == "Player"){
//				Debug.Log(InFieldOfFire(flatHeading));
//			}
		}
	}

	public bool CanFire(Vector3 hit)
	{
		if (ChargesLeft <= 0)
			return false;

//		// Get direction of target as Vector3
//		Vector3 heading = (hit - exitAperture.transform.position).normalized; 
//		Vector3 flatHeading = new Vector3(heading.x, 0f, heading.z);
//
//		// Determine if direction is within field of fire using angles
//		float headingAngle = Vector3.Angle (exitAperture.transform.forward, flatHeading);
//		float rotDir = Vector3.Dot (Vector3.Cross (exitAperture.transform.forward, flatHeading), exitAperture.transform.up);
//		headingAngle = rotDir < 0 ? 360f - headingAngle : headingAngle;

		//return headingAngle <= fieldOfFireAngle;
		return InFieldOfFire (hit - exitAperture.transform.position) == 0f;
	}

	public float InFieldOfFire(Vector3 heading)
	{
		heading.Normalize ();
		heading = new Vector3(heading.x, 0f, heading.z);	// flatten

		float headingAngle = Vector3.Angle (exitAperture.transform.forward, heading);
		float rotDir = Vector3.Dot (Vector3.Cross (exitAperture.transform.forward, heading), exitAperture.transform.up);
		headingAngle = rotDir < 0 ? 360f - headingAngle : headingAngle;
		
		return headingAngle <= fieldOfFireAngle ? 0f : (headingAngle - fieldOfFireAngle) > (360f - fieldOfFireAngle) / 2f ? headingAngle - 360f : headingAngle - fieldOfFireAngle;
	}

	public void Fire(GameObject target, Vector3 hit)
	{
		if (!CanFire (hit)) 
		{
			return;
		}

		// Get ship module to attack
		ShipModule1 enemy = target.GetComponent<ShipModule1> ();

		// Start target damage effect
		Quaternion rot = Quaternion.LookRotation ((transform.position - hit).normalized) ;
		GameObject effect = Instantiate (enemy.damageEffect, hit, rot) as GameObject;
		effect.GetComponent<DestroyByTime> ().alive = effectDuration;

		// Start weapon beam effect
		beamEffect.enabled = true;
		beamEffect.SetPosition (0, exitAperture.transform.position);
		beamEffect.SetPosition (1, hit);
		--chargesLeftThisTurn;

		// Start handler for deactivating beam effect
		StartCoroutine ("PhaserOff", target);
		
	}
	
	IEnumerator PhaserOff(GameObject target)
	{
		// Wait effectDuration then end the effect
		yield return new WaitForSeconds (effectDuration);
		beamEffect.enabled = false;

		// Calculate damage and apply to target
		ShipModule1 enemy = target.GetComponent<ShipModule1> ();
		float dist = (enemy.transform.position - exitAperture.transform.position).magnitude;
		if (dist <= dropOffStartRange) {
			enemy.doDamage (maxDamage);
		} else if (dist >= dropOffEndRange) {
			enemy.doDamage (minDamage);
		} else {
			float modMaxRange = dropOffEndRange - dropOffStartRange;
			float damage = (((modMaxRange - (dist - dropOffStartRange)) / modMaxRange) * (maxDamage - minDamage)) + minDamage;
			enemy.doDamage(damage);
		}
	}

	void OnNewTurn(TransitionToPlayerTurnEvent e)
	{
		chargesLeftThisTurn = chargesPerTurn;
	}
}