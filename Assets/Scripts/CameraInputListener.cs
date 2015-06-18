using UnityEngine;
using System.Collections;

public class CameraInputListener : MonoBehaviour, ICameraInputListenable {
 	bool mouseOver;

	void Awake()
	{
		mouseOver = false;
	}

	#region ICameraInputListenable implementation

	public IEnumerator ListenForInput (Camera c, Transform gameMenu, Transform mapMenu, float zoomSpeed, float minSize, float maxSize)
	{
		float zoom;
		float deltaTime = Time.realtimeSinceStartup;
		Vector3? mousePos = null;	// To determine mouse move delta
		float drag = 0f;
		
		// Listen for player input while Map is shown
		while (gameMenu.gameObject.activeInHierarchy && gameMenu.GetChild(gameMenu.childCount - 1) == mapMenu) {
			zoom = -Input.GetAxisRaw ("Mouse ScrollWheel");
			zoom = zoom > 0f ? 1f : zoom < 0f ? -1f : 0f;		// Why doesn't GetAxisRaw return +-1 for "Mouse ScrollWheel"???
			drag = Input.GetAxisRaw("Drag");
			
			// If scrollwheel input, zoom by changing orthographic camera size.
			if (zoom != 0) {
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

			// If drag input, change camera location
			if(drag > 0f && mouseOver)
			{
				if(mousePos != null){
					Vector3 move = mousePos.Value  - Input.mousePosition; 
					float width = GetComponent<RectTransform>().rect.width;
					float height = GetComponent<RectTransform>().rect.height;

					// Calculate move vector along xz axis, and multiply scalar value to move map proportionally with mouse.
					move = new Vector3(move.x * ((c.orthographicSize * c.aspect) * 2f) / width, // x_delta * (orthographicWidth / screen width)
					                   0f, 
					                   move.y * (c.orthographicSize * 2 / height));				// y_delta * (orthographicHeight / screen height)
					c.transform.position += move;
					mousePos = Input.mousePosition;

					// Yield one frame to allow camera screen to update...
					yield return new WaitForEndOfFrame();
					// ...so anyone who listens for event will have updated camera position
					Events.instance.Raise(new MapMovedEvent());
					 
				}
				else{
					mousePos = Input.mousePosition;
				}

			}else{
				mousePos = null;
			}

			// Record realtimeSinceStartup b/c we cannot use Time.deltatime since game is paused via Time.timescale = 0.0
			deltaTime = Time.realtimeSinceStartup;

			// Yield to next frame.  Time.timescale = 0.0 forces us to use WaitForEndOfFrame IOT receive a call back.
			yield return new WaitForEndOfFrame();
		}
		
	}

	#endregion

	// Called by EventTrigger (in Editor)
	public void PointerEnter()
	{
		mouseOver = true;
	}

	// Called by EventTrigger (in Editor)
	public void PointerExit()
	{
		mouseOver = false;
	}

}
