using UnityEngine;
using System.Collections;

public class JumpToPlayerOnAwake : MonoBehaviour {
	public Vector3 offset;

	void Awake()
	{
		transform.position = GameManager.GM.Player.transform.position + offset;
	}
}
