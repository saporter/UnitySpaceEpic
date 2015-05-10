using UnityEngine;
using System.Collections;

public class StarshipModule : MonoBehaviour {
	public float currentHealth = 10f;
	public float maxHealth = 10f;
	public GameObject damageEffect;
	public GameObject destroyedEffect;
	public bool criticalModule = false;		// If this module is destroyed, the parent module is destroyed

	public void doDamage(float damage)
	{
		Debug.Log ("Damage done: " + damage);
		currentHealth -= damage;
		if (currentHealth <= 0f) {
			DestroyModule();
		}
	}

	void DestroyModule()
	{
		Instantiate (destroyedEffect, transform.position, transform.rotation);
		if (criticalModule) {
			GameObject g = Instantiate (destroyedEffect, transform.position, transform.rotation) as GameObject;
			g.GetComponent<ParticleSystem> ().startDelay = .5f;
			Destroy (transform.parent.gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}
