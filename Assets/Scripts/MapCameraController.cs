using UnityEngine;
using System.Collections;

public class MapCameraController : MonoBehaviour, IMapCamera {
	[SerializeField] float padding = 3f;
	[SerializeField] GameObject player;
	[SerializeField] float maxSize = 10f;
	[SerializeField] float minSize = 5f;
	[SerializeField] GameObject MapUI;
	[SerializeField] float zoomSpeed = 5f;
	
	private Vector3 offset;
	private Coroutine listener;
	
	void Awake()
	{
		offset = transform.position;
		if (player == null)
			player = GameObject.FindGameObjectWithTag ("Player");
		if (MapUI == null)
			Debug.LogError ("MapUI is null in " + gameObject.name + ". Assign MapUI to the Map tap GameObject in Game Menu.");
	}

	void SnapToPlayer()
	{
		if (player == null)
			return;
		
		Vector3 newPos = player.transform.position + offset;
		Vector3 dir = newPos - transform.position;
		float distance = dir.magnitude - padding;
		if (distance > 0)
			transform.position = Vector3.MoveTowards (transform.position, newPos, distance);
	}

	#region IMapCamera implementation

	public void MapOpenned()
	{
		SnapToPlayer ();
		if (listener != null)
			StopCoroutine (listener);
		listener = StartCoroutine (ListenForInput ());
	}

	#endregion




	IEnumerator ListenForInput ()
	{
		Transform menu = MapUI.transform.parent;
		float zoom;
		float deltaTime = Time.realtimeSinceStartup;
		while (menu.gameObject.activeInHierarchy && menu.GetChild(menu.childCount - 1) == MapUI.transform) {
			zoom = -Input.GetAxisRaw ("Mouse ScrollWheel");
			zoom = zoom > 0f ? 1f : zoom < 0f ? -1f : 0f;		// Why doesn't GetAxisRaw return +-1 for "Mouse ScrollWheel"???
			if (zoom != 0) {
				Camera c = GetComponent<Camera> ();
				deltaTime = Time.realtimeSinceStartup - deltaTime;
				c.orthographicSize += zoom * zoomSpeed * deltaTime;
				if(c.orthographicSize < minSize) c.orthographicSize = minSize;
				else if(c.orthographicSize > maxSize) c.orthographicSize = maxSize;
			}

			deltaTime = Time.realtimeSinceStartup;
			yield return new WaitForEndOfFrame();
		}

		Debug.Log("Exiting: " + menu.GetChild(menu.childCount - 1).gameObject.name);

	}
}
