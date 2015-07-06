using UnityEngine;
using System.Collections;

public class UIContainer : MonoBehaviour, IContainer {
	#region IContainer implementation
	[SerializeField] private GameObject _contents;

	public GameObject Contents {
		get { return _contents; }
		set { _contents = value; }
	}
	#endregion
	
}
