using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
	public float alive;

	void Start()
	{
		Destroy (gameObject, alive);
	}
}
