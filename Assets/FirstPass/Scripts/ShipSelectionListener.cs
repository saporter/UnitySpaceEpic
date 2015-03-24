using UnityEngine;
using System.Collections;

public class ShipSelectionListener : MonoBehaviour {
	public GameObject selectionDisk;
	// Use this for initialization
	void Start () {
		Events.instance.AddListener<ClearSelectionKeyPressEvent> (Unselected);
	}

	void OnMouseUp()
	{
		selectionDisk.SetActive (true);
		Events.instance.Raise (new ShipSelectedEvent (gameObject));
	}

	void Unselected(ClearSelectionKeyPressEvent e)
	{
		selectionDisk.SetActive (false);
	}
}

public class ShipSelectedEvent : GameEvent
{
	public GameObject selected;

	public ShipSelectedEvent (GameObject _selected)
	{
		selected = _selected;
	}
}
