using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapElement : MonoBehaviour, IMappable {
	[SerializeField] bool rotateWithObject = false;
	GameObject label;
	Vector3 offset;

	void Start()
	{
		// If I have a label, add it to the TextCanvas
		Text childLabel = GetComponentInChildren<Text> ();
		if (childLabel != null) {
			label = childLabel.gameObject;
			IMapCanvas canvas = transform.parent.GetComponent<IMapCanvas>();
			offset = label.transform.localPosition;
			label.transform.SetParent(canvas.TextCanvas.transform);
			label.transform.localScale = Vector3.one;
		}

		Events.instance.AddListener<MapMovedEvent> (MapZoomed);
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<MapMovedEvent> (MapZoomed);
	}

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
		SnapLabel ();
	}
	#endregion

	void MapZoomed(MapMovedEvent e)
	{
		SnapLabel ();
	}

	void SnapLabel()
	{
		// If I have a label...
		if (label != null) {
			// ...move it to this element's position...
			label.transform.position = transform.position;
			// ...and add it's original local offset.
			label.transform.localPosition += offset;
		}
	}



}
