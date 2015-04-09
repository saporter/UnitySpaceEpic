using UnityEngine;
using System.Collections;

public class MovementMarkerPlacedEvent : GameEvent {
	public Vector3 position;
	
	public MovementMarkerPlacedEvent(Vector3 _pos)
	{
		position = _pos;
	}
}
