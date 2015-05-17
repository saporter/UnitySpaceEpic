using UnityEngine;
using System.Collections;

public class EnableOnCollision : MonoBehaviour {
	int counter;

	void Awake()
	{
		counter = 0;
	}

	void Start()
	{
		if (counter <= 0) {
			SetChildrenActive(false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<IMapEnabler> () != null) {
			if(++counter <= 1){
				SetChildrenActive(true);
			}


			Debug.Log("Map Enablers entered: " + counter);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.GetComponent<IMapEnabler> () != null) {
			if(--counter <= 0){
				SetChildrenActive(false);
			}
			Debug.Log("Map Enablers left: " + counter);
		}
	}

	void SetChildrenActive(bool active)
	{
		for(int i = 0; i < transform.childCount; ++i){
			transform.GetChild(i).gameObject.SetActive(active);
		}
	}
}
