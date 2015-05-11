using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IDropHandler {
	[SerializeField] string holdsSystemType;

	private CanvasGroup canvasGroup;

	void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup> ();
		Events.instance.AddListener<BeginContainerDragEvent> (OnBeginDrag);
		Events.instance.AddListener<EndContainerDragEvent> (OnEndDrag);
	}

	void OnDestroy()
	{
		Events.instance.RemoveListener<BeginContainerDragEvent> (OnBeginDrag);
		Events.instance.RemoveListener<EndContainerDragEvent> (OnEndDrag);
	}

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			return;
		if (transform.childCount == 0){
			GameObject movingFrom = eventData.pointerDrag.transform.parent.gameObject != null ? eventData.pointerDrag.transform.parent.gameObject : null;
			eventData.pointerDrag.transform.SetParent(transform);
			Events.instance.Raise(new ContainerChangedEvent(movingFrom, eventData.pointerDrag));
		}
	}

	#endregion

	void OnBeginDrag (BeginContainerDragEvent e)
	{
		if (holdsSystemType == "" || e.eventData.pointerDrag.GetComponent<IContainer> ().Contents.GetComponent (holdsSystemType) != null) {
			return;
		}
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = .5f;
	}

	void OnEndDrag (EndContainerDragEvent e)
	{
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1f;
	}

}
