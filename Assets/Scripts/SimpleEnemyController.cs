using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleEnemyController : MonoBehaviour {
	private IDisplayableOnGUI[] GUIDisplays;

	public float ScanningScope = 45f;

	void Awake()
	{
		GUIDisplays = GetComponentsInChildren<IDisplayableOnGUI> ();
		foreach (IDisplayableOnGUI d in GUIDisplays) 
		{
			d.ShowOnPlayerMovement(true);
		}
	}

	void OnMouseEnter()
	{
		if (Input.GetMouseButton (1))
			return;

		GameObject player = GameObject.FindWithTag ("Player");
		if (player == null) {
			Debug.Log("Could not find player.  \"Player\" tag returned null.");
			return;
		}

		GUIDisplays = GetComponentsInChildren<IDisplayableOnGUI> ();
		foreach (IDisplayableOnGUI s in GUIDisplays)
			s.ShowIfVisibleFrom (player.transform.position);
	}

	void OnMouseExit()
	{
		GUIDisplays = GetComponentsInChildren<IDisplayableOnGUI> ();
		foreach(IDisplayableOnGUI s in GUIDisplays)
			s.Hide ();
	}
 
}
