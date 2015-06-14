using UnityEngine;
using System.Collections;

public class ResetMap : MonoBehaviour {
	int start;

	// Use this for initialization
	void Start () {
		start = 30;
		GetComponent<Camera> ().clearFlags = CameraClearFlags.Color;
		StartCoroutine (Reset ());
	}
	

	IEnumerator Reset () {
		while (start > 0) {
			start--;
			yield return new WaitForEndOfFrame();
		}
		GetComponent<Camera> ().clearFlags = CameraClearFlags.Depth;
	}
}
