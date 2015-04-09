using UnityEngine;
using System.Collections;

public class PlacingMovementMarkerEvent : GameEvent {

	public Vector3 position;

	public PlacingMovementMarkerEvent(Vector3 _pos)
	{
		position = _pos;
	}
}
