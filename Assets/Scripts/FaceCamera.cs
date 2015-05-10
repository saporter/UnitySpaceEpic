using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		transform.forward = transform.position - Camera.main.transform.position;
	}

}
