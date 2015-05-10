using UnityEngine;
using System.Collections;

public class ShipDamagedEvent : GameEvent {
	public GameObject Ship;

	public ShipDamagedEvent(GameObject _ship)
	{
		Ship = _ship;
	}
}
