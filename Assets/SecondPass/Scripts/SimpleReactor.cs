using UnityEngine;
using System.Collections;

public class SimpleReactor : MonoBehaviour, IPowerSource {

	[SerializeField] 
	private float _powerCapacity = 20f;
	
	public float PowerCapacity{
		get {
			return _powerCapacity;
		}
	}
}
