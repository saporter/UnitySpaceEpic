using UnityEngine;
using System.Collections;

public class DockigPort : MonoBehaviour {
	GameObject player;
	Coroutine docking;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		docking = null;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject == player) {
			if(docking != null)
				StopCoroutine(docking);
			docking = StartCoroutine (ListenForDocking());
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject == player) {
			Events.instance.Raise (new PlayerDockedEvent (false));
			if(docking != null){
				StopCoroutine(docking);
				docking = null;
			}
		}
	}

	IEnumerator ListenForDocking()
	{
		do {
			if (Input.GetAxisRaw ("Dock") != 0f) {
				player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				player.transform.position = transform.parent.position;
				player.transform.rotation = transform.parent.rotation;
				Events.instance.Raise (new PlayerDockedEvent (true));
			}
			yield return new WaitForFixedUpdate ();
		} while(docking != null);
	}

}
