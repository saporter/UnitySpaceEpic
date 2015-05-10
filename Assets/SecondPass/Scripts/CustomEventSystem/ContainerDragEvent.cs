using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ContainerDragEvent : GameEvent {


}

public class BeginContainerDragEvent : ContainerDragEvent {
	public PointerEventData eventData;

	public BeginContainerDragEvent(PointerEventData _eventData){
		eventData = _eventData;
	}
}

public class EndContainerDragEvent : ContainerDragEvent {

}

public class ContainerChangedEvent : ContainerDragEvent 
{
	public GameObject OldSlot;
	public GameObject Item;

	public ContainerChangedEvent(GameObject _oldSlot, GameObject _item)
	{
		OldSlot = _oldSlot;
		Item = _item;
	}
}