using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public float maxEffective;
	public float phaserOnTime = .5f;

	private LineRenderer phaser;

	// Use this for initialization
	void Start () {
		phaser = GetComponent<LineRenderer> ();
	}

	public void Fire(GameObject target, Vector3 hit)
	{
		StarshipModule enemy = target.GetComponent<StarshipModule> ();
		Quaternion rot = Quaternion.LookRotation ((transform.position - hit).normalized) ;
		GameObject effect = Instantiate (enemy.damageEffect, hit, rot) as GameObject;

		phaser.enabled = true;
		phaser.SetPosition (0, transform.position);
		phaser.SetPosition (1, hit);
		effect.GetComponent<DestroyByTime> ().alive = phaserOnTime;

		StartCoroutine ("PhaserOff", target);

	}

	IEnumerator PhaserOff(GameObject target)
	{
		yield return new WaitForSeconds (phaserOnTime);
		phaser.enabled = false;

		StarshipModule enemy = target.GetComponent<StarshipModule> ();
		Instantiate (enemy.destroyedEffect, enemy.transform.position, enemy.transform.rotation);

		Destroy (target);
	} 
}

class PhaserArguments : Object 
{
	public GameObject target;
	public Vector3 hitPoint;

	public PhaserArguments(GameObject o, Vector3 v){
		target = o;
		hitPoint = v;
	}
}