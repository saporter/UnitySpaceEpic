using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	[SerializeField] float padding = 3f;
	[SerializeField] GameObject player;

	private Vector3 offset;
	
	void Awake()
	{
		offset = transform.position;
		if (player == null)
			player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 newPos = player.transform.position + offset;
		Vector3 dir = newPos - transform.position;
		float distance = dir.magnitude - padding;
		if (distance > 0)
			transform.position = Vector3.MoveTowards (transform.position, newPos, distance);
	}
}
