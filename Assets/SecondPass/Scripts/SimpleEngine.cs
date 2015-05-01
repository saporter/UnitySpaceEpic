using UnityEngine;
using System.Collections;

public class SimpleEngine : MonoBehaviour, IEngine {
	[SerializeField] 
	private float _engineCapacity = 15f;

	public float EngineCapacity{
		get {
			return _engineCapacity;
		}
	}
}
