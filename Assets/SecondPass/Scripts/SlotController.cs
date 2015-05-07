using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IDropHandler {
	public GameObject Item {
		get {
			if(transform.childCount > 0) return transform.GetChild(0).gameObject;
			else return null;
		}
	}

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			return;
		if (!Item) {
			eventData.pointerDrag.transform.SetParent(transform);
		}
	}

	#endregion
}
