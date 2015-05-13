using UnityEngine;
using System.Collections;

public class PlayerDockedEvent : GameEvent {
	public bool playerDocked;

	public PlayerDockedEvent(bool playerEnteringDock){
		playerDocked = playerEnteringDock;
	}

}
