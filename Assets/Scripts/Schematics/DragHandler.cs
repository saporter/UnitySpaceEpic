using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	Vector3 startPosition;
	Transform startParent;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			return;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
		Events.instance.Raise (new BeginContainerDragEvent (eventData));
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			return;
		transform.position = Input.mousePosition;
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			return;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if(transform.parent == startParent)
			transform.position = startPosition;
		Events.instance.Raise (new EndContainerDragEvent());
	}

	#endregion

}
