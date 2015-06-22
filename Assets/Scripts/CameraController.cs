using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	[SerializeField] float padding = 3f;
	[SerializeField] GameObject player;
	[SerializeField] float minZoom = 10f;
	[SerializeField] float maxZoom = 5f;

	private Vector3 offset;
	
	void Awake()
	{
		offset = transform.position;
		if (player == null)
			player = GameObject.FindGameObjectWithTag ("Player");
	}
	void Start()
	{
		LoadVars ();
	}
	
	void LoadVars()
	{
		IXmlLoader XmlLoader = GameObject.FindGameObjectWithTag ("XmlLoader").GetComponent<IXmlLoader>();
		
		if (!XmlLoader.FloatVars.TryGetValue ("CameraController.Padding", out padding))
			Debug.LogError ("Could not find CameraController.Padding from XmlLoader");
		if (!XmlLoader.FloatVars.TryGetValue ("CameraController.MinZoom", out minZoom))
			Debug.LogError ("Could not find CameraController.MinZoom from XmlLoader");
		if (!XmlLoader.FloatVars.TryGetValue ("CameraController.MaxZoom", out maxZoom))
			Debug.LogError ("Could not find CameraController.MaxZoom from XmlLoader");
	}

	void FixedUpdate()
	{
		if (player == null)
			return;
		
		Vector3 newPos = player.transform.position + offset;
		Vector3 dir = newPos - transform.position;
		float distance = dir.magnitude - padding;
		if (distance > 0)
			transform.position = Vector3.MoveTowards (transform.position, newPos, distance);
		
		float zoom = -Input.GetAxisRaw ("Mouse ScrollWheel");
		zoom = zoom > 0f ? 1f : zoom < 0f ? -1f : 0f;		// Why doesn't GetAxisRaw return +-1 for "Mouse ScrollWheel"???
		if (zoom != 0) {
			Camera c = GetComponent<Camera> ();
			c.orthographicSize += zoom;
			if(c.orthographicSize < maxZoom) c.orthographicSize = maxZoom;
			else if(c.orthographicSize > minZoom) c.orthographicSize = minZoom;
		}
	}

}
