using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	float timer;
	Coroutine c;

	void Awake()
	{
		timer = 0f;
		c = null;
	}

	void Update()
	{
		if (transform.position.z < 0f && c == null) {
			timer = 0f;
			c = StartCoroutine (MyTimer ());
		}
		else if (transform.position.z > 0f && c != null) {
			StopCoroutine(c);
			c = null;
			Debug.Log("Final Time is: " + timer);
		}
	}

	IEnumerator MyTimer()
	{
		Debug.Log ("Timer started...");
		while (true) {
			timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
	}
}
