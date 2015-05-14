using UnityEngine;
using System.Collections;

public class RefreshPlanetsOnAwake : MonoBehaviour {

	void Awake()
	{
		this.gameObject.GetComponent<F3DSun>().Planets = FindObjectsOfType<F3DPlanet>();
	}
}
