using UnityEngine;
using System.Collections;

public class BackgroundLeaper : MonoBehaviour, IMapEnabler {
	static float maxDistance = 0f;
	GameObject player;

	void Awake()
	{
		player = GameManager.GM.Player;

		float myDistance = transform.position.x - player.transform.position.x;
		maxDistance = myDistance > maxDistance ? myDistance : maxDistance;
	}

	void FixedUpdate()
	{
		if (player) {
			float dist = player.transform.position.x - transform.position.x;
			if (Mathf.Abs (dist) > maxDistance) {
				if (dist < 0f)
					transform.position = new Vector3 (transform.position.x - (2 * maxDistance), transform.position.y, transform.position.z);
				else
					transform.position = new Vector3 (transform.position.x + (2 * maxDistance), transform.position.y, transform.position.z);
			}

			dist = player.transform.position.z - transform.position.z;
			if (Mathf.Abs (dist) > maxDistance) {
				if (dist < 0f)
					transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - (2 * maxDistance));
				else
					transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + (2 * maxDistance));
			}
		}
	}
}
