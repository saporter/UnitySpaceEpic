using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleEnemyController : MonoBehaviour {
	private IDisplayableOnGUI[] GUIDisplays;

	public float ScanningScope = 45f;
	public LayerMask shipMask;
	public LayerMask floorMask;

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
		foreach(IDisplayableOnGUI s in GUIDisplays)
			s.Show ();
	}

	void OnMouseExit()
	{
		foreach(IDisplayableOnGUI s in GUIDisplays)
			s.Hide ();
	}
 
}
