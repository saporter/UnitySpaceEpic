using UnityEngine;
using System.Collections;

public class BackgroundLeaper : MonoBehaviour {
	static float maxDistance = 0f;
	GameObject player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		float myDistance = transform.position.x - player.transform.position.x;
		maxDistance = myDistance > maxDistance ? myDistance : maxDistance;
	}

	void Update()
	{
		float dist = player.transform.position.x - transform.position.x;
		if (Mathf.Abs (dist) > maxDistance) {
			if(dist < 0f)
				transform.position = new Vector3(transform.position.x - (2 * maxDistance), transform.position.y, transform.position.z);
			else
				transform.position = new Vector3(transform.position.x + (2 * maxDistance), transform.position.y, transform.position.z);
		}

		dist = player.transform.position.z - transform.position.z;
		if (Mathf.Abs (dist) > maxDistance) {
			if(dist < 0f)
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (2 * maxDistance));
			else
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (2 * maxDistance));
		}
	}
}
