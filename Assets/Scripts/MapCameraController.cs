using UnityEngine;
using System.Collections;

public class MapCameraController : MonoBehaviour, IMapCamera {
	[SerializeField] float padding = 3f;
	[SerializeField] GameObject player;
	[SerializeField] float maxSize = 10f;
	[SerializeField] float minSize = 5f;
	[SerializeField] GameObject MapUI;
	[SerializeField] GameObject MapScreen;
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

	// Center map over player IAW padding tolerance
	void SnapToPlayer()
	{
		if (player == null)
			return;
		
		Vector3 newPos = player.transform.position + offset;
		Vector3 dir = newPos - transform.position;
		float distance = dir.magnitude - padding;
		if (distance > 0) {
			transform.position = Vector3.MoveTowards (transform.position, newPos, distance);

			// Need to raise MapMovedEvent on next frame so anyone we notify will have updated camera position.
			StartCoroutine(NotifyMapMove());
		}

	}

	// Raises MapMovedEvent() on next frame
	IEnumerator NotifyMapMove()
	{
		yield return new WaitForEndOfFrame ();
		Events.instance.Raise(new MapMovedEvent());
	}

	#region IMapCamera implementation


	public void MapOpenned()
	{
		// Center map over player IAW padding tolerance
		SnapToPlayer ();

		// Start listening coroutine for player map input
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

		// Listen for player input while Map is shown
		while (menu.gameObject.activeInHierarchy && menu.GetChild(menu.childCount - 1) == MapUI.transform) {
			zoom = -Input.GetAxisRaw ("Mouse ScrollWheel");
			zoom = zoom > 0f ? 1f : zoom < 0f ? -1f : 0f;		// Why doesn't GetAxisRaw return +-1 for "Mouse ScrollWheel"???

			// If scrollwheel input, zoom by changing orthographic camera size.
			if (zoom != 0) {
				Camera c = GetComponent<Camera> ();
				deltaTime = Time.realtimeSinceStartup - deltaTime;	// Cannot use Time.deltatime since game is paused via Time.timescale = 0.0 
				float old = c.orthographicSize;
				c.orthographicSize += zoom * zoomSpeed * deltaTime;
				if(c.orthographicSize < minSize) c.orthographicSize = minSize;
				else if(c.orthographicSize > maxSize) c.orthographicSize = maxSize;

				// Yield one frame to allow camera screen to update...
				yield return new WaitForEndOfFrame();
				// ...so anyone who listens for event will have updated camera position
				Events.instance.Raise(new MapMovedEvent(c.orthographicSize, old));
			}

			// Record realtimeSinceStartup b/c we cannot use Time.deltatime since game is paused via Time.timescale = 0.0
			deltaTime = Time.realtimeSinceStartup;

			// Yield to next frame.  Time.timescale = 0.0 forces us to use WaitForEndOfFrame IOT receive a call back.
			yield return new WaitForEndOfFrame();
		}

		// Remove this eventually
		Debug.Log("Exiting: " + menu.GetChild(menu.childCount - 1).gameObject.name);

	}
}
