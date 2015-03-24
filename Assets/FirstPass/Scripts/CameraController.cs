using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float speed;

	private Rigidbody cameraRigidbody;

	// Use this for initialization
	void Start () {
		cameraRigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		Vector3 movement = new Vector3 (horizontal, 0f, vertical);
		movement.Normalize ();

		cameraRigidbody.MovePosition (transform.position + movement * speed * Time.deltaTime);
	}
}
