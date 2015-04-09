using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorController : MonoBehaviour, ISelectable {
	private Outline outline;
	private SelectableFunction _selected;

	public SelectableFunction Selected { get { return _selected;} set { _selected = value;}} 

	void Awake()
	{
		outline = GetComponent<Outline> ();
		outline.enabled = false;
	}

	public void OnMouseEnter()
	{
		outline.enabled = true;
	}

	public void OnMouseExit()
	{
		outline.enabled = false;
	}

	public void OnMouseClick()
	{
		if(Selected != null)
			Selected ();
		else {
			Debug.Log ("Selected delegate not set in SelectorController for " + gameObject.name);
		}
	}
}

