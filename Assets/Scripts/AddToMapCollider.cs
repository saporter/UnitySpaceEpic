using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class AddToMapCollider : MonoBehaviour {
	static GameObject mapCanvas;

	[SerializeField] GameObject mapElement;
	[SerializeField] string text;


	bool elementAdded;

	void Awake()
	{
		if (mapCanvas == null) {
			GameObject map = GameObject.FindGameObjectWithTag ("Map Canvas");
			if(map == null)
				Debug.LogError ("No Map Canvas found in Scene");
			mapCanvas = map;
		}

		elementAdded = false;
		if (mapElement == null)
			Debug.LogError ("No mapElement attached.  Attached valid Map Element prefab for " + transform.parent != null ? transform.parent.gameObject.name : "<no parent>");
	}

	void OnTriggerEnter(Collider other)
	{
		if (elementAdded)
			return;


		 if(other.gameObject.tag == "Aperture Mask"){

			//Debug.Log("Adding map element for " + gameObject.name);
			GameObject clone = Instantiate(mapElement) as GameObject;
			clone.transform.SetParent(mapCanvas.transform, false);
			clone.GetComponent<IMappable>().FollowGameObject = gameObject;
			if(gameObject.tag != "Player")
				clone.transform.SetAsFirstSibling();

			if(clone.transform.childCount > 0)
				clone.transform.GetChild(0).gameObject.GetComponent<Text>().text = this.text;

			clone.GetComponent<IMappable>().SnapToGameObject();

			elementAdded = true;
		}
	}
}
