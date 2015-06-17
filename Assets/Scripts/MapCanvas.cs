using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using System.Collections.Generic;

public class MapCanvas : MonoBehaviour, IMapCanvas {
	[SerializeField] Camera mapCamera;
	[SerializeField] GameObject textCanvasUI;

	#region IMapCanvas implementation
	public GameObject TextCanvas {
		get {
			return textCanvasUI;
		}
	}

	public Camera MapCamera {
		get {
			return mapCamera;
		}
	}
	#endregion

	void Awake()
	{
		if (textCanvasUI == null)
			Debug.LogError ("TextCanvasUI is null in " + gameObject.name + ". Add reference to Map Canvas (text) in Editor.");
	}



	// Called by button event (under Map tab in GameMenu
	public void RefreshMap()
	{
		// Update all Map Elements
		IMappable[] mapElements = GetComponentsInChildren<IMappable> ();
		foreach (IMappable e in mapElements) {
			e.SnapToGameObject ();
		}

		// Let the Camera know the map is active (which will also update Map Elements on next frame... so maybe previous code is redundant?
		mapCamera.GetComponent<IMapCamera> ().MapOpenned ();
	}


	
}
