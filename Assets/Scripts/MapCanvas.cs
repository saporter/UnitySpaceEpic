using UnityEngine;
using System.Collections;

public class MapCanvas : MonoBehaviour {

	public void RefreshMap()
	{
		IMappable[] mapElements = GetComponentsInChildren<IMappable> ();
		foreach (IMappable e in mapElements) {
			e.SnapToGameObject ();

		}
	}
	
}
