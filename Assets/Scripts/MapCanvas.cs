using UnityEngine;
using System.Collections;

public class MapCanvas : MonoBehaviour {
	[SerializeField] Camera MapCamera;


	public void RefreshMap()
	{
		IMappable[] mapElements = GetComponentsInChildren<IMappable> ();
		foreach (IMappable e in mapElements) {
			e.SnapToGameObject ();
		}
		MapCamera.GetComponent<IMapCamera> ().MapOpenned ();
	}
	
}
