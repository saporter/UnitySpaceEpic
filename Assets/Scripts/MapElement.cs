using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapElement : MonoBehaviour, IMappable {
	[SerializeField] bool rotateWithObject = false;

	#region IMappable implementation
	[SerializeField] GameObject _follow;
	public GameObject FollowGameObject {
		get {
			return _follow;
		}
		set {
			_follow = value;
		}
	}

	public void SnapToGameObject()
	{
		this.transform.position = new Vector3 (_follow.transform.position.x, transform.parent.position.y, _follow.transform.position.z);
		if(rotateWithObject)
			this.transform.rotation = _follow.transform.rotation * Quaternion.Euler (90f, 0f, 0f);

		
	}
	#endregion



}
