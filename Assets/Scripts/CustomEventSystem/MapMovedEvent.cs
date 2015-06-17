using UnityEngine;
using System.Collections;

public class MapMovedEvent : GameEvent {
	public float currentSize;
	public float oldSize;

	public MapMovedEvent(float _size, float _old)
	{
		this.currentSize = _size;
		this.oldSize = _old;
	}

	public MapMovedEvent()
	{
	}
}
