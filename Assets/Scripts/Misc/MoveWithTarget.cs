using UnityEngine;
using System.Collections;

public class MoveWithTarget : MonoBehaviour {
	public Transform Target;

	Vector3 offset;

	void Awake()
	{
		if (Target == null)
			Target = Camera.main.transform;
		offset = Target.transform.position - transform.position;
	}

	void FixedUpdate()
	{
		transform.position = Target.transform.position - offset;
	}
}
