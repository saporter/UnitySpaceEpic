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

		// listening routine on MapScreen
		listener = StartCoroutine (MapScreen.GetComponent<ICameraInputListenable>().ListenForInput (GetComponent<Camera>(), MapUI.transform.parent, MapUI.transform, zoomSpeed, minSize, maxSize));
	}

	#endregion



}
